#region License
/*
LangLib

A program and library for application localization.
Copyright (C) 2015 VPKSoft, Petteri Kautonen

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
using VPKSoft.LangLib;
using System.Reflection;

namespace DBLocalization
{ 
    public partial class FormLoginDatabase : Form
    {
        public static bool Execute(out DBLangEngine.DatabaseType dbType, out string connStr, out string captionStr, out string schema)
        {
            FormLoginDatabase frm = new FormLoginDatabase();
            schema = string.Empty;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)frm.cmbDatabaseType.SelectedItem;
                dbType = v.Key;
                switch (v.Key)
                {
                    case DBLangEngine.DatabaseType.dtSQLite:
                        connStr = frm.cbOverrideConnectionString.Checked ? frm.tbConnectionString.Text : "Data Source=" + frm.tbServer.Text + ";Pooling=true;FailIfMissing=false";
                        captionStr = frm.tbServer.Text;
                        break;
                    case DBLangEngine.DatabaseType.dtMySQL:
                        connStr = frm.cbOverrideConnectionString.Checked ? frm.tbConnectionString.Text : string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8;", frm.tbServer.Text, frm.nudPort.Value, frm.tbDatabase.Text, frm.tbUser.Text, frm.tbPassword.Text);
                        captionStr = frm.tbServer.Text + ":" + frm.tbDatabase.Text;
                        break;
                    case DBLangEngine.DatabaseType.dtPostgreSQL:
                        connStr = frm.cbOverrideConnectionString.Checked ? frm.tbConnectionString.Text : string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", frm.tbServer.Text, frm.nudPort.Value, frm.tbDatabase.Text, frm.tbUser.Text, frm.tbPassword.Text);
                        captionStr = frm.tbServer.Text + ":" + frm.tbDatabase.Text;
                        schema = frm.tbSchema.Text;
                        break;
                    case DBLangEngine.DatabaseType.dtMSSQL:
                        connStr = frm.cbOverrideConnectionString.Checked ? frm.tbConnectionString.Text : string.Format("Persist Security Info=False;User ID={3};Password={4};Initial Catalog={2};Server={0};", frm.tbServer.Text, frm.nudPort.Value, frm.tbDatabase.Text, frm.tbUser.Text, frm.tbPassword.Text);
                        captionStr = frm.tbServer.Text + ":" + frm.tbDatabase.Text;
                        schema = frm.tbSchema.Text;
                        break;
                    default:
                        connStr = string.Empty;
                        captionStr = "DBLangVersion";
                        break;
                }
                return true;
            }
            connStr = string.Empty;
            dbType = DBLangEngine.DatabaseType.dtSQLite;
            captionStr = "DBLangVersion";
            return false;
        }

        public FormLoginDatabase()
        {
            InitializeComponent();
            Array dbs = Enum.GetValues(typeof(DBLangEngine.DatabaseType));

            cmbDatabaseType.DisplayMember = "Value";

            foreach (var value in dbs)
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = new KeyValuePair<DBLangEngine.DatabaseType, string>((DBLangEngine.DatabaseType)value, GetEnumDescription((DBLangEngine.DatabaseType)value));
                cmbDatabaseType.Items.Add(v);
            }

            cmbDatabaseType.SelectedIndex = 0;
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

        private void ClearFields()
        {
            tbDatabase.Text = string.Empty;
            tbUser.Text = string.Empty;
            tbPassword.Text = string.Empty;
            nudPort.Value = 0;
            tbSchema.Text = string.Empty;
        }

        private void UpdateFormByDatabase(DBLangEngine.DatabaseType dbType)
        {
            cbOverrideConnectionString.Checked = false;
            lbServer.Text = "Server:";
            btBrowse.Visible = false;
            tbServer.Size = new Size(341, 20);
            tbServer.Enabled = true;
            tbDatabase.Enabled = true;
            tbUser.Enabled = true;
            tbPassword.Enabled = true;
            nudPort.Enabled = true;
            tbSchema.Enabled = true;

            lbServer.Enabled = true;
            lbDatabase.Enabled = true;
            lbUser.Enabled = true;
            lbPassword.Enabled = true;
            lbPort.Enabled = true;
            lbSchema.Enabled = true;

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
                    tbServer.Size = new Size(306, 20);
                    tbServer.Text = "lang.sqlite";
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
        }

        private void UpdateConnectionString()
        {
            KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.SelectedItem;
            DBLangEngine.DatabaseType dbType = v.Key;
            string connStr;
            switch (v.Key)
            {
                case DBLangEngine.DatabaseType.dtSQLite:
                    connStr = cbOverrideConnectionString.Checked ? tbConnectionString.Text : "Data Source=" + tbServer.Text + ";Pooling=true;FailIfMissing=false";
                    break;
                case DBLangEngine.DatabaseType.dtMySQL:
                    connStr = cbOverrideConnectionString.Checked ? tbConnectionString.Text : string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8;", tbServer.Text, nudPort.Value, tbDatabase.Text, tbUser.Text, tbPassword.Text);
                    break;
                case DBLangEngine.DatabaseType.dtPostgreSQL:
                    connStr = cbOverrideConnectionString.Checked ? tbConnectionString.Text : string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", tbServer.Text, nudPort.Value, tbDatabase.Text, tbUser.Text, tbPassword.Text);
                    break;
                case DBLangEngine.DatabaseType.dtMSSQL:
                    connStr = cbOverrideConnectionString.Checked ? tbConnectionString.Text : string.Format("Persist Security Info=False;User ID={3};Password={4};Initial Catalog={2};Server={0};", tbServer.Text, nudPort.Value, tbDatabase.Text, tbUser.Text, tbPassword.Text);
                    break;
                default:
                    connStr = string.Empty;
                    break;
            }
            if (!cbOverrideConnectionString.Checked)
            {
                tbConnectionString.Text = connStr;
            }
        }

        private void cmbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)cmbDatabaseType.SelectedItem;
            UpdateFormByDatabase(v.Key);
            UpdateConnectionString();
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (odSQLite.ShowDialog() == DialogResult.OK)
            {
                tbServer.Text = odSQLite.FileName;
            }
        }

        private void cbOverrideConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            tbConnectionString.Enabled = cbOverrideConnectionString.Checked;
            UpdateConnectionString();
        }

        private void ShouldUpdateConnectionString(object sender, EventArgs e)
        {
            UpdateConnectionString();
        }
    }
}
