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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPKSoft.LangLib;
using VPKSoft.Utils;

namespace DBLocalization
{
    public static class SQLs
    {
        static string schema = string.Empty;

        public static void SetSchema(DBLangEngine.DatabaseType dbType, string schemaName)
        {
            if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                schema = schemaName == string.Empty ? string.Empty : "[" + schemaName + "].";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                schema = schemaName == string.Empty ? string.Empty : schemaName + ".";
            }
            else
            {
                schema = string.Empty;
            }
        }


        public static string InsertFormItems(DBLangEngine.DatabaseType dbType,
                                             object app, object form, object item, object culture,
                                             object propertyName, object valueType, object value,
                                             object inUse)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return
                    string.Format("INSERT INTO FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}); " +
                                  "UPDATE FORMITEMS SET INUSE = {6} WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(item.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(valueType.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    string.Format("INSERT INTO FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} FROM DUAL " +
                                  "WHERE NOT EXISTS (SELECT * FROM FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}) LIMIT 1; " +
                                  "UPDATE FORMITEMS SET INUSE = {6} WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(item.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(valueType.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));

            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("INSERT INTO " + schema + "FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}); " +
                                  "UPDATE " + schema + "FORMITEMS SET INUSE = {6} WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(item.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(valueType.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    string.Format("INSERT INTO " + schema + "FORMITEMS (APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM FORMITEMS WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}); " +
                                  "UPDATE " + schema + "FORMITEMS SET INUSE = {6} WHERE APP_FORM = {0} AND ITEM = {1} AND " +
                                  "CULTURE = {2} AND PROPERTYNAME = {3}; ",
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(item.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(valueType.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string InsertMessages(DBLangEngine.DatabaseType dbType, 
                                            object culture, object messageName, object value,
                                            object commentEN_US, object inUse)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return 
                    string.Format("INSERT INTO MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4} " +
                                  "WHERE NOT EXISTS(SELECT 1 FROM MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}); " +
                                  "UPDATE MESSAGES SET INUSE = {4} WHERE CULTURE = {0} AND MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(messageName.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(commentEN_US.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));    
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    string.Format("INSERT INTO MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4} FROM DUAL " +
                                  "WHERE NOT EXISTS(SELECT * FROM MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}) LIMIT 1; " +
                                  "UPDATE MESSAGES SET INUSE = {4} WHERE CULTURE = {0} AND MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(messageName.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(commentEN_US.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("INSERT INTO " + schema + "MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4} " +
                                  "WHERE NOT EXISTS(SELECT 1 FROM " + schema + "MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}); " +
                                  "UPDATE " + schema + "MESSAGES SET INUSE = {4} WHERE CULTURE = {0} AND MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(messageName.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(commentEN_US.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    string.Format("INSERT INTO " + schema + "MESSAGES(CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE) " +
                                  "SELECT {0}, {1}, {2}, {3}, {4} " +
                                  "WHERE NOT EXISTS(SELECT 1 FROM " + schema + "MESSAGES WHERE CULTURE = {0} AND MESSAGENAME = {1}); " +
                                  "UPDATE " + schema + "MESSAGES SET INUSE = {4} WHERE CULTURE = {0} AND MESSAGENAME = {1}; ",
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(messageName.ToString()),
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(commentEN_US.ToString()),
                                  (inUse.ToString() == false.ToString() ? "0" : "1"));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string SelectDistinctCulture(DBLangEngine.DatabaseType dbType)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return "SELECT DISTINCT CULTURE FROM FORMITEMS ORDER BY CULTURE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return "SELECT DISTINCT CULTURE FROM FORMITEMS ORDER BY CULTURE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return "SELECT DISTINCT CULTURE FROM " + schema + "FORMITEMS ORDER BY CULTURE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return "SELECT DISTINCT CULTURE FROM " + schema + "FORMITEMS ORDER BY CULTURE; ";
            }
            else
            {
                throw new NotImplementedException();
            }
        }
            
