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

namespace DBLocalization
{
    public partial class FormDataDump : Form
    {
        public FormDataDump()
        {
            InitializeComponent();
        }

        public static void Execute(List<string> lines)
        {
            FormDataDump frm = new FormDataDump();
            frm.tbScript.SuspendLayout();
            foreach (string line in lines)
            {
                frm.tbScript.Text += line;
            }
            frm.tbScript.ResumeLayout();
            frm.Show();
        }

        public static void Execute(string[] lines)
        {
            List<string> lineList = new List<string>();
            lineList.AddRange(lines);
            Execute(lineList);
        }

        private void tbScript_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                tbScript.SelectAll();
                e.Handled = true;
            }
        }
    }
}
