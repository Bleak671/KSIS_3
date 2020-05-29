namespace KSIS_3
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listChat = new System.Windows.Forms.ListBox();
            this.list = new System.Windows.Forms.ListBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(470, 307);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 20);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(188, 307);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(276, 20);
            this.textBox1.TabIndex = 1;
            // 
            // listChat
            // 
            this.listChat.FormattingEnabled = true;
            this.listChat.Location = new System.Drawing.Point(188, 10);
            this.listChat.Name = "listChat";
            this.listChat.Size = new System.Drawing.Size(357, 290);
            this.listChat.TabIndex = 4;
            // 
            // list
            // 
            this.list.FormattingEnabled = true;
            this.list.Location = new System.Drawing.Point(22, 10);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(147, 160);
            this.list.TabIndex = 5;
            // 
            // btnFind
            // 
            this.btnFind.Enabled = false;
            this.btnFind.Location = new System.Drawing.Point(41, 203);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(112, 23);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Поиск абонентов";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(22, 177);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(147, 20);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "Имя";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 340);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.list);
            this.Controls.Add(this.listChat);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnSend);
            this.Name = "Form1";
            this.Text = "Одногруппники";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listChat;
        private System.Windows.Forms.ListBox list;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox textBox2;
    }
}

