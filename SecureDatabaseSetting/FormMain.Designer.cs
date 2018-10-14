namespace SecureDatabaseSetting
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.btBrowse = new System.Windows.Forms.Button();
            this.lbPort = new System.Windows.Forms.Label();
            this.tbSchema = new System.Windows.Forms.TextBox();
            this.lbSchema = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.lbDatabase = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.lbServer = new System.Windows.Forms.Label();
            this.cmbDatabaseType = new System.Windows.Forms.ComboBox();
            this.cbOverrideConnectionString = new System.Windows.Forms.CheckBox();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.lbConnectionString = new System.Windows.Forms.Label();
            this.cbUsePort = new System.Windows.Forms.CheckBox();
            this.lbSaveConfigTo = new System.Windows.Forms.Label();
            this.tbSaveConfigTo = new System.Windows.Forms.TextBox();
            this.btSaveConfigTo = new System.Windows.Forms.Button();
            this.sdSQLite = new System.Windows.Forms.SaveFileDialog();
            this.ttTip = new System.Windows.Forms.ToolTip(this.components);
            this.btTestConnection = new System.Windows.Forms.Button();
            this.cbNoTables = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(378, 223);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 24;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(12, 223);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 22;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(321, 117);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(109, 20);
            this.nudPort.TabIndex = 13;
            this.nudPort.ValueChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // btBrowse
            // 
            this.btBrowse.Location = new System.Drawing.Point(422, 39);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(29, 20);
            this.btBrowse.TabIndex = 3;
            this.btBrowse.Text = "...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(246, 120);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(29, 13);
            this.lbPort.TabIndex = 12;
            this.lbPort.Text = "Port:";
            // 
            // tbSchema
            // 
            this.tbSchema.Location = new System.Drawing.Point(110, 117);
            this.tbSchema.Name = "tbSchema";
            this.tbSchema.Size = new System.Drawing.Size(130, 20);
            this.tbSchema.TabIndex = 11;
            this.tbSchema.TextChanged += new System.EventHandler(this.BuildConnectionString);
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
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(321, 91);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '•';
            this.tbPassword.Size = new System.Drawing.Size(130, 20);
            this.tbPassword.TabIndex = 9;
            this.tbPassword.TextChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(245, 94);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(56, 13);
            this.lbPassword.TabIndex = 8;
            this.lbPassword.Text = "Password:";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(110, 91);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(130, 20);
            this.tbUser.TabIndex = 7;
            this.tbUser.TextChanged += new System.EventHandler(this.BuildConnectionString);
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
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(110, 65);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(341, 20);
            this.tbDatabase.TabIndex = 5;
            this.tbDatabase.TextChanged += new System.EventHandler(this.BuildConnectionString);
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
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(110, 39);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(341, 20);
            this.tbServer.TabIndex = 2;
            this.tbServer.TextChanged += new System.EventHandler(this.BuildConnectionString);
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
            // cmbDatabaseType
            // 
            this.cmbDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseType.FormattingEnabled = true;
            this.cmbDatabaseType.Location = new System.Drawing.Point(12, 12);
            this.cmbDatabaseType.Name = "cmbDatabaseType";
            this.cmbDatabaseType.Size = new System.Drawing.Size(439, 21);
            this.cmbDatabaseType.TabIndex = 0;
            this.cmbDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cmbDatabaseType_SelectedIndexChanged);
            // 
            // cbOverrideConnectionString
            // 
            this.cbOverrideConnectionString.AutoSize = true;
            this.cbOverrideConnectionString.Location = new System.Drawing.Point(387, 145);
            this.cbOverrideConnectionString.Name = "cbOverrideConnectionString";
            this.cbOverrideConnectionString.Size = new System.Drawing.Size(64, 17);
            this.cbOverrideConnectionString.TabIndex = 17;
            this.cbOverrideConnectionString.Text = "override";
            this.cbOverrideConnectionString.UseVisualStyleBackColor = true;
            this.cbOverrideConnectionString.CheckedChanged += new System.EventHandler(this.cbOverrideConnectionString_CheckedChanged);
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(110, 143);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(271, 20);
            this.tbConnectionString.TabIndex = 16;
            // 
            // lbConnectionString
            // 
            this.lbConnectionString.AutoSize = true;
            this.lbConnectionString.Location = new System.Drawing.Point(12, 146);
            this.lbConnectionString.Name = "lbConnectionString";
            this.lbConnectionString.Size = new System.Drawing.Size(92, 13);
            this.lbConnectionString.TabIndex = 15;
            this.lbConnectionString.Text = "Connection string:";
            // 
            // cbUsePort
            // 
            this.cbUsePort.AutoSize = true;
            this.cbUsePort.Location = new System.Drawing.Point(436, 120);
            this.cbUsePort.Name = "cbUsePort";
            this.cbUsePort.Size = new System.Drawing.Size(15, 14);
            this.cbUsePort.TabIndex = 14;
            this.cbUsePort.UseVisualStyleBackColor = true;
            this.cbUsePort.CheckedChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // lbSaveConfigTo
            // 
            this.lbSaveConfigTo.AutoSize = true;
            this.lbSaveConfigTo.Location = new System.Drawing.Point(12, 172);
            this.lbSaveConfigTo.Name = "lbSaveConfigTo";
            this.lbSaveConfigTo.Size = new System.Drawing.Size(79, 13);
            this.lbSaveConfigTo.TabIndex = 18;
            this.lbSaveConfigTo.Text = "Save config to:";
            // 
            // tbSaveConfigTo
            // 
            this.tbSaveConfigTo.Location = new System.Drawing.Point(110, 169);
            this.tbSaveConfigTo.Name = "tbSaveConfigTo";
            this.tbSaveConfigTo.Size = new System.Drawing.Size(306, 20);
            this.tbSaveConfigTo.TabIndex = 19;
            // 
            // btSaveConfigTo
            // 
            this.btSaveConfigTo.Location = new System.Drawing.Point(422, 169);
            this.btSaveConfigTo.Name = "btSaveConfigTo";
            this.btSaveConfigTo.Size = new System.Drawing.Size(29, 20);
            this.btSaveConfigTo.TabIndex = 20;
            this.btSaveConfigTo.Text = "...";
            this.btSaveConfigTo.UseVisualStyleBackColor = true;
            this.btSaveConfigTo.Click += new System.EventHandler(this.btSaveConfigTo_Click);
            // 
            // sdSQLite
            // 
            this.sdSQLite.Filter = "SQLite database (*.sqlite*)|*.sqlite*|All files (*.*)|*.*";
            this.sdSQLite.Title = "Select database file";
            // 
            // btTestConnection
            // 
            this.btTestConnection.Location = new System.Drawing.Point(259, 223);
            this.btTestConnection.Name = "btTestConnection";
            this.btTestConnection.Size = new System.Drawing.Size(113, 23);
            this.btTestConnection.TabIndex = 23;
            this.btTestConnection.Text = "Test connection";
            this.btTestConnection.UseVisualStyleBackColor = true;
            this.btTestConnection.Click += new System.EventHandler(this.btTestConnection_Click);
            // 
            // cbNoTables
            // 
            this.cbNoTables.AutoSize = true;
            this.cbNoTables.Location = new System.Drawing.Point(110, 195);
            this.cbNoTables.Name = "cbNoTables";
            this.cbNoTables.Size = new System.Drawing.Size(316, 17);
            this.cbNoTables.TabIndex = 21;
            this.cbNoTables.Text = "Don\'t try to create tables (you need to create them your self)...";
            this.cbNoTables.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(465, 258);
            this.Controls.Add(this.cbNoTables);
            this.Controls.Add(this.btTestConnection);
            this.Controls.Add(this.btSaveConfigTo);
            this.Controls.Add(this.tbSaveConfigTo);
            this.Controls.Add(this.lbSaveConfigTo);
            this.Controls.Add(this.cbUsePort);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "LangLib - Database configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.TextBox tbSchema;
        private System.Windows.Forms.Label lbSchema;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.Label lbDatabase;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.ComboBox cmbDatabaseType;
        private System.Windows.Forms.CheckBox cbOverrideConnectionString;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label lbConnectionString;
        private System.Windows.Forms.CheckBox cbUsePort;
        private System.Windows.Forms.Label lbSaveConfigTo;
        private System.Windows.Forms.TextBox tbSaveConfigTo;
        private System.Windows.Forms.Button btSaveConfigTo;
        private System.Windows.Forms.SaveFileDialog sdSQLite;
        private System.Windows.Forms.ToolTip ttTip;
        private System.Windows.Forms.Button btTestConnection;
        private System.Windows.Forms.CheckBox cbNoTables;
    }
}