        public static string SelectUsedCultures(DBLangEngine.DatabaseType dbType, string culture)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, IFNULL(INUSE, 0) AS INUSE " +
                       "FROM FORMITEMS " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY APP_FORM, ITEM";

            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, IFNULL(INUSE, 0) AS INUSE " +
                       "FROM FORMITEMS " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY APP_FORM, ITEM";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, COALESCE(INUSE, 0) AS INUSE " +
                       "FROM " + schema + "FORMITEMS " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY APP_FORM, ITEM";

            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, ISNULL(INUSE, 0) AS INUSE " +
                       "FROM " + schema + "FORMITEMS " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY APP_FORM, ITEM";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string SelectUsedMessages(DBLangEngine.DatabaseType dbType, string culture)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return "SELECT MESSAGENAME, VALUE, COMMENT_EN_US, CULTURE, IFNULL(INUSE, 0) AS INUSE " +
                       "FROM MESSAGES " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY MESSAGENAME, VALUE ";


            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return "SELECT MESSAGENAME, VALUE, COMMENT_EN_US, CULTURE, IFNULL(INUSE, 0) AS INUSE " +
                       "FROM MESSAGES " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY MESSAGENAME, VALUE ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return "SELECT MESSAGENAME, VALUE, COMMENT_EN_US, CULTURE, COALESCE(INUSE, 0) AS INUSE " +
                       "FROM " + schema + "MESSAGES " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY MESSAGENAME, VALUE ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return "SELECT MESSAGENAME, VALUE, COMMENT_EN_US, CULTURE, ISNULL(INUSE, 0) AS INUSE " +
                       "FROM " + schema + "MESSAGES " +
                       "WHERE CULTURE = " + DBUtils.MkStr(culture) + " " +
                       "ORDER BY MESSAGENAME, VALUE ";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string UpdateMessage(DBLangEngine.DatabaseType dbType, object message, object messageCulture, object messageName)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return 
                    string.Format("UPDATE MESSAGES SET VALUE = {0} " +
                    "WHERE CULTURE = {1} AND MESSAGENAME = {2}; ",
                    DBUtils.MkStr(message.ToString()),
                    DBUtils.MkStr(messageCulture.ToString()),
                    DBUtils.MkStr(messageName.ToString()));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    string.Format("UPDATE MESSAGES SET VALUE = {0} " +
                    "WHERE CULTURE = {1} AND MESSAGENAME = {2}; ",
                    DBUtils.MkStr(message.ToString()),
                    DBUtils.MkStr(messageCulture.ToString()),
                    DBUtils.MkStr(messageName.ToString()));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("UPDATE " + schema + "MESSAGES SET VALUE = {0} " +
                    "WHERE CULTURE = {1} AND MESSAGENAME = {2}; ",
                    DBUtils.MkStr(message.ToString()),
                    DBUtils.MkStr(messageCulture.ToString()),
                    DBUtils.MkStr(messageName.ToString()));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    string.Format("UPDATE " + schema + "MESSAGES SET VALUE = {0} " +
                    "WHERE CULTURE = {1} AND MESSAGENAME = {2}; ",
                    DBUtils.MkStr(message.ToString()),
                    DBUtils.MkStr(messageCulture.ToString()),
                    DBUtils.MkStr(messageName.ToString()));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string UpdateFormItems(DBLangEngine.DatabaseType dbType, 
                                             object value, object culture, object app, object form, 
                                             object propertyName, object item)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return 
                    string.Format("UPDATE FORMITEMS SET VALUE = {0} " +
                                  "WHERE CULTURE = {1} AND APP_FORM = {2} AND PROPERTYNAME = {3} AND ITEM = {4}; ",
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(item.ToString()));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    string.Format("UPDATE FORMITEMS SET VALUE = {0} " +
                                  "WHERE CULTURE = {1} AND APP_FORM = {2} AND PROPERTYNAME = {3} AND ITEM = {4}; ",
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(item.ToString()));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("UPDATE " + schema + "FORMITEMS SET VALUE = {0} " +
                                  "WHERE CULTURE = {1} AND APP_FORM = {2} AND PROPERTYNAME = {3} AND ITEM = {4}; ",
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(item.ToString()));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    string.Format("UPDATE " + schema + "FORMITEMS SET VALUE = {0} " +
                                  "WHERE CULTURE = {1} AND APP_FORM = {2} AND PROPERTYNAME = {3} AND ITEM = {4}; ",
                                  DBUtils.MkStr(value.ToString()),
                                  DBUtils.MkStr(culture.ToString()),
                                  DBUtils.MkStr(app.ToString() + "." + form.ToString()),
                                  DBUtils.MkStr(propertyName.ToString()),
                                  DBUtils.MkStr(item.ToString()));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string DeleteUnusedMessages(DBLangEngine.DatabaseType dbType, string culture)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return
                    string.Format("DELETE FROM MESSAGES  WHERE IFNULL(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    string.Format("DELETE FROM MESSAGES  WHERE IFNULL(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("DELETE FROM " + schema + "MESSAGES  WHERE COALESCE(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    string.Format("DELETE FROM " + schema + "MESSAGES WHERE ISNULL(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string DeleteUnusedFormItems(DBLangEngine.DatabaseType dbType, string culture)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return
                    string.Format("DELETE FROM FORMITEMS WHERE IFNULL(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    string.Format("DELETE FROM FORMITEMS WHERE IFNULL(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    string.Format("DELETE FROM " + schema + "FORMITEMS WHERE COALESCE(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    string.Format("DELETE FROM " + schema + "FORMITEMS WHERE ISNULL(INUSE, 0) = 0 AND CULTURE = {0}; ", DBUtils.MkStr(culture));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string SelectDumpFormItems(DBLangEngine.DatabaseType dbType)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return
                    "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE " +
                    "FROM FORMITEMS " +
                    "ORDER BY CULTURE, APP_FORM, ITEM, PROPERTYNAME, VALUE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE " +
                    "FROM FORMITEMS " +
                    "ORDER BY CULTURE, APP_FORM, ITEM, PROPERTYNAME, VALUE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE " +
                    "FROM " + schema + "FORMITEMS " +
                    "ORDER BY CULTURE, APP_FORM, ITEM, PROPERTYNAME, VALUE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    "SELECT APP_FORM, ITEM, CULTURE, PROPERTYNAME, VALUETYPE, VALUE, INUSE " +
                    "FROM " + schema + "FORMITEMS " +
                    "ORDER BY CULTURE, APP_FORM, ITEM, PROPERTYNAME, VALUE; ";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string SelectDumpMessages(DBLangEngine.DatabaseType dbType)
        {
            if (dbType == DBLangEngine.DatabaseType.dtSQLite)
            {
                return
                    "SELECT CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE " +
                    "FROM MESSAGES " +
                    "ORDER BY CULTURE, MESSAGENAME, VALUE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMySQL)
            {
                return
                    "SELECT CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE " +
                    "FROM MESSAGES " +
                    "ORDER BY CULTURE, MESSAGENAME, VALUE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtPostgreSQL)
            {
                return
                    "SELECT CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE " +
                    "FROM " + schema + "MESSAGES " +
                    "ORDER BY CULTURE, MESSAGENAME, VALUE; ";
            }
            else if (dbType == DBLangEngine.DatabaseType.dtMSSQL)
            {
                return
                    "SELECT CULTURE, MESSAGENAME, VALUE, COMMENT_EN_US, INUSE " +
                    "FROM " + schema + "MESSAGES " +
                    "ORDER BY CULTURE, MESSAGENAME, VALUE; ";
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
