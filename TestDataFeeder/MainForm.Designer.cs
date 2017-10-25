namespace TestDataFeeder
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textInstrumentId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textDataValue = new System.Windows.Forms.TextBox();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.linkPortalUrl = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textLog = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-4, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Instrument ID:";
            // 
            // textInstrumentId
            // 
            this.textInstrumentId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textInstrumentId.Location = new System.Drawing.Point(157, 40);
            this.textInstrumentId.MaxLength = 3;
            this.textInstrumentId.Name = "textInstrumentId";
            this.textInstrumentId.Size = new System.Drawing.Size(100, 20);
            this.textInstrumentId.TabIndex = 1;
            this.textInstrumentId.TextChanged += new System.EventHandler(this.textInstrumentId_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-4, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temperature Value:";
            // 
            // textDataValue
            // 
            this.textDataValue.AcceptsReturn = true;
            this.textDataValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textDataValue.Location = new System.Drawing.Point(157, 66);
            this.textDataValue.MaxLength = 3;
            this.textDataValue.Name = "textDataValue";
            this.textDataValue.Size = new System.Drawing.Size(100, 20);
            this.textDataValue.TabIndex = 3;
            this.textDataValue.TextChanged += new System.EventHandler(this.textDataValue_TextChanged);
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(3, 92);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(254, 36);
            this.buttonSubmit.TabIndex = 6;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_ClickAsync);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.buttonSubmit);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.linkPortalUrl);
            this.panel1.Controls.Add(this.textInstrumentId);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textDataValue);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 240);
            this.panel1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-4, -3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Portal URL:";
            // 
            // linkPortalUrl
            // 
            this.linkPortalUrl.AutoSize = true;
            this.linkPortalUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkPortalUrl.Location = new System.Drawing.Point(0, 17);
            this.linkPortalUrl.Name = "linkPortalUrl";
            this.linkPortalUrl.Size = new System.Drawing.Size(63, 13);
            this.linkPortalUrl.TabIndex = 5;
            this.linkPortalUrl.TabStop = true;
            this.linkPortalUrl.Text = "linkPortalUrl";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textLog);
            this.panel2.Location = new System.Drawing.Point(279, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(293, 243);
            this.panel2.TabIndex = 8;
            // 
            // textLog
            // 
            this.textLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textLog.Location = new System.Drawing.Point(0, 0);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(293, 240);
            this.textLog.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "CHORDS Data Feeder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textInstrumentId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textDataValue;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkPortalUrl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textLog;
    }
}