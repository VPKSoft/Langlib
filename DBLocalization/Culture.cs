﻿#region License
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data.SqlClient;

namespace DBLocalization
{
    public class Culture
    {
        public List<Culture> Cultures = new List<Culture>();

        public string CultureText;
        public string NativeName;
        public int LCID;

        public Culture(string culture, string nativeName, int lcid)
        {
            CultureText = culture;
            NativeName = nativeName;
            LCID = lcid;
        }

        public override string ToString()
        {
            return NativeName + " [" + CultureText + "]";
        }


        public Culture(ref SqlConnection conn)
        {
            string sql = "SELECT CULTURE, NATIVENAME, LCID " +
                         "FROM CULTURES " +
                         "ORDER BY NATIVENAME; ";

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cultures.Add(new Culture(dr.GetString(0), dr.GetString(1), dr.GetInt32(2)));
                    }
                }
            }
        }

        public Culture(ref NpgsqlConnection conn)
        {
            string sql = "SELECT CULTURE, NATIVENAME, LCID " +
                         "FROM CULTURES " +
                         "ORDER BY NATIVENAME; ";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
            {
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cultures.Add(new Culture(dr.GetString(0), dr.GetString(1), dr.GetInt32(2)));
                    }
                }
            }
        }

        public Culture(ref MySqlConnection conn)
        {
            string sql = "SELECT CULTURE, NATIVENAME, LCID " +
                  "FROM CULTURES " +
                  "ORDER BY NATIVENAME; ";

            using (MySqlCommand command = new MySqlCommand(sql, conn))
            {
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cultures.Add(new Culture(dr.GetString(0), dr.GetString(1), dr.GetInt32(2)));
                    }
                }
            }
        }

        public Culture(ref SQLiteConnection conn)
        {
            using (SQLiteCommand command = new SQLiteCommand(conn))
            {
                command.CommandText = "SELECT CULTURE, NATIVENAME, LCID " +
                                      "FROM CULTURES " +
                                      "ORDER BY NATIVENAME COLLATE NOCASE ";
                using (SQLiteDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cultures.Add(new Culture(dr.GetString(0), dr.GetString(1), dr.GetInt32(2)));
                    }
                }
            }
        }
    }
}
