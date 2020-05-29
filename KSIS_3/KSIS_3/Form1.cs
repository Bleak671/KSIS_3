using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace KSIS_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const int typeSize = 1;
        const int typeShift = 0;
        const int lengthSize = 2;
        const int lengthShift = 1;
        const int hdrSize = 3;
        const int dataShift = 3;

        public bool conn = false;
        public List<Client> clients = new List<Client>();
        Task UdpRec;
        Task TcpRec;
        public class Client
        {
            public IPEndPoint iep;
            public String name;
            public Client(IPEndPoint iepp, string str)
            {
                iep = iepp;
                name = str;
            }
        }

        class Packet
        {
            public byte type;
            public UInt16 length;
            public byte[] data;

            public Packet(byte typ)
            {
                type = typ;
                length = hdrSize;
                data = null;
            }

            public Packet(byte typ, string dat)
            {
                type = typ;
                data = Encoding.Unicode.GetBytes(dat);
                length = (UInt16)(hdrSize + data.Length);
            }

            public Packet(byte[] dat)
            {
                type = dat[0];
                length = BitConverter.ToUInt16(dat, typeSize);
                data = new byte[length - hdrSize];
                Buffer.BlockCopy(dat, hdrSize, data, 0, length - hdrSize);
            }
            public byte[] getBytes()
            {
                byte[] dat = new byte[length];
                Buffer.BlockCopy(BitConverter.GetBytes(type), 0, dat, typeShift, typeSize);
                Buffer.BlockCopy(BitConverter.GetBytes(length), 0, dat, lengthShift, lengthSize);
                if (data != null)
                    Buffer.BlockCopy(data, 0, dat, dataShift, length - hdrSize);
                return dat;
            }
        }

        public static String GetName(IPEndPoint iep, List<Client> list)
        {
            foreach(Client c in list)
            {
                if (IPEndPoint.Equals(c.iep,iep))
                {
                    return c.name;
                }
            }
            return null;
        }

        private void Connect()
        {
            UdpClient client = new UdpClient();
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("230.230.230.230"), 8005);
            try
            {
                Packet msg = new Packet(1, textBox2.Text);
                client.Send(msg.getBytes(), msg.length, iep);
                client.Close();

                if (!conn)
                {
                    conn = true;
                    UdpRec = new Task(UdpReceive);
                    UdpRec.Start();
                    TcpRec = new Task(TcpReceive);
                    TcpRec.Start();
                }
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                client.Close();
                MessageBox.Show(ex.Message);
            }
        }
        private void UdpReceive()
        {
            while (true)
            {
                UdpClient client = new UdpClient(8005);
                client.JoinMulticastGroup(IPAddress.Parse("230.230.230.230"), 100);
                IPEndPoint iep = null;
                byte[] data = client.Receive(ref iep);
                Packet msg = new Packet(data);
                if (String.Compare(iep.Address.ToString(), GetLocalIPAddress()) == 0)
                    return;
                client.Close();

                Client cl = new Client(new IPEndPoint(iep.Address, 8006), Encoding.Unicode.GetString(msg.data));
                switch (msg.type)
                {
                    case 1:
                        bool flag = true;
                        if (clients.Contains(cl))
                        {
                            flag = false;
                        }
                        if (flag)
                        {
                            clients.Add(cl);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                list.Items.Add(cl.name + "(" + cl.iep.ToString() + ")");
                                listChat.Items.Add(DateTime.Now + " " + cl.name + ": подключился");
                            }));

                            msg = new Packet(1,textBox2.Text);
                            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            sender.Connect(cl.iep.Address, 8006);
                            sender.Send(msg.getBytes(), SocketFlags.None);
                            sender.Close();
                        }
                        break;
                    case 0:
                        foreach (Client c in clients)
                        {
                            if (c.iep.ToString() == cl.iep.ToString())
                            {
                                clients.Remove(c);
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    list.Items.Remove(c.name + "(" + c.iep.ToString() + ")");
                                    listChat.Items.Add(DateTime.Now + " " + c.name + ": покинул чат");
                                }));
                                break;
                            }
                        }
                        break;
                }
            }
        }

        private void TcpReceive()
        {
            while (conn)
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), 8006);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[65536];
                stream.Read(data, 0, data.Length);
                Packet msg = new Packet(data);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()), 8006);

                switch (msg.type)
                {
                    case 1:
                        Client c = new Client(iep, Encoding.Unicode.GetString(msg.data));
                        if (!clients.Contains(c))
                        {
                            clients.Add(c);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                list.Items.Add(c.name + "(" + c.iep.ToString() + ")");
                                listChat.Items.Add(DateTime.Now + " " + c.name + ": присоединился");
                            }));
                        }
                        break;
                    case 2:
                        foreach (Client cl in clients)
                        {
                            if (cl.iep.Equals(iep))
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    listChat.Items.Add(DateTime.Now + " " + cl.name + ": " + Encoding.Unicode.GetString(msg.data));
                                }));
                                break;
                            }
                        }
                        break;
                }
                client.Close();
                stream.Close();
                listener.Stop();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Packet msg = new Packet(2,textBox1.Text);
            listChat.Items.Add(DateTime.Now + " " + "Вы: " + textBox1.Text);
            textBox1.Text = "";
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            foreach (Client c in clients)
            {
                socket.Connect(c.iep);
                socket.Send(msg.getBytes(), SocketFlags.None);
                socket.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (String.Compare(textBox2.Text, "") != 0)
            {
                btnFind.Enabled = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("230.230.230.230"), 8005);
            try
            {
                Packet msg = new Packet(0);
                client.Send(msg.getBytes(), msg.length, ep);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
