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
            this.label3 = new System.Windows.Forms.Label();
            this.linkPortalUrl = new System.Windows.Forms.LinkLabel();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Instrument ID:";
            // 
            // textInstrumentId
            // 
            this.textInstrumentId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textInstrumentId.Location = new System.Drawing.Point(171, 59);
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
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temperature Value:";
            // 
            // textDataValue
            // 
            this.textDataValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textDataValue.Location = new System.Drawing.Point(171, 85);
            this.textDataValue.MaxLength = 3;
            this.textDataValue.Name = "textDataValue";
            this.textDataValue.Size = new System.Drawing.Size(100, 20);
            this.textDataValue.TabIndex = 3;
            this.textDataValue.TextChanged += new System.EventHandler(this.textDataValue_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Portal URL:";
            // 
            // linkPortalUrl
            // 
            this.linkPortalUrl.AutoSize = true;
            this.linkPortalUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkPortalUrl.Location = new System.Drawing.Point(12, 29);
            this.linkPortalUrl.Name = "linkPortalUrl";
            this.linkPortalUrl.Size = new System.Drawing.Size(93, 20);
            this.linkPortalUrl.TabIndex = 5;
            this.linkPortalUrl.TabStop = true;
            this.linkPortalUrl.Text = "linkPortalUrl";
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(16, 111);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(256, 23);
            this.buttonSubmit.TabIndex = 6;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_ClickAsync);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.linkPortalUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textDataValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textInstrumentId);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "CHORDS Data Feeder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textInstrumentId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textDataValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkPortalUrl;
        private System.Windows.Forms.Button buttonSubmit;
    }
}