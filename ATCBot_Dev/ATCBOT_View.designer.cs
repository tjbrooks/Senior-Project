
namespace ATCBot_Dev
{
   partial class ATCBOT_View
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonRequestData = new System.Windows.Forms.Button();
            this.richResponse = new System.Windows.Forms.RichTextBox();
            this.conTwitchBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.disTwitchBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(20, 23);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(112, 43);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect to Simulator";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(20, 70);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(112, 43);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect from Simulator";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonRequestData
            // 
            this.buttonRequestData.Location = new System.Drawing.Point(20, 117);
            this.buttonRequestData.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRequestData.Name = "buttonRequestData";
            this.buttonRequestData.Size = new System.Drawing.Size(112, 43);
            this.buttonRequestData.TabIndex = 2;
            this.buttonRequestData.Text = "Data from Simulator";
            this.buttonRequestData.UseVisualStyleBackColor = true;
            this.buttonRequestData.Click += new System.EventHandler(this.buttonRequestData_Click);
            // 
            // richResponse
            // 
            this.richResponse.Location = new System.Drawing.Point(194, 11);
            this.richResponse.Margin = new System.Windows.Forms.Padding(2);
            this.richResponse.Name = "richResponse";
            this.richResponse.Size = new System.Drawing.Size(511, 275);
            this.richResponse.TabIndex = 3;
            this.richResponse.Text = "";
            // 
            // conTwitchBtn
            // 
            this.conTwitchBtn.Location = new System.Drawing.Point(20, 165);
            this.conTwitchBtn.Name = "conTwitchBtn";
            this.conTwitchBtn.Size = new System.Drawing.Size(112, 43);
            this.conTwitchBtn.TabIndex = 4;
            this.conTwitchBtn.Text = "Connect to Twitch";
            this.conTwitchBtn.UseVisualStyleBackColor = true;
            this.conTwitchBtn.Click += new System.EventHandler(this.conTwitchBtn_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // disTwitchBtn
            // 
            this.disTwitchBtn.Location = new System.Drawing.Point(20, 214);
            this.disTwitchBtn.Name = "disTwitchBtn";
            this.disTwitchBtn.Size = new System.Drawing.Size(112, 43);
            this.disTwitchBtn.TabIndex = 5;
            this.disTwitchBtn.Text = "Disconnect from Twitch";
            this.disTwitchBtn.UseVisualStyleBackColor = true;
            this.disTwitchBtn.Click += new System.EventHandler(this.disTwitchBtn_Click);
            // 
            // ATCBOT_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 297);
            this.Controls.Add(this.disTwitchBtn);
            this.Controls.Add(this.conTwitchBtn);
            this.Controls.Add(this.richResponse);
            this.Controls.Add(this.buttonRequestData);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ATCBOT_View";
            this.Text = "ATC_BOT";
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button buttonConnect;
      private System.Windows.Forms.Button buttonDisconnect;
      private System.Windows.Forms.Button buttonRequestData;
      private System.Windows.Forms.RichTextBox richResponse;
        private System.Windows.Forms.Button conTwitchBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button disTwitchBtn;
    }
}

