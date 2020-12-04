namespace AsyncSimpleGUIClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConListBox = new System.Windows.Forms.ListBox();
            this.btnChat = new System.Windows.Forms.Button();
            this.btnBlock = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConListBox
            // 
            this.ConListBox.FormattingEnabled = true;
            this.ConListBox.ItemHeight = 12;
            this.ConListBox.Location = new System.Drawing.Point(12, 47);
            this.ConListBox.Name = "ConListBox";
            this.ConListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ConListBox.Size = new System.Drawing.Size(305, 304);
            this.ConListBox.Sorted = true;
            this.ConListBox.TabIndex = 0;
            // 
            // btnChat
            // 
            this.btnChat.Location = new System.Drawing.Point(12, 370);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(75, 23);
            this.btnChat.TabIndex = 1;
            this.btnChat.Text = "채팅하기";
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // btnBlock
            // 
            this.btnBlock.Location = new System.Drawing.Point(242, 370);
            this.btnBlock.Name = "btnBlock";
            this.btnBlock.Size = new System.Drawing.Size(75, 23);
            this.btnBlock.TabIndex = 3;
            this.btnBlock.Text = "차단하기";
            this.btnBlock.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "접속자목록";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "\"\"";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 410);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBlock);
            this.Controls.Add(this.btnChat);
            this.Controls.Add(this.ConListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ConListBox;
        private System.Windows.Forms.Button btnChat;
        private System.Windows.Forms.Button btnBlock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}