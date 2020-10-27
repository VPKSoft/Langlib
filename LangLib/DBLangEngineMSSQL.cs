#region License
/*
LangLib

A program and library for application localization.
Copyright (C) 2020 VPKSoft, Petteri Kautonen

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
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.Resources;
using System.IO;
using System.Windows;
using System.Data.SqlClient;
using VPKSoft.Utils;

namespace VPKSoft.LangLib
{
    /// <summary>
    /// A class to enumerate Form / Window objects and properties.
    /// </summary>
    public partial class DBLangEngine : GuiObjectsEnum
    {
        /// <summary>
        /// The most important method in LangLib. This creates the database,
        /// <para/>the tables to it (MESSAGES, FORMITEMS, CULTURES).
        /// <para/><para/>Also the FallBackCulture is updated for the underlying form/window.
        /// <para/>Messages from the given <paramref name="messageResource"/> are inserted to
        /// <para/>the database if their don't exists.
        /// <para/><para/>The table fields FORMITEMS.INUSE and MESSAGES.INUSE are updated
        /// <para/>for the FallBackCulture.
        /// </summary>
        /// <param name="messageResource">A resource name that contains the application
        /// <para/>messages in the fall FallBackCulture language.
        /// <para/>For example if I have an application which assembly name is
        /// <para/>LangLibTestWinforms and it has a .resx file called Messages
        /// <para/>I would give this parameter a value of "LangLibTestWinforms.Messages".</param>
        /// <param name="culture">The culture to use for the localization.
        /// <para/>If no culture is given the current system culture is used and
        /// <para/>the FallBackCulture is used as fallback culture.</param>
        /// <param name="loadItems">To load language items or not.</param>
        public void InitalizeLanguageMSSQL(string messageResource, CultureInfo culture = null, bool loadItems = true)
        {
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            if (!Design)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    if (!Directory.Exists(DataDir))
                    {
                        Directory.CreateDirectory(DataDir);
                    }

                    if (MSSQLConnection == null)
                    {
                        string connStr = dbConnectionStr == string.Empty ? string.Format("Persist Security Info=False;User ID={3};Password={4};Initial Catalog={2};Server={0};", dbHost, dbPort, dbName, dbUser, dbPassword) : dbConnectionStr;
                        MSSQLConnection = new SqlConnection(connStr);
                        MSSQLConnection.Open();                        
                    }

                    if (!tablesCreated && !dbNoTables)
                    {
                        using (SqlCommand command = new SqlCommand(TableCreateMessages, MSSQLConnection))
                        {
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand(TableCreateFormItems, MSSQLConnection))
                        {
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand(TableCreateCultures, MSSQLConnection))
                        {
                            command.ExecuteNonQuery();
                        }
                        tablesCreated = true;
                    }

                    if (!culturesInserted && !loadItems)
                    {
                        string sql = string.Empty;
                        CultureInfo[] allCultures;
                        allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

                        foreach (CultureInfo ci in allCultures)
                        {
                            sql += InsertCulture(ci);
                        }


                        using (SqlTransaction trans = MSSQLConnection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(sql, MSSQLConnection, trans))
                            {
                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        culturesInserted = true;
                    }

                    if (!loadItems)
                    {
                        GetGuiObjets(fallBackCulture);
                        if (!langUseUpdated)
                        {
                            using (SqlTransaction trans = MSSQLConnection.BeginTransaction())
                            {
                                using (SqlCommand command = new SqlCommand("UPDATE " + Schema + "FORMITEMS SET INUSE = 0 WHERE CULTURE = '" + fallBackCulture + "' ", MSSQLConnection, trans))
                                {
                                    command.ExecuteNonQuery();
                                }
                                trans.Commit();
                            }
                            langUseUpdated = true;
                        }
                        SaveLanguageItems(this.BaseInstance, fallBackCulture);
                    }

                    if (loadItems)
                    {
                        List<string> localProps = LocalizedPropsMSSQL(AppForm, CultureInfo.CurrentCulture);
                        GetGuiObjets(CultureInfo.CurrentCulture, localProps);
                        LoadLanguageItems(CultureInfo.CurrentCulture);
                    }

                    if (!loadItems)
                    {
                        SaveMessages(messageResource, Assembly.GetExecutingAssembly(), ref MSSQLConnection);
                    }

                    ts = ts.Add((DateTime.Now - dt));
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(MissingManifestResourceException)) // This is fun. The user actually gets some help from the exception message ;-)
                    {
                        throw new LangLibException(string.Format("Missing resource '{0}'.{1}Perhaps {2}.[Resource file name] would do,{3}without the .resx file extension.",
                            messageResource, Environment.NewLine, GetAssemblyName(this.appType), Environment.NewLine));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Gets all items for the given <paramref name="app_form"/> bases on the given culture.
        /// <para/>If the given culture does not exist the FallBackCulture is used.
        /// </summary>
        /// <param name="app_form">A combination of the applications assembly name and
        /// <para/>the underlying form / window name.</param>
        /// <param name="ci">A culture to use for getting the form / window items.</param>
        public void RunDBCacheMSSQL(string app_form, CultureInfo ci)
        {
            // Let's not use database connection if already cached.
            if (DBCacheHolder.ListContains(app_form, DBCache))
            {
                foreach (GuiObject go in this)
                {
                    try
                    {
                        DBCacheHolder dc = DBCache.First(first => first.PropertyName == go.PropertyName && first.Item == go.Item && first.AppForm == app_form);
                        if (dc != null)
                        {
                            if (dc.ValueType == "System.String")
                            {
                                go.Value = dc.Value;
                            }
                        }
                    }
                    catch
                    {
                        // something wrong?
                    }
                }
                return;
            }

            List<string> handled = new List<string>();
            using (SqlCommand command = new SqlCommand(SelectDBCache(app_form, ci), MSSQLConnection))
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string item = dr.GetString(3);
                        string propertyName = dr.GetString(2);
                        string valueType = dr.GetString(0);
                        string value = dr.GetString(1);

                        foreach (GuiObject go in this)
                        {
                            if (go.PropertyName == propertyName &&
                                go.Item == item)
                            {
                                if (handled.Contains(go.PropertyName + "." + go.Item))
                                {
                                    continue;
                                }

                                try
                                {
                                    DBCache.Add(new DBCacheHolder(dr));
                                }
                                catch
                                {
                                    // Database connection error or internal logic failure? Well we can't let the application fall.
                                }

                                handled.Add(go.PropertyName + "." + go.Item);
                                if (valueType == "System.String")
                                {
                                    go.Value = value;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saved the application messages if the weren't already saved.
        /// <para/>The database field MESSAGES.INUSE is also updated for all saved cultures.
        /// </summary>
        /// <param name="resourcefile">A resource name that contains the application
        /// <para/>messages in the fall FallBackCulture language.
        /// <para/>For example if I have an application which assembly name is
        /// <para/>LangLibTestWinforms and it has a .resx file called Messages
        /// <para/>I would give this parameter a value of "LangLibTestWinforms.Messages"</param>
        /// <param name="assembly">The product's assembly (executing assembly).</param>
        /// <param name="conn">Reference to a SqlConnection class instance.</param>
        public static void SaveMessages(string resourcefile, Assembly assembly, ref SqlConnection conn)
        {
            if (messagesSaved)
            {
                return;
            }

            if (!messagesSaved)
            {
                messagesSaved = true;
            }

            string sql = string.Empty;
            sql += "UPDATE " + Schema + "MESSAGES SET INUSE = 0 WHERE CULTURE = " + DBUtils.MkStr(fallBackCulture.Name) + "; ";

            assembly = Assembly.GetEntryAssembly();

            ResourceManager rm = new ResourceManager(resourcefile, assembly);
            ResourceSet rs = rm.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            foreach (DictionaryEntry i in rs)
            {
                string value, comment;
                SplitMessage(i, out value, out comment);
                sql += InsertMessage(i.Key.ToString(), value, comment);
            }

            using (SqlTransaction trans = conn.BeginTransaction())
            {
                using (SqlCommand command = new SqlCommand(sql, conn, trans))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }
        }

        /// <summary>
        /// Gets a list of strings of properties to localize from
        /// <para/>from the language database.
        /// </summary>
        /// <param name="app_form">Application product name concatenated with dot (.)
        /// <para/>a form or window name.</param>
        /// <param name="ci">A culture to use for getting the form / window items.</param>
        /// <returns>A list of strings of properties to localize from
        /// <para/>from the language database.</returns>
        public List<string> LocalizedPropsMSSQL(string app_form, CultureInfo ci)
        {
            List<string> handled = new List<string>();
            using (SqlCommand command = new SqlCommand(SelectLocalizedProps(app_form, ci), MSSQLConnection))
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string item = dr.GetString(3);
                        string propertyName = dr.GetString(2);
                        string valueType = dr.GetString(0);
                        string value = dr.GetString(1);
                        if (handled.Contains(item + "." + propertyName))
                        {
                            continue;
                        }
                        handled.Add(item + "." + propertyName);
                    }
                }
            }
            return handled;
        }


        /// <summary>
        /// Inserts a single language item to the database or buffers
        /// <para/>it if the BeginBuffer method has been called before.
        /// </summary>
        /// <param name="app_form">A combination of the applications assembly name and
        /// <para/>the underlying form / window name.</param>
        /// <param name="item">An item name. E.g. "Form1".</param>
        /// <param name="propertyName">A property name. E.g. "Text".</param>
        /// <param name="valueType">A value type. E.g. "System.String".</param>
        /// <param name="value">A property value. E.g. "Form1".</param>
        /// <param name="ci">The culture in which language the item is.
        /// <para/>The database field FORMITEMS.INUSE is also updated to value of 1.</param>
        public void InsertLangItemMSSQL(string app_form, string item, string propertyName, string valueType, string value, CultureInfo ci)
        {
            if (buffer)
            {
                sql_entry += InsertLangItem(app_form, item, propertyName, valueType, value, ci);
            }
            else
            {
                using (SqlCommand command = new SqlCommand(InsertLangItem(app_form, item, propertyName, valueType, value, ci), MSSQLConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Gets a message based on a name and culture from cache.
        /// <para/>If the cache does not have the message, a database search is executed
        /// <para/>and <see cref="FallBackCulture"/> culture is used as fallback.
        /// <para/>If the database does not have a message the default message is used.
        /// </summary>
        /// <param name="name">The name of the message to get</param>
        /// <param name="ci">Culture for the message</param>
        /// <param name="defaultMessage">The default message</param>
        /// <param name="items">Parameters for formatting the message.</param>
        /// <returns>A message based on the rules in the summary.</returns>
        public static string GetStatMessageMSSQL(string name, CultureInfo ci, string defaultMessage, params object[] items)
        {
            foreach (StringMessages m in statMessages)
            {
                if (m.MessageName == name)
                {
                    try
                    {
                        return string.Format(m.Message, items);
                    }
                    catch
                    {
                        return m.Message;
                    }
                }
            }

            string connStr = dbConnectionStr == string.Empty ? string.Format("Persist Security Info=False;User ID={3};Password={4};Initial Catalog={2};Server={0};", dbHost, dbPort, dbName, dbUser, dbPassword) : dbConnectionStr;
            SqlConnection statDBLangConnnection = new SqlConnection(connStr);
            statDBLangConnnection.Open();
            using (statDBLangConnnection)
            {
                using (SqlCommand command = new SqlCommand(SelectStatMessage(ci.Name, name), statDBLangConnnection))
                {
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!statMessages.Contains(new StringMessages(name, dr.GetString(0))))
                            {
                                statMessages.Add(new StringMessages(name, dr.GetString(0)));
                            }

                            try
                            {
                                return string.Format(dr.GetString(0), items);
                            }
                            catch
                            {
                                return dr.GetString(0);
                            }
                        }
                        else
                        {
                            string value, comment;
                            SplitMessage(defaultMessage, out value, out comment);
                            try
                            {
                                return string.Format(value, items);
                            }
                            catch
                            {
                                return value;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets a message based on a name and culture from cache.
        /// <para/>If the cache does not have the message, a database search is executed
        /// <para/>and <see cref="FallBackCulture"/> culture is used as fallback.
        /// <para/>If the database does not have a message the default message is used.
        /// </summary>
        /// <param name="name">The name of the message to get</param>
        /// <param name="ci">Culture for the message</param>
        /// <param name="defaultMessage">The default message</param>
        /// <param name="items">Parameters for formatting the message.</param>
        /// <returns>A message based on the rules in the summary.</returns>
        public string GetMessageMSSQL(string name, CultureInfo ci, string defaultMessage, params object[] items)
        {
            foreach (StringMessages m in messages)
            {
                if (m.MessageName == name)
                {
                    try
                    {
                        return string.Format(m.Message, items);
                    }
                    catch
                    {
                        return m.Message;
                    }
                }
            }
            using (SqlCommand command = new SqlCommand(SelectMessage(ci.Name, name), MSSQLConnection))
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (!messages.Contains(new StringMessages(name, dr.GetString(0))))
                        {
                            messages.Add(new StringMessages(name, dr.GetString(0)));
                        }

                        try
                        {
                            return string.Format(dr.GetString(0), items);
                        }
                        catch
                        {
                            return dr.GetString(0);
                        }

                    }
                    else
                    {
                        string value, comment;
                        SplitMessage(defaultMessage, out value, out comment);
                        try
                        {
                            return string.Format(value, items);
                        }
                        catch
                        {
                            return value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of CultureInfo class instances which are localized.
        /// <para/>(Exists in the database).
        /// </summary>
        /// <returns>A list of CultureInfo class instances which are localized</returns>
        public List<CultureInfo> GetLocalizedCulturesMSSQL()
        {
            List<CultureInfo> retVal = new List<CultureInfo>();
            using (SqlCommand command = new SqlCommand("SELECT DISTINCT CULTURE FROM " + Schema + "FORMITEMS WITH (NOLOCK) ", MSSQLConnection))
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        retVal.Add(CultureInfo.GetCultureInfo(dr.GetString(0)));
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Executes a transaction (a sequence of SQL sentences) from
        /// <para/>the "buffer". After completion of the "buffer" excecution,
        /// <para/>the "buffer" is cleared and buffering is disabled.
        /// </summary>
        public void EndBufferMSSQL()
        {
            if (buffer)
            {
                buffer = false;
                using (SqlTransaction trans = MSSQLConnection.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand(sql_entry, MSSQLConnection, trans))
                    {
                        command.CommandText = sql_entry;
                        command.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                sql_entry = string.Empty;
            }
        }
    }
}