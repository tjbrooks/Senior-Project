namespace ATCBot_Dev
{
    partial class Twitch_Connect
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
            this.connect = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.userNameBox = new System.Windows.Forms.TextBox();
            this.userNameLbl = new System.Windows.Forms.Label();
            this.oAuthLbl = new System.Windows.Forms.Label();
            this.oAuthBox = new System.Windows.Forms.TextBox();
            this.channelBox = new System.Windows.Forms.TextBox();
            this.chanNameLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.connect.Location = new System.Drawing.Point(79, 103);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 0;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // exit
            // 
            this.exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exit.Location = new System.Drawing.Point(242, 103);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 1;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // userNameBox
            // 
            this.userNameBox.Location = new System.Drawing.Point(103, 12);
            this.userNameBox.Name = "userNameBox";
            this.userNameBox.Size = new System.Drawing.Size(244, 20);
            this.userNameBox.TabIndex = 2;
            // 
            // userNameLbl
            // 
            this.userNameLbl.AutoSize = true;
            this.userNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userNameLbl.Location = new System.Drawing.Point(27, 12);
            this.userNameLbl.Name = "userNameLbl";
            this.userNameLbl.Size = new System.Drawing.Size(70, 15);
            this.userNameLbl.TabIndex = 3;
            this.userNameLbl.Text = "User Name";
            // 
            // oAuthLbl
            // 
            this.oAuthLbl.AutoSize = true;
            this.oAuthLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oAuthLbl.Location = new System.Drawing.Point(57, 43);
            this.oAuthLbl.Name = "oAuthLbl";
            this.oAuthLbl.Size = new System.Drawing.Size(40, 15);
            this.oAuthLbl.TabIndex = 4;
            this.oAuthLbl.Text = "OAuth";
            // 
            // oAuthBox
            // 
            this.oAuthBox.Location = new System.Drawing.Point(103, 43);
            this.oAuthBox.Name = "oAuthBox";
            this.oAuthBox.Size = new System.Drawing.Size(244, 20);
            this.oAuthBox.TabIndex = 5;
            // 
            // channelBox
            // 
            this.channelBox.Location = new System.Drawing.Point(103, 70);
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(244, 20);
            this.channelBox.TabIndex = 6;
            // 
            // chanNameLbl
            // 
            this.chanNameLbl.AutoSize = true;
            this.chanNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chanNameLbl.Location = new System.Drawing.Point(7, 70);
            this.chanNameLbl.Name = "chanNameLbl";
            this.chanNameLbl.Size = new System.Drawing.Size(90, 15);
            this.chanNameLbl.TabIndex = 7;
            this.chanNameLbl.Text = "Channel Name";
            // 
            // Twitch_Connect
            // 
            this.AcceptButton = this.connect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 153);
            this.Controls.Add(this.chanNameLbl);
            this.Controls.Add(this.channelBox);
            this.Controls.Add(this.oAuthBox);
            this.Controls.Add(this.oAuthLbl);
            this.Controls.Add(this.userNameLbl);
            this.Controls.Add(this.userNameBox);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.connect);
            this.Name = "Twitch_Connect";
            this.ShowIcon = false;
            this.Text = "Twitch Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.TextBox userNameBox;
        private System.Windows.Forms.Label userNameLbl;
        private System.Windows.Forms.Label oAuthLbl;
        private System.Windows.Forms.TextBox oAuthBox;
        private System.Windows.Forms.TextBox channelBox;
        private System.Windows.Forms.Label chanNameLbl;
    }
}

