#region License
/*
LangLib

A program and library for application localization.
Copyright (C) 2019 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of LangLib.

LangLib is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

LangLib is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with LangLib.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Reflection;
using System.IO;

using System.Data.SQLite;
using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

using VPKSoft.LangLib;
using VPKSoft.ConfLib;
using VPKSoft.Utils;

namespace SecureDatabaseSetting
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitFromHostApp();
            Array dbs = Enum.GetValues(typeof(DBLangEngine.DatabaseType));

            cmbDatabaseType.DisplayMember = "Value";

            foreach (var value in dbs)
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = new KeyValuePair<DBLangEngine.DatabaseType, string>((DBLangEngine.DatabaseType)value, GetEnumDescription((DBLangEngine.DatabaseType)value));
                cmbDatabaseType.Items.Add(v);
            }

            cmbDatabaseType.SelectedIndex = 0;


            SetDefaultFile();

            ttTip.SetToolTip(btSaveConfigTo, "This should not be changed.");

            this.Icon = Icon.ExtractAssociatedIcon(parentExecutable);
            LoadSettings();
        }

        private string appName = string.Empty;
        private string parentExecutable = string.Empty;

        private static bool closedAccepted = false;

        private void InitFromHostApp()
        {
            ProgramArgumentCollection coll = new ProgramArgumentCollection();
            if (coll.ArgumentExists("--program"))
            {
                parentExecutable = coll["--program"];
            }
            else
            {
                string wmiQuery = string.Format("SELECT * FROM Win32_Process WHERE ProcessId = {0}", Process.GetCurrentProcess().Id);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", wmiQuery);
                ManagementObjectCollection.ManagementObjectEnumerator mColl = searcher.Get().GetEnumerator();
                mColl.MoveNext();

                parentExecutable = (string)mColl.Current["ExecutablePath"];
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(parentExecutable);

            if (info.ProductName == null || info.ProductName == string.Empty)
            {
                appName = Path.GetFileNameWithoutExtension(parentExecutable);
            }
            else
            {
                appName = info.ProductName;
            }

            Text = "LangLib - Database configuration  [" + appName + "]";
        }

        private void SetDefaultFile()
        {
            foreach (char chr in Path.GetInvalidFileNameChars())
            {
                appName = appName.Replace(chr, '_');
            }
            tbSaveConfigTo.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + appName + @"\dbconfig.sqlite";
        }

        private string GetDefaultFileLang()
        {
            foreach (char chr in Path.GetInvalidFileNameChars())
            {
                appName = appName.Replace(chr, '_');
            }
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + appName + @"\lang.sqlite";
        }


        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        private void cmbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.SelectedItem;
            UpdateFormByDatabase(v.Key);
        }

        private void UpdateFormByDatabase(DBLangEngine.DatabaseType dbType)
        {
            lbServer.Text = "Server:";
            btBrowse.Visible = false;
            tbConnectionString.ReadOnly = true;
            tbServer.Size = new Size(341, 20);
            tbServer.Enabled = true;
            tbDatabase.Enabled = true;
            tbUser.Enabled = true;
            tbPassword.Enabled = true;
            nudPort.Enabled = true;
            tbSchema.Enabled = true;
            cbUsePort.Checked = true;
            cbUsePort.Enabled = false;

            lbServer.Enabled = true;
            lbDatabase.Enabled = true;
            lbUser.Enabled = true;
            lbPassword.Enabled = true;
            lbPort.Enabled = true;
            lbSchema.Enabled = true;
            cbOverrideConnectionString.Checked = false;

            btBrowse.Visible = false;

            ClearFields();

            switch (dbType)
            {
                case DBLangEngine.DatabaseType.dtSQLite:
                    tbDatabase.Enabled = false;
                    tbUser.Enabled = false;
                    tbPassword.Enabled = false;
                    nudPort.Enabled = false;
                    tbSchema.Enabled = false;
                    lbDatabase.Enabled = false;
                    lbUser.Enabled = false;
                    lbPassword.Enabled = false;
                    lbPort.Enabled = false;
                    lbSchema.Enabled = false;
                    lbServer.Text = "File name:";
                    tbServer.Text = GetDefaultFileLang();
                    tbServer.Size = new Size(306, 20);                    
                    btBrowse.Visible = true;
                    break;
                case DBLangEngine.DatabaseType.dtMySQL:
                    lbSchema.Enabled = false;
                    tbSchema.Enabled = false;
                    tbServer.Text = "localhost";
                    tbUser.Text = string.Empty;
                    tbPassword.Text = string.Empty;
                    nudPort.Value = 3306;
                    break;
                case DBLangEngine.DatabaseType.dtMSSQL:
                    cbUsePort.Checked = false;
                    cbUsePort.Enabled = true;
                    tbServer.Text = "localhost";
                    tbSchema.Text = "dbo";
                    nudPort.Value = 1433;
                    break;
                case DBLangEngine.DatabaseType.dtPostgreSQL:
                    tbServer.Text = "localhost";
                    tbSchema.Text = "public";
                    nudPort.Value = 5432;
                    break;
            }
            BuildConnectionString(null, null);
        }

        private void BuildConnectionString(object sender, EventArgs e)
        {
            SetDefaultFile();
            if (!tbConnectionString.ReadOnly)
            {
                return;
            }

            KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.SelectedItem;
            string pwText = new string('•', tbPassword.Text.Length);
            switch (v.Key)
            {
                case DBLangEngine.DatabaseType.dtSQLite:
                    tbConnectionString.Text = "Data Source=" + tbServer.Text + ";Pooling=true;FailIfMissing=false;";
                    break;
                case DBLangEngine.DatabaseType.dtMySQL:
                    tbConnectionString.Text = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8;", tbServer.Text, nudPort.Value, tbDatabase.Text, tbUser.Text, pwText);
                    break;
                case DBLangEngine.DatabaseType.dtPostgreSQL:
                    tbConnectionString.Text = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", tbServer.Text, nudPort.Value, tbDatabase.Text, tbUser.Text, pwText);
                    break;
                case DBLangEngine.DatabaseType.dtMSSQL:
                    tbConnectionString.Text = string.Format("Persist Security Info=False;User ID={3};Password={4};Initial Catalog={2};Server={0};", tbServer.Text + (cbUsePort.Checked ? "," + nudPort.Value : string.Empty), nudPort.Value, tbDatabase.Text, tbUser.Text, pwText);
                    break;
            }
        }

        private void ClearFields()
        {
            tbDatabase.Text = string.Empty;
            tbUser.Text = string.Empty;
            tbPassword.Text = string.Empty;
            nudPort.Value = 0;
            tbSchema.Text = string.Empty;
            tbConnectionString.Text = string.Empty;
        }

        private void cbOverrideConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            tbConnectionString.ReadOnly = !cbOverrideConnectionString.Checked;
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Path.GetDirectoryName(tbServer.Text)))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(tbServer.Text));
                }
                catch
                {
                    return;
                }
            }
            sdSQLite.InitialDirectory = Path.GetDirectoryName(tbServer.Text);
            sdSQLite.FileName = Path.GetFileName(tbServer.Text);
            sdSQLite.OverwritePrompt = false;
            if (sdSQLite.ShowDialog() == DialogResult.OK)
            {
                tbServer.Text = sdSQLite.FileName;
            }
        }

        private void btSaveConfigTo_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Path.GetDirectoryName(tbSaveConfigTo.Text)))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(tbSaveConfigTo.Text));
                }
                catch
                {
                    return;
                }
            }
            sdSQLite.InitialDirectory = Path.GetDirectoryName(tbSaveConfigTo.Text);
            sdSQLite.FileName = Path.GetFileName(tbSaveConfigTo.Text);
            sdSQLite.OverwritePrompt = true;
            if (sdSQLite.ShowDialog() == DialogResult.OK)
            {
                tbSaveConfigTo.Text = sdSQLite.FileName;
            }
        }

        private string ReplacePasswordToReal()
        {
            try
            {
                string sConnStr1 = tbConnectionString.Text;
                sConnStr1 = sConnStr1.Substring(0, sConnStr1.IndexOf('•'));
                string sConnStr2 = tbConnectionString.Text;
                sConnStr2 = sConnStr2.Substring(sConnStr2.LastIndexOf('•') + 1);
                return sConnStr1 + tbPassword.Text + sConnStr2;
            }
            catch
            {
                return tbConnectionString.Text;
            }
        }

        private void LoadSettings()
        {
            Conflib cl = new Conflib();
            cl.AutoCreateSettings = true;
            cl.DataDir = Path.GetDirectoryName(tbSaveConfigTo.Text);
            cl.DBName = Path.GetFileName(tbSaveConfigTo.Text);
            DBLangEngine.DatabaseType dbType = DBLangEngine.DatabaseType.dtSQLite;
            
            try
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = new KeyValuePair<DBLangEngine.DatabaseType, string>((DBLangEngine.DatabaseType)Enum.Parse(typeof(DBLangEngine.DatabaseType), cl["dbType"]), GetEnumDescription((DBLangEngine.DatabaseType)Enum.Parse(typeof(DBLangEngine.DatabaseType), cl["dbType"])));
                for (int i = 0; i < cmbDatabaseType.Items.Count; i++)
                {
                    KeyValuePair<DBLangEngine.DatabaseType, string> item = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.Items[i];
                    
                    if (item.Equals(v))
                    {
                        dbType = v.Key;
                        cmbDatabaseType.SelectedIndex = i;
                        break;
                    }
                }
            } 
            catch
            {
                cmbDatabaseType.SelectedIndex = 0;
                cmbDatabaseType_SelectedIndexChanged(null, null);
            }

            string dbServer = cl["dbServer", dbType == DBLangEngine.DatabaseType.dtSQLite ? GetDefaultFileLang() : "localhost"];
            dbServer = dbServer == null ? dbType == DBLangEngine.DatabaseType.dtSQLite ? GetDefaultFileLang() : "localhost" : dbServer; // down we just like this ? format

            tbServer.Text = dbServer;
            tbDatabase.Text = cl["dbDatabase"];
            tbUser.Text = cl["dbUser"];
            tbPassword.Text = cl["dbPassword"];
            tbSchema.Text = cl["dbSchema"];

            try
            {
                cbNoTables.Checked = bool.Parse(cl["dbNoTables"]);
            }
            catch
            {
                cbNoTables.Checked = false;
            }

            try
            {
                nudPort.Value = decimal.Parse(cl["dbPort"]);
            }
            catch
            {
                nudPort.Value = 0;
            }
            string pwText = new string('•', tbPassword.Text.Length);
            try
            {
                tbConnectionString.Text = cl["dbConnStr"].Replace(cl["dbPassword"], pwText);
            }
            catch
            {
                // Do nothing
            }
            ReplacePasswordToReal();
            try
            {
                cbOverrideConnectionString.Checked = bool.Parse(cl["dbConnStrOverride"]);
            } 
            catch
            {
                cbOverrideConnectionString.Checked = false;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Conflib cl = new Conflib();
            cl.AutoCreateSettings = true;
            cl.DataDir = Path.GetDirectoryName(tbSaveConfigTo.Text);
            cl.DBName = Path.GetFileName(tbSaveConfigTo.Text);
            KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.SelectedItem;

            cl["dbType"] = v.Key.ToString();
            cl["dbServer"] = tbServer.Text;
            cl["dbDatabase"] = tbDatabase.Text;
            cl["dbUser"] = "SECURE:" + tbUser.Text;
            cl["dbPassword"] = "SECURE:" + tbPassword.Text;
            cl["dbSchema"] = tbSchema.Text;
            cl["dbPort"] = nudPort.Value.ToString();
            cl["dbConnStr"] = "SECURE:" + ReplacePasswordToReal();
            cl["dbConnStrOverride"] = cbOverrideConnectionString.Checked.ToString();
            cl["dbNoTables"] = cbNoTables.Checked.ToString();
            closedAccepted = true;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            closedAccepted = false;
            Close();
        }

        private void btTestConnection_Click(object sender, EventArgs e)
        {
            KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.SelectedItem;
            switch (v.Key)
            {
                case DBLangEngine.DatabaseType.dtSQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(ReplacePasswordToReal()))
                        {
                            conn.Open();
                        }
                        MessageBox.Show("Connection succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection error occurred: '" + ex.Message + "'.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case DBLangEngine.DatabaseType.dtPostgreSQL:
                    try
                    {
                        using (NpgsqlConnection conn = new NpgsqlConnection(ReplacePasswordToReal()))
                        {
                            conn.Open();
                        }
                        MessageBox.Show("Connection succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection error occurred: '" + ex.Message + "'.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case DBLangEngine.DatabaseType.dtMySQL:
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(ReplacePasswordToReal()))
                        {
                            conn.Open();
                        }
                        MessageBox.Show("Connection succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection error occurred: '" + ex.Message + "'.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case DBLangEngine.DatabaseType.dtMSSQL:
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(ReplacePasswordToReal()))
                        {
                            conn.Open();
                        }
                        MessageBox.Show("Connection succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection error occurred: '" + ex.Message + "'.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
            btOK.Focus();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (closedAccepted)
            {
                Environment.ExitCode = 0;
            }
            else
            {
                Environment.ExitCode = 1;
            }
        }
    }
}
