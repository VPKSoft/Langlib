namespace DBLocalization
{
    partial class FormLoginDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginDatabase));
            this.cmbDatabaseType = new System.Windows.Forms.ComboBox();
            this.lbServer = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.lbDatabase = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbSchema = new System.Windows.Forms.TextBox();
            this.lbSchema = new System.Windows.Forms.Label();
            this.lbPort = new System.Windows.Forms.Label();
            this.btBrowse = new System.Windows.Forms.Button();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.odSQLite = new System.Windows.Forms.OpenFileDialog();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lbConnectionString = new System.Windows.Forms.Label();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.cbOverrideConnectionString = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbDatabaseType
            // 
            this.cmbDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseType.FormattingEnabled = true;
            this.cmbDatabaseType.Location = new System.Drawing.Point(12, 12);
            this.cmbDatabaseType.Name = "cmbDatabaseType";
            this.cmbDatabaseType.Size = new System.Drawing.Size(436, 21);
            this.cmbDatabaseType.TabIndex = 0;
            this.cmbDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cmbDatabaseType_SelectedIndexChanged);
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Location = new System.Drawing.Point(12, 42);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(41, 13);
            this.lbServer.TabIndex = 1;
            this.lbServer.Text = "Server:";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(107, 39);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(341, 20);
            this.tbServer.TabIndex = 2;
            this.tbServer.TextChanged += new System.EventHandler(this.ShouldUpdateConnectionString);
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(107, 65);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(341, 20);
            this.tbDatabase.TabIndex = 5;
            this.tbDatabase.TextChanged += new System.EventHandler(this.ShouldUpdateConnectionString);
            // 
            // lbDatabase
            // 
            this.lbDatabase.AutoSize = true;
            this.lbDatabase.Location = new System.Drawing.Point(12, 68);
            this.lbDatabase.Name = "lbDatabase";
            this.lbDatabase.Size = new System.Drawing.Size(56, 13);
            this.lbDatabase.TabIndex = 4;
            this.lbDatabase.Text = "Database:";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(107, 91);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(130, 20);
            this.tbUser.TabIndex = 7;
            this.tbUser.TextChanged += new System.EventHandler(this.ShouldUpdateConnectionString);
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Location = new System.Drawing.Point(12, 94);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(32, 13);
            this.lbUser.TabIndex = 6;
            this.lbUser.Text = "User:";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(318, 91);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '•';
            this.tbPassword.Size = new System.Drawing.Size(130, 20);
            this.tbPassword.TabIndex = 9;
            this.tbPassword.TextChanged += new System.EventHandler(this.ShouldUpdateConnectionString);
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(242, 94);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(56, 13);
            this.lbPassword.TabIndex = 8;
            this.lbPassword.Text = "Password:";
            // 
            // tbSchema
            // 
            this.tbSchema.Location = new System.Drawing.Point(107, 117);
            this.tbSchema.Name = "tbSchema";
            this.tbSchema.Size = new System.Drawing.Size(130, 20);
            this.tbSchema.TabIndex = 11;
            this.tbSchema.TextChanged += new System.EventHandler(this.ShouldUpdateConnectionString);
            // 
            // lbSchema
            // 
            this.lbSchema.AutoSize = true;
            this.lbSchema.Location = new System.Drawing.Point(12, 120);
            this.lbSchema.Name = "lbSchema";
            this.lbSchema.Size = new System.Drawing.Size(49, 13);
            this.lbSchema.TabIndex = 10;
            this.lbSchema.Text = "Schema:";
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(242, 120);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(29, 13);
            this.lbPort.TabIndex = 12;
            this.lbPort.Text = "Port:";
            // 
            // btBrowse
            // 
            this.btBrowse.Location = new System.Drawing.Point(419, 39);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(29, 20);
            this.btBrowse.TabIndex = 3;
            this.btBrowse.Text = "...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(318, 118);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(130, 20);
            this.nudPort.TabIndex = 13;
            this.nudPort.ValueChanged += new System.EventHandler(this.ShouldUpdateConnectionString);
            // 
            // odSQLite
            // 
            this.odSQLite.Filter = "SQLite database (*.sqlite*)|*.sqlite*|All files (*.*)|*.*";
            this.odSQLite.Title = "Select database file";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(12, 169);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 17;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(373, 169);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 18;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // lbConnectionString
            // 
            this.lbConnectionString.AutoSize = true;
            this.lbConnectionString.Location = new System.Drawing.Point(12, 146);
            this.lbConnectionString.Name = "lbConnectionString";
            this.lbConnectionString.Size = new System.Drawing.Size(92, 13);
            this.lbConnectionString.TabIndex = 14;
            this.lbConnectionString.Text = "Connection string:";
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Enabled = false;
            this.tbConnectionString.Location = new System.Drawing.Point(107, 143);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(271, 20);
            this.tbConnectionString.TabIndex = 15;
            // 
            // cbOverrideConnectionString
            // 
            this.cbOverrideConnectionString.AutoSize = true;
            this.cbOverrideConnectionString.Location = new System.Drawing.Point(384, 145);
            this.cbOverrideConnectionString.Name = "cbOverrideConnectionString";
            this.cbOverrideConnectionString.Size = new System.Drawing.Size(64, 17);
            this.cbOverrideConnectionString.TabIndex = 16;
            this.cbOverrideConnectionString.Text = "override";
            this.cbOverrideConnectionString.UseVisualStyleBackColor = true;
            this.cbOverrideConnectionString.CheckedChanged += new System.EventHandler(this.cbOverrideConnectionString_CheckedChanged);
            // 
            // FormLoginDatabase
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(457, 204);
            this.Controls.Add(this.lbConnectionString);
            this.Controls.Add(this.tbConnectionString);
            this.Controls.Add(this.cbOverrideConnectionString);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.tbSchema);
            this.Controls.Add(this.lbSchema);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.tbDatabase);
            this.Controls.Add(this.lbDatabase);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.lbServer);
            this.Controls.Add(this.cmbDatabaseType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoginDatabase";
            this.ShowInTaskbar = false;
            this.Text = "Login to a database";
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDatabaseType;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.Label lbDatabase;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.TextBox tbSchema;
        private System.Windows.Forms.Label lbSchema;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.OpenFileDialog odSQLite;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label lbConnectionString;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.CheckBox cbOverrideConnectionString;
    }
}