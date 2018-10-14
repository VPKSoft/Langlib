namespace DBLocalization
{
    partial class FormDataDump
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataDump));
            this.tbScript = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbScript
            // 
            this.tbScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbScript.Location = new System.Drawing.Point(12, 12);
            this.tbScript.MaxLength = 1000000;
            this.tbScript.Multiline = true;
            this.tbScript.Name = "tbScript";
            this.tbScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbScript.Size = new System.Drawing.Size(784, 471);
            this.tbScript.TabIndex = 0;
            this.tbScript.WordWrap = false;
            this.tbScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbScript_KeyDown);
            // 
            // FormDataDump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 495);
            this.Controls.Add(this.tbScript);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDataDump";
            this.Text = "Language dump script";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbScript_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbScript;
    }
}