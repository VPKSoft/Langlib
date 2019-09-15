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
using System.Globalization;
using VPKSoft.Utils;

namespace VPKSoft.LangLib
{
    /// <summary>
    /// A class to enumerate Form / Window objects and properties.
    /// </summary>
    public partial class DBLangEngine : GuiObjectsEnum
    {
        private static string Schema
        {
            get
            {
                if (dbType == DatabaseType.dtPostgreSQL)
                {
                    return dbSchema + ".";
                }
                else if (dbType == DatabaseType.dtMSSQL)
                {
                    return "[" + dbSchema + "].";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private static string TableCreateMessages
        {
            get
            {
                if (dbType == DatabaseType.dtSQLite)
                {
                    return
                        "CREATE TABLE IF NOT EXISTS MESSAGES( " +
                        "CULTURE TEXT NOT NULL, " +
                        "MESSAGENAME TEXT NOT NULL, " +
                        "VALUE TEXT NULL, " +
                        "COMMENT_EN_US TEXT NULL, " +
                        "INUSE INTEGER NULL); ";
                }
                else if (dbType == DatabaseType.dtMySQL)
                {
                    return
                        "CREATE TABLE IF NOT EXISTS MESSAGES( " +
                        "CULTURE VARCHAR(100) NOT NULL, " +
                        "MESSAGENAME VARCHAR(255) NOT NULL, " +
                        "VALUE VARCHAR(2000) NULL, " +
                        "COMMENT_EN_US VARCHAR(2000) NULL, " +
                        "INUSE INTEGER NULL); ";
                }
                else if (dbType == DatabaseType.dtPostgreSQL)
                {
                    return
                        "CREATE OR REPLACE FUNCTION CREATE_LANGTABLE () " +
                        "  RETURNS INTEGER AS " +
                        "$$ " +
                        "BEGIN " +
                        "  IF NOT EXISTS ( " +
                        "  SELECT 1 " +
                        "  FROM PG_CATALOG.PG_TABLES  " +
                        "  WHERE " +
                        "  SCHEMANAME = '" + dbSchema + "' AND " +
                        "  TABLENAME  = 'messages') " +
                        "  THEN " +
                        "  CREATE TABLE " + Schema + "MESSAGES( " +
                        "  CULTURE VARCHAR(100) NOT NULL, " +
                        "  MESSAGENAME VARCHAR(255) NOT NULL, " +
                        "  VALUE VARCHAR(2000) NULL, " +
                        "  COMMENT_EN_US VARCHAR(2000) NULL, " +
                        "  INUSE INTEGER NULL); " +
                        "  END IF; " +
                        "  RETURN 1; " +
                        "  END; " +
                        "  $$ LANGUAGE plpgsql; ";
                } 
                else if (dbType == DatabaseType.dtMSSQL)
                {
                    return
                        "IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'MESSAGES' AND " +
                        "SCHEMA_ID = (SELECT SCHEMA_ID FROM SYS.SCHEMAS WHERE NAME = '" + dbSchema + "') AND TYPE = 'U') " +
                        "CREATE TABLE [" + dbSchema + "].[MESSAGES]( " +
                        "CULTURE NVARCHAR(100) NOT NULL, " +
                        "MESSAGENAME NVARCHAR(255) NOT NULL, " +
                        "VALUE NVARCHAR(2000) NULL, " +
                        "COMMENT_EN_US NVARCHAR(2000) NULL, " +
                        "INUSE INTEGER NULL); "; 
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static string TableCreateFormItems
        {
            get
            {
                if (dbType == DatabaseType.dtSQLite)
                {
                    return
                        "CREATE TABLE IF NOT EXISTS FORMITEMS( " +
                        "APP_FORM TEXT NOT NULL, " +
                        "ITEM TEXT NOT NULL, " +
                        "CULTURE TEXT NOT NULL, " +
                        "PROPERTYNAME TEXT NOT NULL, " +
                        "VALUETYPE TEXT NOT NULL, " +
                        "VALUE TEXT NULL, " +
                        "INUSE INTEGER NULL); ";
                }
                else if (dbType == DatabaseType.dtMySQL)
                {
                    return
                        "CREATE TABLE IF NOT EXISTS FORMITEMS( " +
                        "APP_FORM TEXT NOT NULL, " +
                        "ITEM VARCHAR(255) NOT NULL, " +
                        "CULTURE VARCHAR(100) NOT NULL, " +
                        "PROPERTYNAME VARCHAR(255) NOT NULL, " +
                        "VALUETYPE VARCHAR(100) NOT NULL, " +
                        "VALUE VARCHAR(2000) NULL, " +
                        "INUSE INTEGER NULL); ";
                }
                else if (dbType == DatabaseType.dtPostgreSQL)
                {
                    return
                        "CREATE OR REPLACE FUNCTION CREATE_LANGTABLE () " +
                        "  RETURNS INTEGER AS " +
                        "$$ " +
                        "BEGIN " +
                        "  IF NOT EXISTS ( " +
                        "  SELECT 1 " +
                        "  FROM PG_CATALOG.PG_TABLES  " +
                        "  WHERE " +
                        "  SCHEMANAME = '" + dbSchema + "' AND " +
                        "  TABLENAME  = 'formitems') " +
                        "  THEN " +
                        "  CREATE TABLE " + Schema + "FORMITEMS( " +
                        "  APP_FORM TEXT NOT NULL, " +
                        "  ITEM VARCHAR(255) NOT NULL, " +
                        "  CULTURE VARCHAR(100) NOT NULL, " +
                        "  PROPERTYNAME VARCHAR(255) NOT NULL, " +
                        "  VALUETYPE VARCHAR(100) NOT NULL, " +
                        "  VALUE VARCHAR(2000) NULL, " +
                        "  INUSE INTEGER NULL); " +
                        "  END IF; " +
                        "  RETURN 1; " +
                        "  END; " +
                        "  $$ LANGUAGE plpgsql; ";
                }
                else if (dbType == DatabaseType.dtMSSQL)
                {
                    return
                        "IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'FORMITEMS' AND " +
                        "SCHEMA_ID = (SELECT SCHEMA_ID FROM SYS.SCHEMAS WHERE NAME = '" + dbSchema + "') AND TYPE = 'U') " +
                        "CREATE TABLE " + Schema + "[FORMITEMS]( " +
                        "APP_FORM NVARCHAR(MAX) NOT NULL, " +
                        "ITEM NVARCHAR(255) NOT NULL, " +
                        "CULTURE NVARCHAR(100) NOT NULL, " +
                        "PROPERTYNAME NVARCHAR(255) NOT NULL, " +
                        "VALUETYPE NVARCHAR(100) NOT NULL, " +
                        "VALUE NVARCHAR(2000) NULL, " +
                        "INUSE INTEGER NULL); ";
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static string TableCreateCultures
        {
            get
            {
                if (dbType == DatabaseType.dtSQLite)
                {
                    return
                        "CREATE TABLE IF NOT EXISTS CULTURES( " +
                        "CULTURE TEXT NOT NULL, " +
                        "NATIVENAME TEXT NULL, " +
                        "LCID INTEGER NULL); ";
                }
                else if (dbType == DatabaseType.dtMySQL)
                {
                    return
                        "CREATE TABLE IF NOT EXISTS CULTURES( " +
                        "CULTURE VARCHAR(100) NOT NULL, " +
                        "NATIVENAME VARCHAR(200) NULL, " +
                        "LCID INTEGER NULL); ";
                }
                else if (dbType == DatabaseType.dtPostgreSQL)
                {
                    return
                        "CREATE OR REPLACE FUNCTION CREATE_LANGTABLE () " +
                        "  RETURNS INTEGER AS " +
                        "$$ " +
                        "BEGIN " +
                        "  IF NOT EXISTS ( " +
                        "  SELECT 1 " +
                        "  FROM PG_CATALOG.PG_TABLES  " +
                        "  WHERE " +
                        "  SCHEMANAME = '" + dbSchema + "' AND " +
                        "  TABLENAME  = 'cultures') " +
                        "  THEN " +
                        "  CREATE TABLE " + Schema + "CULTURES( " +
                        "  CULTURE VARCHAR(100) NOT NULL, " +
                        "  NATIVENAME VARCHAR(200) NULL, " +
                        "  LCID INTEGER NULL); " +
                        "  END IF; " +
                        "  RETURN 1; " +
                        "  END; " +
                        "  $$ LANGUAGE plpgsql; ";
                }
                else if (dbType == DatabaseType.dtMSSQL)
                {
                    return
                        "IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'CULTURES' AND " +
                        "SCHEMA_ID = (SELECT SCHEMA_ID FROM SYS.SCHEMAS WHERE NAME = '" + dbSchema + "') AND TYPE = 'U') " +
                        "CREATE TABLE " + Schema + "[CULTURES]( " +
                        "CULTURE NVARCHAR(100) NOT NULL, " +
                        "NATIVENAME NVARCHAR(200) NULL, " +
                        "LCID INTEGER NULL); ";
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static string SelectDBCache(string app_form, CultureInfo ci)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return 
                    string.Format("SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 0 AS SORT " +
                    "FROM FORMITEMS " +
                    "WHERE APP_FORM = {0} AND CULTURE = {1} " +
                    "UNION " +
                    "SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 1 AS SORT " +
                    "FROM FORMITEMS " +
                    "WHERE APP_FORM = {0} AND CULTURE = {2} " +
                    "ORDER BY SORT ",
                    DBUtils.MkStr(app_form),
                    DBUtils.MkStr(ci.Name),
                    DBUtils.MkStr(fallBackCulture.Name));
            } 
            else if (dbType == DatabaseType.dtMySQL)
            {
                return
                    string.Format("SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 0 AS SORT " +
                    "FROM FORMITEMS " +
                    "WHERE APP_FORM = {0} AND CULTURE = {1} " +
                    "UNION " +
                    "SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 1 AS SORT " +
                    "FROM FORMITEMS " +
                    "WHERE APP_FORM = {0} AND CULTURE = {2} " +
                    "ORDER BY SORT ",
                    DBUtils.MkStr(app_form),
                    DBUtils.MkStr(ci.Name),
                    DBUtils.MkStr(fallBackCulture.Name));
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 0 AS SORT " +
                    "FROM " + Schema + "FORMITEMS " +
                    "WHERE APP_FORM = {0} AND CULTURE = {1} " +
                    "UNION " +
                    "SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 1 AS SORT " +
                    "FROM " + Schema + "FORMITEMS " +
                    "WHERE APP_FORM = {0} AND CULTURE = {2} " +
                    "ORDER BY SORT ",
                    DBUtils.MkStr(app_form),
                    DBUtils.MkStr(ci.Name),
                    DBUtils.MkStr(fallBackCulture.Name));
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    string.Format("SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 0 AS SORT " +
                    "FROM " + Schema + "FORMITEMS WITH (NOLOCK) " +
                    "WHERE APP_FORM = {0} AND CULTURE = {1} " +
                    "UNION " +
                    "SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 1 AS SORT " +
                    "FROM " + Schema + "FORMITEMS WITH (NOLOCK) " +
                    "WHERE APP_FORM = {0} AND CULTURE = {2} " +
                    "ORDER BY SORT ",
                    DBUtils.MkStr(app_form),
                    DBUtils.MkStr(ci.Name),
                    DBUtils.MkStr(fallBackCulture.Name));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string SelectLocalizedProps(string app_form, CultureInfo ci)
        {
            if (dbType == DatabaseType.dtSQLite || dbType == DatabaseType.dtMySQL || dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 0 AS SORT " +
                                 "FROM " + Schema + "FORMITEMS " +
                                 "WHERE APP_FORM = {0} AND CULTURE = {1} " +
                                 "UNION " +
                                 "SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 1 AS SORT " +
                                 "FROM " + Schema + "FORMITEMS " +
                                 "WHERE APP_FORM = {0} AND CULTURE = {2} " +
                                 "ORDER BY SORT ",
                                 DBUtils.MkStr(app_form),
                                 DBUtils.MkStr(ci.Name),
                                 DBUtils.MkStr(fallBackCulture.Name));
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    string.Format("SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 0 AS SORT " +
                                 "FROM " + Schema + "FORMITEMS WITH (NOLOCK) " +
                                 "WHERE APP_FORM = {0} AND CULTURE = {1} " +
                                 "UNION " +
                                 "SELECT VALUETYPE, VALUE, PROPERTYNAME, ITEM, INUSE, CULTURE, APP_FORM, 1 AS SORT " +
                                 "FROM " + Schema + "FORMITEMS WITH (NOLOCK) " +
                                 "WHERE APP_FORM = {0} AND CULTURE = {2} " +
                                 "ORDER BY SORT ",
                                 DBUtils.MkStr(app_form),
                                 DBUtils.MkStr(ci.Name),
                                 DBUtils.MkStr(fallBackCulture.Name));
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private static string SelectStatMessage(string cultureName, string name)
        {
            if (dbType == DatabaseType.dtSQLite || dbType == DatabaseType.dtMySQL || dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("SELECT VALUE, 0 AS SORT FROM " + Schema + "MESSAGES " +
                                 "WHERE CULTURE = {0} AND MESSAGENAME = {1} " +
                                 "UNION " +
                                 "SELECT VALUE, 1 AS SORT FROM " + Schema + "MESSAGES " +
                                 "WHERE CULTURE = {2} AND MESSAGENAME = {1} " +
                                 "ORDER BY SORT ",
                                 DBUtils.MkStr(cultureName),
                                 DBUtils.MkStr(name),
                                 DBUtils.MkStr(fallBackCulture.Name));
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    string.Format("SELECT VALUE, 0 AS SORT FROM " + Schema + "MESSAGES WITH (NOLOCK) " +
                                 "WHERE CULTURE = {0} AND MESSAGENAME = {1} " +
                                 "UNION " +
                                 "SELECT VALUE, 1 AS SORT FROM " + Schema + "MESSAGES WITH (NOLOCK) " +
                                 "WHERE CULTURE = {2} AND MESSAGENAME = {1} " +
                                 "ORDER BY SORT ",
                                 DBUtils.MkStr(cultureName),
                                 DBUtils.MkStr(name),
                                 DBUtils.MkStr(fallBackCulture.Name));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string InsertLangItem(string app_form, string item, string propertyName, string valueType, string value, CultureInfo ci)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return
                    string.Format("INSERT INTO FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5} " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}); " +
                                  "UPDATE FORMITEMS SET INUSE = 1 WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app_form),
                                  DBUtils.MkStr(item),
                                  DBUtils.MkStr(ci.Name),
                                  DBUtils.MkStr(propertyName),
                                  DBUtils.MkStr(valueType),
                                  DBUtils.MkStr(value));
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                return
                    string.Format("INSERT INTO FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5} FROM DUAL " +
                                  "WHERE NOT EXISTS (SELECT * FROM FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}) LIMIT 1; " +
                                  "UPDATE FORMITEMS SET INUSE = 1 WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app_form),
                                  DBUtils.MkStr(item),
                                  DBUtils.MkStr(ci.Name),
                                  DBUtils.MkStr(propertyName),
                                  DBUtils.MkStr(valueType),
                                  DBUtils.MkStr(value));
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("INSERT INTO " + Schema + "FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5} " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM " + Schema + "FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}); " +
                                  "UPDATE " + Schema + "FORMITEMS SET INUSE = 1 WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app_form),
                                  DBUtils.MkStr(item),
                                  DBUtils.MkStr(ci.Name),
                                  DBUtils.MkStr(propertyName),
                                  DBUtils.MkStr(valueType),
                                  DBUtils.MkStr(value));
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    string.Format("INSERT INTO " + Schema + "FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5} " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM " + Schema + "FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}); " +
                                  "UPDATE " + Schema + "FORMITEMS SET INUSE = 1 WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app_form),
                                  DBUtils.MkStr(item),
                                  DBUtils.MkStr(ci.Name),
                                  DBUtils.MkStr(propertyName),
                                  DBUtils.MkStr(valueType),
                                  DBUtils.MkStr(value));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string InsertMessage(string key, string value, string comment)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return
                    string.Format("INSERT INTO MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US) " +
                                  "SELECT {0}, {1}, {2}, {3} " +
                                  "WHERE NOT EXISTS(SELECT 1 FROM MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}); " +
                                  "UPDATE MESSAGES SET INUSE = 1 WHERE MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(fallBackCulture.Name),
                                  DBUtils.MkStr(key),
                                  DBUtils.MkStr(value),
                                  DBUtils.MkStr(comment));

            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                return
                    string.Format("INSERT INTO MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US) " +
                                  "SELECT {0}, {1}, {2}, {3} FROM DUAL " +
                                  "WHERE NOT EXISTS(SELECT * FROM MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}) LIMIT 1; " +
                                  "UPDATE MESSAGES SET INUSE = 1 WHERE MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(fallBackCulture.Name),
                                  DBUtils.MkStr(key),
                                  DBUtils.MkStr(value),
                                  DBUtils.MkStr(comment));
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("INSERT INTO " + Schema + "MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US) " +
                                  "SELECT {0}, {1}, {2}, {3} " +
                                  "WHERE NOT EXISTS(SELECT 1 FROM " + Schema + "MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}); " +
                                  "UPDATE " + Schema + "MESSAGES SET INUSE = 1 WHERE MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(fallBackCulture.Name),
                                  DBUtils.MkStr(key),
                                  DBUtils.MkStr(value),
                                  DBUtils.MkStr(comment));

            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    string.Format("INSERT INTO " + Schema + "MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US) " +
                                  "SELECT {0}, {1}, {2}, {3} " +
                                  "WHERE NOT EXISTS(SELECT 1 FROM " + Schema + "MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}); " +
                                  "UPDATE " + Schema + "MESSAGES SET INUSE = 1 WHERE MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(fallBackCulture.Name),
                                  DBUtils.MkStr(key),
                                  DBUtils.MkStr(value),
                                  DBUtils.MkStr(comment));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string InsertCulture(CultureInfo ci)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return
                    "INSERT INTO CULTURES (CULTURE, NATIVENAME, LCID) " +
                    "SELECT '" + ci.Name.Replace("'", "''") + "', '" + ci.NativeName.Replace("'", "''") + "', " + ci.LCID + " " +
                    "WHERE NOT EXISTS(SELECT 1 FROM CULTURES WHERE CULTURE = '" + ci.Name.Replace("'", "''") + "'); ";
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                return
                    "INSERT INTO CULTURES " + Environment.NewLine +
                    "(CULTURE, NATIVENAME, LCID) " + Environment.NewLine +
                    "SELECT '" + ci.Name.Replace("'", "''") + "', '" + ci.NativeName.Replace("'", "''") + "', " + ci.LCID + " FROM DUAL " + Environment.NewLine +
                    "WHERE NOT EXISTS (SELECT * FROM CULTURES WHERE CULTURE = '" + ci.Name.Replace("'", "''") + "' AND NATIVENAME = '" + ci.NativeName.Replace("'", "''") + "' AND LCID = " + ci.LCID + ") " + Environment.NewLine +
                    "LIMIT 1; " + Environment.NewLine;
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    "INSERT INTO " + Schema + "CULTURES (CULTURE, NATIVENAME, LCID) " +
                    "SELECT '" + ci.Name.Replace("'", "''") + "', '" + ci.NativeName.Replace("'", "''") + "', " + ci.LCID + " " +
                    "WHERE NOT EXISTS(SELECT 1 FROM " + Schema + "CULTURES WHERE CULTURE = '" + ci.Name.Replace("'", "''") + "'); ";
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    "INSERT INTO " + Schema + "CULTURES (CULTURE, NATIVENAME, LCID) " +
                    "SELECT '" + ci.Name.Replace("'", "''") + "', '" + ci.NativeName.Replace("'", "''") + "', " + ci.LCID + " " +
                    "WHERE NOT EXISTS(SELECT 1 FROM " + Schema + "CULTURES WHERE CULTURE = '" + ci.Name.Replace("'", "''") + "'); ";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string SelectMessage(string cultureName, string name)
        {
            if (dbType == DatabaseType.dtSQLite || dbType == DatabaseType.dtMySQL || dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("SELECT VALUE, 0 AS SORT FROM " + Schema + "MESSAGES " +
                                  "WHERE CULTURE = {0} AND MESSAGENAME = {1} " +
                                  "UNION " +
                                  "SELECT VALUE, 1 AS SORT FROM " + Schema + "MESSAGES " +
                                  "WHERE CULTURE = {2} AND MESSAGENAME = {1} " +
                                  "ORDER BY SORT ",
                                  DBUtils.MkStr(cultureName),
                                  DBUtils.MkStr(name),
                                  DBUtils.MkStr(fallBackCulture.Name));
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return
                    string.Format("SELECT VALUE, 0 AS SORT FROM " + Schema + "MESSAGES WITH (NOLOCK) " +
                                  "WHERE CULTURE = {0} AND MESSAGENAME = {1} " +
                                  "UNION " +
                                  "SELECT VALUE, 1 AS SORT FROM " + Schema + "MESSAGES WITH (NOLOCK) " +
                                  "WHERE CULTURE = {2} AND MESSAGENAME = {1} " +
                                  "ORDER BY SORT ",
                                  DBUtils.MkStr(cultureName),
                                  DBUtils.MkStr(name),
                                  DBUtils.MkStr(fallBackCulture.Name));
            }
            else
            {
                throw new NotImplementedException();                
            }
        }

        private static string PostgrSQLCreateLanguage1()
        {
            if (dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    "CREATE OR REPLACE FUNCTION MAKE_PLPGSQL() " +
                    "RETURNS VOID " +
                    "LANGUAGE SQL " +
                    "AS $$ " +
                    "CREATE LANGUAGE plpgsql; " +
                    "$$; ";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string PostgrSQLCreateLanguage2()
        {
            if (dbType == DatabaseType.dtPostgreSQL)
            {
                return
                    "SELECT " +
                        "CASE " +
                    "    WHEN EXISTS( " +
                    "        SELECT 1 " +
                    "        FROM PG_CATALOG.PG_LANGUAGE " +
                    "        WHERE LANNAME='plpgsql' " +
                    "    ) " +
                    "    THEN NULL " +
                    "    ELSE MAKE_PLPGSQL()  " +
                    "END; " +

                    "DROP FUNCTION MAKE_PLPGSQL();";
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}