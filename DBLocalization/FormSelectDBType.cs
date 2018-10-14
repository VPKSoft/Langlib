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

namespace DBLocalization
{
    public partial class FormSelectDBType : Form
    {
        public FormSelectDBType()
        {
            InitializeComponent();
            Array dbs = Enum.GetValues(typeof(DBLangEngine.DatabaseType));

            cmbDatabaseType.DisplayMember = "Value";

            foreach (var value in dbs)
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = new KeyValuePair<DBLangEngine.DatabaseType, string>((DBLangEngine.DatabaseType)value, FormLoginDatabase.GetEnumDescription((DBLangEngine.DatabaseType)value));
                cmbDatabaseType.Items.Add(v);
            }

            cmbDatabaseType.SelectedIndex = 0;
        }

        public static bool Execute(out DBLangEngine.DatabaseType dbType)
        {
            FormSelectDBType frm = new FormSelectDBType();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = (KeyValuePair<DBLangEngine.DatabaseType, string>)frm.cmbDatabaseType.SelectedItem;
                dbType = v.Key;
                return true;
            }
            else
            {
                dbType = DBLangEngine.DatabaseType.dtSQLite;
                return false;
            }
        }
    }
}
