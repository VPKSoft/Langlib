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
using System.Data.SQLite;
using VPKSoft.LangLib;
using System.Globalization;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data.SqlClient;
using VPKSoft.Utils;
using System.IO;

namespace DBLocalization
{
    public partial class MainWindow : Form
    {
        SQLiteConnection connSQLite = null;
        MySqlConnection connMySQL = null;
        NpgsqlConnection connPostgreSQL = null;
        SqlConnection connMSSQL = null;

        public MainWindow()
        {
            InitializeComponent();
            mnuSelectCurrentCulture.Text = "Select current culture (" + CultureInfo.CurrentCulture.ToString() + ")";

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (File.Exists(arg) && Path.GetExtension(arg).ToUpperInvariant() != ".exe".ToUpperInvariant())
                {
                    OpenDatabase(arg);
                }
            }
        }

        private DBLangEngine.DatabaseType dbType = DBLangEngine.DatabaseType.dtSQLite;
        string connStr = string.Empty;
        string captionStr = string.Empty;
        string schema = string.Empty;
        Culture culture;
        private string currentFile = string.Empty;


        private void OpenDatabase(string fileName)
        {
            try
            {
                CloseDBConnection();
                connStr = "Data Source=" + fileName + ";Pooling=true;FailIfMissing=false";
                connSQLite = new SQLiteConnection(connStr);
                connSQLite.Open();
                culture = new Culture(ref connSQLite);
                currentFile = fileName;
                captionStr = fileName;
                dbType = DBLangEngine.DatabaseType.dtSQLite;
                schema = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to database ('" + ex.Message + "').", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SQLs.SetSchema(DBLangEngine.DatabaseType.dtSQLite, schema);

            Text = "DBLangVersion [" + captionStr + "]";
            mnuSave.Enabled = true;
            mnuAddFromCulture.Enabled = true;
            mnuRemoveUnused.Enabled = true;
            cmbCulture.Enabled = true;

            mnuExportDatabase.Enabled = dbType == DBLangEngine.DatabaseType.dtSQLite;


            mnuAddFomEN_US.Enabled = false;
            mnuSelectCurrentCulture.Enabled = true;
            mnuDumpData.Enabled = true;

            cmbCulture.Items.AddRange(culture.Cultures.ToArray());
            for (int i = 0; i < cmbCulture.Items.Count; i++)
            {
                if ((cmbCulture.Items[i] as Culture).LCID == 1033)
                {
                    cmbCulture.SelectedIndex = i;
                }
                cmbCulture.AutoCompleteCustomSource.Add((cmbCulture.Items[i] as Culture).NativeName);
            }

            mnuSelectSomeCulture.Enabled = true;
            ListCulturesMenu();
        }

        private void mnuLoadDB_Click(object sender, EventArgs e)
        {
            if (FormLoginDatabase.Execute(out dbType, out connStr, out captionStr, out schema))
            {
                CloseDBConnection();
                try
                {
                    if (dbType == DBLangEngine.DatabaseType.dtSQLite)
                    {
                        connSQLite = new SQLiteConnection(connStr);
                    }
                    else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
                    {
                        connMySQL = new MySqlConnection(connStr);
                    }
                    else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
                    {
                        connPostgreSQL = new NpgsqlConnection(connStr);
                    }
                    else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
                    {
                        connMSSQL = new SqlConnection(connStr);
                    }

                    if (dbType == DBLangEngine.DatabaseType.dtSQLite)
                    {
                        connSQLite.Open();
                        culture = new Culture(ref connSQLite);
                    }
                    else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
                    {
                        connMySQL.Open();
                        culture = new Culture(ref connMySQL);
                    }
                    else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
                    {
                        connPostgreSQL.Open();
                        culture = new Culture(ref connPostgreSQL);
                    }
                    else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
                    {
                        connMSSQL.Open();
                        culture = new Culture(ref connMSSQL);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to connect to database ('" + ex.Message + "').", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SQLs.SetSchema(dbType, schema);

                Text = "DBLangVersion [" + captionStr + "]";
                mnuSave.Enabled = true;
                mnuAddFromCulture.Enabled = true;
                mnuRemoveUnused.Enabled = true;
                cmbCulture.Enabled = true;


                mnuAddFomEN_US.Enabled = false;
                mnuSelectCurrentCulture.Enabled = true;
                mnuDumpData.Enabled = true;

                mnuExportDatabase.Enabled = dbType == DBLangEngine.DatabaseType.dtSQLite;

                cmbCulture.Items.AddRange(culture.Cultures.ToArray());
                for (int i = 0; i < cmbCulture.Items.Count; i++)
                {
                    if ((cmbCulture.Items[i] as Culture).LCID == 1033)
                    {
                        cmbCulture.SelectedIndex = i;
                    }
                    cmbCulture.AutoCompleteCustomSource.Add((cmbCulture.Items[i] as Culture).NativeName);
                }

                mnuSelectSomeCulture.Enabled = true;
                ListCulturesMenu();
            }
        }

        private IDbCommand GetCommand()
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return new SQLiteCommand(connSQLite);
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return new MySqlCommand(string.Empty, connMySQL);
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return new NpgsqlCommand(string.Empty, connPostgreSQL);
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return new SqlCommand(string.Empty, connMSSQL);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private IDbCommand GetCommand(IDbTransaction trans)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return new SQLiteCommand(string.Empty, connSQLite, (SQLiteTransaction)trans);
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return new MySqlCommand(string.Empty, connMySQL, (MySqlTransaction)trans);
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return new NpgsqlCommand(string.Empty, connPostgreSQL, (NpgsqlTransaction)trans);
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return new SqlCommand(string.Empty, connMSSQL, (SqlTransaction)trans);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private IDbTransaction GetTrans()
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return connSQLite.BeginTransaction();
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return connMySQL.BeginTransaction();
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return connPostgreSQL.BeginTransaction();
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return connMSSQL.BeginTransaction();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void ListCulturesMenu()
        {
            mnuSelectSomeCulture.DropDownItems.Clear();

            using (IDbCommand command = GetCommand())
            {
                command.CommandText = SQLs.SelectDistinctCulture(dbType);

                
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(dr.GetString(0)) { Tag = CultureInfo.GetCultureInfo(dr.GetString(0)), Checked = (cmbCulture.SelectedItem as Culture).CultureText == dr.GetString(0) };
                        item.Click += SelectSomeCultureClick;
                        mnuSelectSomeCulture.DropDownItems.Add(item);
                    }
                }
            }
        }

        private void SelectSomeCultureClick(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem).Tag as CultureInfo).Name != (cmbCulture.SelectedItem as Culture).CultureText)
            {
                for (int i = 0; i < cmbCulture.Items.Count; i++)
                {
                    if ((cmbCulture.Items[i] as Culture).LCID == ((sender as ToolStripMenuItem).Tag as CultureInfo).LCID)
                    {
                        cmbCulture.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private bool CurrentCultureLoaded()
        {
            return (cmbCulture.SelectedItem as Culture).LCID == CultureInfo.CurrentCulture.LCID;
        }

        private bool EnUSCultureLoaded()
        {
            return (cmbCulture.SelectedItem as Culture).LCID == 1033;
        }

        private void LoadDB(string culture = "")
        {
            gvFormItems.Rows.Clear();
            gvMessages.Rows.Clear();
            if (culture == string.Empty)
            {
                culture = (cmbCulture.Items[cmbCulture.SelectedIndex] as Culture).CultureText;
            }

            string culture2 = (cmbCulture.Items[cmbCulture.SelectedIndex] as Culture).CultureText;

            using (IDbCommand command = GetCommand())
            {
                command.CommandText = SQLs.SelectUsedCultures(dbType, culture);
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string split1, split2;
                        try
                        {
                            string[] split = dr.GetString(0).Split('.');
                            split1 = split[0];
                            split2 = split[1];
                        }
                        catch
                        {
                            split1 = dr.GetString(0);
                            split2 = string.Empty;
                        }
                        gvFormItems.Rows.Add(split1,
                                             split2,
                                             dr.GetString(1),
                                             culture2,
                                             dr.GetString(3),
                                             dr.GetString(5),
                                             dr.GetString(4),
                                             dr.GetInt32(6) == 1);
                    }
                }
            }

            using (IDbCommand command = GetCommand())
            {
                command.CommandText = SQLs.SelectUsedMessages(dbType, culture);

                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        gvMessages.Rows.Add(dr.GetString(0),
                                            dr.GetString(1),
                                            dr.GetString(2),
                                            culture2,
                                            dr.GetInt32(4) == 1);
                    }
                }
            }
            mnuAddFomEN_US.Enabled = !EnUSCultureLoaded();
            mnuSelectCurrentCulture.Enabled = !CurrentCultureLoaded();
        }

        private void cbCulture_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDB();
            ListCulturesMenu();
        }

        private void SaveIfNotExist()
        {
            string sql = string.Empty;
            for (int i = 0; i < gvFormItems.Rows.Count; i++)
            {
                sql += SQLs.InsertFormItems(dbType,  
                                            gvFormItems.Rows[i].Cells[colApp.Index].Value,
                                            gvFormItems.Rows[i].Cells[colForm.Index].Value,
                                            gvFormItems.Rows[i].Cells[colItem.Index].Value,
                                            gvFormItems.Rows[i].Cells[colCulture.Index].Value,
                                            gvFormItems.Rows[i].Cells[colPropertyName.Index].Value,
                                            gvFormItems.Rows[i].Cells[colValueType.Index].Value,
                                            gvFormItems.Rows[i].Cells[colValue.Index].Value,
                                            gvFormItems.Rows[i].Cells[colInUse.Index].Value);
            }
            using (IDbTransaction trans = GetTrans())
            {
                using (IDbCommand command = GetCommand(trans))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }

            sql = string.Empty;

            for (int i = 0; i < gvMessages.Rows.Count; i++)
            {
                sql += SQLs.InsertMessages(dbType,
                                           gvMessages.Rows[i].Cells[colMessageCulture.Index].Value,
                                           gvMessages.Rows[i].Cells[colMessageName.Index].Value,
                                           gvMessages.Rows[i].Cells[colMessageValue.Index].Value,
                                           gvMessages.Rows[i].Cells[colCommentEnUs.Index].Value,
                                           gvMessages.Rows[i].Cells[colMsgInUse.Index].Value);                
            }

            using (IDbTransaction trans = GetTrans())
            {
                using (IDbCommand command = GetCommand(trans))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }
        }

        private void Save()
        {
            string sql = string.Empty;
            for (int i = 0; i < gvMessages.Rows.Count; i++)
            {
                sql += SQLs.UpdateMessage(dbType,
                                          gvMessages.Rows[i].Cells[colMessageValue.Index].Value,
                                          gvMessages.Rows[i].Cells[colMessageCulture.Index].Value,
                                          gvMessages.Rows[i].Cells[colMessageName.Index].Value);

            }

            using (IDbTransaction trans = GetTrans())
            {
                using (IDbCommand command = GetCommand(trans))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }

            sql = string.Empty;
            for (int i = 0; i < gvFormItems.Rows.Count; i++)
            {
                sql += SQLs.UpdateFormItems(dbType, 
                                            gvFormItems.Rows[i].Cells[colValue.Index].Value,
                                            gvFormItems.Rows[i].Cells[colCulture.Index].Value,
                                            gvFormItems.Rows[i].Cells[colApp.Index].Value, 
                                            gvFormItems.Rows[i].Cells[colForm.Index].Value,
                                            gvFormItems.Rows[i].Cells[colPropertyName.Index].Value,
                                            gvFormItems.Rows[i].Cells[colItem.Index].Value);
            }
            using (IDbTransaction trans = GetTrans())
            {
                using (IDbCommand command = GetCommand(trans))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void CloseDBConnection()
        {
            if (connMySQL != null)
            {
                connMySQL.Dispose();
                connMySQL = null;
            }

            if (connSQLite != null)
            {
                connSQLite.Dispose();
                connSQLite = null;
            }

            if (connPostgreSQL != null)
            {
                connPostgreSQL.Dispose();
                connPostgreSQL = null;
            }

            if (connMSSQL != null)
            {
                connMSSQL.Dispose();
                connMSSQL = null;
            }
            
            Text = "DBLocalization";
            mnuSave.Enabled = false;
            cmbCulture.Enabled = false;
            mnuAddFomEN_US.Enabled = false;
            mnuDumpData.Enabled = false;
            mnuAddFromCulture.Enabled = false;
            mnuSelectCurrentCulture.Enabled = false;
            mnuRemoveUnused.Enabled = false;
            mnuSelectSomeCulture.Enabled = false;
            cmbCulture.Enabled = false;
            gvFormItems.RowCount = 0;
            gvMessages.RowCount = 0;
            cmbCulture.Items.Clear();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseDBConnection();
        }

        private void mnuAddFomEN_US_Click(object sender, EventArgs e)
        {
            LoadDB("en-US");
            SaveIfNotExist();
            LoadDB();
        }

        private void mnuRemoveUnused_Click(object sender, EventArgs e)
        {
            string culture = (cmbCulture.Items[cmbCulture.SelectedIndex] as Culture).CultureText;
            string sql = SQLs.DeleteUnusedMessages(dbType, culture);
            sql += SQLs.DeleteUnusedFormItems(dbType, culture);
            using (IDbTransaction trans = GetTrans())
            {
                using (IDbCommand command = GetCommand(trans))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }
            LoadDB();
        }

        private void gvFormItems_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void gvFormItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string eValue = gvFormItems.Rows[e.RowIndex].Cells[colValue.Index].Value.ToString();
            if (FormEditCell.Execute(ref eValue))
            {
                gvFormItems.Rows[e.RowIndex].Cells[colValue.Index].Value = eValue;
            }
        }

        private void mnuSelectCurrentCulture_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cmbCulture.Items.Count; i++)
            {
                if ((cmbCulture.Items[i] as Culture).LCID == CultureInfo.CurrentCulture.LCID)
                {
                    cmbCulture.SelectedIndex = i;
                }
            }
        }

        private void mnuAddFromCulture_Click(object sender, EventArgs e)
        {
            Culture selected;
            if (AddFromCulture.Execute(culture.Cultures, (Culture)cmbCulture.SelectedItem, out selected))
            {
                LoadDB(selected.CultureText);
                SaveIfNotExist();
                LoadDB();
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            new FormAbout().ShowDialog();
        }

        private void DoMessageDump()
        {
            DBLangEngine.DatabaseType scriptDbType;
            if (FormSelectDBType.Execute(out scriptDbType))
            {
                string scriptSchema = string.Empty;
                if (scriptDbType == DBLangEngine.DatabaseType.dtMSSQL)
                {
                    scriptSchema = "[dbo].";
                }
                else if (scriptDbType == DBLangEngine.DatabaseType.dtPostgreSQL)
                {
                    scriptSchema = "public.";
                }

                List<string> sqlDump = new List<string>();

                string addStrForm = "INSERT INTO FORMITEMS(APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE) " + Environment.NewLine +
                                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} " + Environment.NewLine +
                                    "WHERE NOT EXISTS (SELECT 1 FROM {7}FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND CULTURE = {2} AND PROPERTYNAME = {3}); " + Environment.NewLine +
                                    "UPDATE {7}FORMITEMS SET INUSE = {6}, VALUE = {5} WHERE APP_FORM = {0} AND ITEM = {1} AND PROPERTYNAME = {3} AND CULTURE = {2}; " + Environment.NewLine;
                string addStrMessage = "INSERT INTO {5}MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE) " +
                                       "SELECT {0}, {1}, {2}, {3}, {4} " + Environment.NewLine +
                                       "WHERE NOT EXISTS (SELECT 1 FROM {5}MESSAGES WHERE MESSAGENAME = {1} AND CULTURE = {0}); " + Environment.NewLine +
                                       "UPDATE {5}MESSAGES SET VALUE = {2}, COMMENT_EN_US = {3}, INUSE = {4} WHERE MESSAGENAME = {1} AND CULTURE = {0}; " + Environment.NewLine;

                if (scriptDbType == DBLangEngine.DatabaseType.dtMySQL)
                {
                    addStrForm = "INSERT INTO FORMITEMS(APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE) " + Environment.NewLine +
                                 "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} FROM DUAL " + Environment.NewLine +
                                 "WHERE NOT EXISTS (SELECT * FROM {7}FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND CULTURE = {2} AND PROPERTYNAME = {3}) LIMIT 1; " + Environment.NewLine +
                                 "UPDATE {7}FORMITEMS SET INUSE = {6}, VALUE = {5} WHERE APP_FORM = {0} AND ITEM = {1} AND PROPERTYNAME = {3} AND CULTURE = {2}; " + Environment.NewLine;

                    addStrMessage = "INSERT INTO {5}MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE) " +
                                    "SELECT {0}, {1}, {2}, {3}, {4} FROM DUAL " + Environment.NewLine +
                                    "WHERE NOT EXISTS (SELECT * FROM {5}MESSAGES WHERE MESSAGENAME = {1} AND CULTURE = {0}) LIMIT 1; " + Environment.NewLine +
                                    "UPDATE {5}MESSAGES SET VALUE = {2}, COMMENT_EN_US = {3}, INUSE = {4} WHERE MESSAGENAME = {1} AND CULTURE = {0}; " + Environment.NewLine;
                }

                using (IDbCommand command = GetCommand())
                {
                    command.CommandText = SQLs.SelectDumpFormItems(dbType);
                    using (IDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            sqlDump.Add(string.Format(addStrForm,
                                        DBUtils.MkStr(dr.GetString(0)),
                                        DBUtils.MkStr(dr.GetString(1)),
                                        DBUtils.MkStr(dr.GetString(2)),
                                        DBUtils.MkStr(dr.GetString(3)),
                                        DBUtils.MkStr(dr.GetString(4)),
                                        DBUtils.MkStr(dr.GetString(5)),
                                        dr.GetInt32(6),
                                        scriptSchema));
                        }
                    }
                }

                using (IDbCommand command = GetCommand())
                {
                    command.CommandText = SQLs.SelectDumpMessages(dbType);
                    using (IDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            sqlDump.Add(string.Format(addStrMessage,
                                        DBUtils.MkStr(dr.GetString(0)),
                                        DBUtils.MkStr(dr.GetString(1)),
                                        DBUtils.MkStr(dr.GetString(2)),
                                        DBUtils.MkStr(dr.GetString(3)),
                                        dr.GetInt32(4),
                                        scriptSchema));
                        }
                    }
                }
                FormDataDump.Execute(sqlDump);
            }
        }

        private void mnuConnectToDatabase_Click(object sender, EventArgs e)
        {
            DoMessageDump();
        }

        private void mnuExportDatabase_Click(object sender, EventArgs e)
        {
            sdSQLite.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            if (sdSQLite.ShowDialog() == DialogResult.OK)
            {
                Save();
                CloseDBConnection();
                File.Copy(currentFile, sdSQLite.FileName, true);
                OpenDatabase(currentFile);
            }
        }
    }
}
