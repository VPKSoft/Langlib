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
using System.Data.SQLite;
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.Resources;
using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data.SqlClient;
using VPKSoft.ConfLib;
using VPKSoft.Utils;

namespace VPKSoft.LangLib
{
    /// <summary>
    /// A class to enumerate Form / Window objects and properties.
    /// </summary>
    public partial class DBLangEngine: GuiObjectsEnum
    {
        /// <summary>
        /// The application type. 
        /// <para/>WPF (for Windows Presentaton Foundation)
        /// <para/>Winforms (for Windows Forms application)
        /// <para/>Undefined (for throwing exceptions)
        /// </summary>
        private Misc.AppType appType;

        /// <summary>
        /// The constructor for Windows Forms application.
        /// </summary>
        /// <param name="form">The base form for the DBLangEngine to use.</param>
        /// <param name="appType">The application type. Should be AppType.Winforms.</param>
        public DBLangEngine(System.Windows.Forms.Form form, Misc.AppType appType) :
            base(form)
        {
            this.appType = appType;
            ThisForm = form;
            // int the data dir with default
            dataDir = Paths.GetAppSettingsFolder(appType);
        }

        /// <summary>
        /// Whether to use x:Uid's to reference to a FrameworkElement class instance.
        /// </summary>
        private bool useUids = true;

        /// <summary>
        /// The constructor for Windows Presentation Foundation (WPF) application.
        /// </summary>
        /// <param name="window">The base window for the DBLangEngine to use.</param>
        /// <param name="appType">The application type. Should be AppType.WPF.</param>
        /// <param name="useUids">Whether to use x:Uid's to reference to a FrameworkElement class instance.</param>
        public DBLangEngine(System.Windows.Window window, Misc.AppType appType, bool useUids = true) :
            base(window, useUids)
        {
            this.appType = appType;
            this.useUids = useUids;
            ThisWindow = window;
            // int the data dir with default
            dataDir = Paths.GetAppSettingsFolder(appType);
        }        


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
        public void InitalizeLanguage(string messageResource, CultureInfo culture = null, bool loadItems = true)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                InitalizeLanguageSQLite(messageResource, culture, loadItems);
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                InitalizeLanguageMySQL(messageResource, culture, loadItems);
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                InitalizeLanguagePostgreSQL(messageResource, culture, loadItems);
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                InitalizeLanguageMSSQL(messageResource, culture, loadItems);
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// A writable data directory.
        /// </summary>
        private static string dataDir;

        /// <summary>
        /// A default database name.
        /// </summary>
        private static string dbName = string.Empty;


        /// <summary>
        /// A database connection port.
        /// </summary>
        private static UInt16 dbPort = 0;

        /// <summary>
        /// A database host name.
        /// </summary>
        private static string dbHost = string.Empty;

        /// <summary>
        /// A database user name.
        /// </summary>
        private static string dbUser = string.Empty;

        /// <summary>
        /// A password to a database.
        /// </summary>
        private static string dbPassword = string.Empty;

        /// <summary>
        /// An ADO.NET data provider connection string to a database.
        /// </summary>
        private static string dbConnectionStr = string.Empty;

        /// <summary>
        /// A schema to use if the target database uses schemas.
        /// </summary>
        private static string dbSchema = string.Empty;

        /// <summary>
        /// The type of the database to use.
        /// </summary>
        private static DatabaseType dbType = DatabaseType.dtSQLite;

        /// <summary>
        /// Indicates if the library should construct database tables.
        /// </summary>
        private static bool dbNoTables = false;


        /// <summary>
        /// Sets the database type to a given value and the default schema
        /// <para/>in case of PostgreSQL and Microsoft SQL Server.
        /// </summary>
        /// <param name="dbTypeSet">A database type.</param>
        public static void SetDBType(DatabaseType dbTypeSet)
        {
            dbType = dbTypeSet;
            switch(dbType)
            {
                case DatabaseType.dtMSSQL:
                    dbSchema = "dbo";
                    dbPort = 1433;
                    break;
                case DatabaseType.dtPostgreSQL:
                    dbSchema = "public";
                    dbPort = 5432;
                    break;
                case DatabaseType.dtMySQL:
                    dbPort = 3306;
                    dbSchema = string.Empty;
                    break;
                case DatabaseType.dtSQLite:
                    dbPort = 0;
                    dbSchema = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Checks if the program should run database configuration tool.
        /// <para/>This is indicated by command line argument "--configureLang".
        /// </summary>
        /// <returns>True if a database configuration should be run, otherwise false.</returns>
        public static bool ShoudConfigure()
        {
            ProgramArgumentCollection args = new ProgramArgumentCollection();
            return args["--configureLang"] == "1";
        }

        /// <summary>
        /// Runs the SecureDatabaseSetting program.
        /// </summary>
        /// <returns>True if the SecureDatabaseSetting was run successfully, otherwise false.</returns>
        public static bool RunConfigurator()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo(Paths.AppInstallDir + "SecureDatabaseSetting.exe");
                si.Arguments = "--program=" + "\"" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace(".vshost", string.Empty) + "\"";
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(si);
                proc.WaitForExit();
                return proc.ExitCode == 0;
            } 
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get an assembly of the application depending of
        /// <para/>the application type (<paramref name="appType"/>).
        /// </summary>
        /// <param name="appType">The application type.</param>
        /// <returns>The assembly of the application.</returns>
        private static string GetAssemblyName(Misc.AppType appType)
        {
            if (appType == Misc.AppType.Winforms)
            {
                return System.Windows.Forms.Application.ProductName;
            }
            else if (appType == Misc.AppType.WPF)
            {
                return System.Windows.Application.ResourceAssembly.GetName().Name;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the SQLite database file name residing in DataDir.
        /// <para/>The default is lang.sqlite.
        /// <para/>If a file called dbconfig.sqlite resides in the DataDir,
        /// <para/>the file is loaded and database connection is set
        /// <para/>the way it's defined in the dbconfig.sqlite file.
        /// </summary>
        public static string DBName
        {
            get
            {
                return dbName;
            }

            set
            {
                if (File.Exists(dataDir + "dbconfig.sqlite"))
                {
                    dbName = value;
                    LoadSettings();
                    return;
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException("Empty string is not allowed.");
                }

                dbName = value;
                SetDBType(DatabaseType.dtSQLite);
                foreach (char chr in Path.GetInvalidFileNameChars())
                {
                    dbName = dbName.Replace(chr, '_');
                }
            }
        }

       
        /// <summary>
        /// Loads database settings from a SQLite database called dbconfig.sqlite.
        /// </summary>
        private static void LoadSettings()
        {
            Conflib cl = new Conflib();
            cl.DataDir = dataDir;
            cl.DBName = "dbconfig.sqlite";

            cl.AutoCreateSettings = true;
            try
            {
                KeyValuePair<DBLangEngine.DatabaseType, string> v = new KeyValuePair<DBLangEngine.DatabaseType, string>((DBLangEngine.DatabaseType)Enum.Parse(typeof(DBLangEngine.DatabaseType), cl["dbType"]), cl["dbType"]);
                dbType = v.Key;
            }
            catch
            {
                dbType = DatabaseType.dtSQLite;
                return;
            }

            dbHost = cl["dbServer"];
            dbName = cl["dbDatabase"];
            dbUser = cl["dbUser"];
            dbPassword = cl["dbPassword"];
            dbSchema = cl["dbSchema"];
            dbPort = (UInt16)decimal.Parse(cl["dbPort"]);
            dbPassword = cl["dbPassword"];
            dbConnectionStr = bool.Parse(cl["dbConnStrOverride"]) ? cl["dbConnStr"] : string.Empty;
            try
            {
                dbNoTables = bool.Parse(cl["dbNoTables"]);
            }
            catch
            {
                dbNoTables = false;
            }
        }


        /// <summary>
        /// Gets or sets a writable directory where the settings should be saved.
        /// <para/>The default is "[...]\AppData\Local\[Assembly product name]."
        /// </summary>
        public static string DataDir
        {
            get
            {
                return dataDir;
            }

            set
            {
                if (value == string.Empty)
                {
                    throw new ArgumentException("Empty string is not allowed.");
                }

                dataDir = value;
                foreach (char chr in Path.GetInvalidPathChars())
                {
                    dataDir = dataDir.Replace(chr, '_');
                }

                dataDir = dataDir.EndsWith(@"\") ? dataDir : dataDir + @"\";
            }            
        }

        /// <summary>
        /// If the messages were saved to the database 
        /// <para/>unsing the FallBackCulture property.
        /// </summary>
        private static bool messagesSaved = false;

        /// <summary>
        /// If the INUSE field int the database was updated.
        /// </summary>
        private static bool langUseUpdated = false;

        /// <summary>
        /// If the entire culture list supported by
        /// <para/>the .NET Framework were inserted into to
        /// <para/>language database.
        /// </summary>
        private static bool culturesInserted = false;

        /// <summary>
        /// If all the database tables required by the
        /// <para/>library where created.
        /// </summary>
        private static bool tablesCreated = false;

        /// <summary>
        /// Application product name and the
        /// <para/>underlying form name combined with a dot (.).
        /// </summary>
        private string parentItem = string.Empty;

        /// <summary>
        /// The SQLite database connection to be used for this library.
        /// </summary>
        private static SQLiteConnection DBLangConnnection = null;

        /// <summary>
        /// The MySQL database connection to be used for this library.
        /// </summary>
        private static MySqlConnection MySQLConnection = null;

        /// <summary>
        /// The PostgreSQL database connection to be used for this library.
        /// </summary>
        private static NpgsqlConnection PostgreConnection = null;

        /// <summary>
        /// The Microsoft SQL Server database connection to be used for this library.
        /// </summary>
        private static SqlConnection MSSQLConnection = null;

        /// <summary>
        /// If the library should buffer the language items
        /// <para/>to be inserted into the database.
        /// <para/>This is to avoid slowness created by
        /// <para/>multiple transactions instead of one.
        /// </summary>
        private bool buffer = false;

        /// <summary>
        /// Used as a buffer for SQL sentences to avoid
        /// <para/>too small transactions.
        /// </summary>
        private string sql_entry = string.Empty;

        /// <summary>
        /// A list of forms / windows that already
        /// <para/>have been enumerated to avoid
        /// <para/>redoing the enumeration process.
        /// </summary>
        private static List<string> formNames = new List<string>();

        /// <summary>
        /// A buffer for translated messages. This is to avoid querying
        /// <para/>them from the database multiple times.
        /// </summary>
        private List<StringMessages> messages = new List<StringMessages>();

        /// <summary>
        /// A static buffer for translated messages. 
        /// <para/>This is to avoid querying
        /// <para/>them from the database multiple times.
        /// </summary>
        private static List<StringMessages> statMessages = new List<StringMessages>();

        /// <summary>
        /// The fall-back culture to be used if the 
        /// <para/>current culture is not found from the language database.
        /// <para/>The default is "en-US" (1033) which should be used
        /// <para/>to ease up the localization process.
        /// </summary>
        private static CultureInfo fallBackCulture = new CultureInfo(1033);

        /// <summary>
        /// The fall-back culture to be used if the 
        /// <para/>current culture is not found from the language database.
        /// <para/>The default is "en-US" (1033) which should be used
        /// <para/>to ease up the localization process.
        /// </summary>
        public static CultureInfo FallBackCulture
        {
            get
            {
                return fallBackCulture;
            }

            set
            {
                fallBackCulture = value;
            }
        }

        /// <summary>
        /// A type of a database.
        /// </summary>
        public enum DatabaseType
        {
            /// <summary>
            /// A SQLite database
            /// </summary>
            [Description("SQLite database")]
            dtSQLite,

            /// <summary>
            /// A MySQL database
            /// </summary>
            [Description("MySQL database")]
            dtMySQL,

            /// <summary>
            /// A Microsoft SQL Server database.
            /// </summary>
            [Description("Microsoft SQL server database")]
            dtMSSQL,

            /// <summary>
            /// A PostgreSQL database.
            /// </summary>
            [Description("PostgreSQL server database")]
            dtPostgreSQL
        }

        /// <summary>
        /// A base exception type for the LangLib
        /// for general exceptions.
        /// </summary>
        public class LangLibException: Exception
        {
            /// <summary>
            /// The LangLibException class constructor.
            /// </summary>
            /// <param name="message">Initializes a new instance of the LangLibException class with a specified error message.</param>
            public LangLibException(string message):
                base(message)
            {

            }
        }



        /// <summary>
        /// The total time the library has spent in the
        /// <para/>InitalizeLanguage method.
        /// </summary>
        private static TimeSpan ts = new TimeSpan();

        /// <summary>
        /// The total time the library has spent in the
        /// <para/>InitalizeLanguage method in seconds. 
        /// <para/>This property is mostly for testing and optimization purposes.
        /// </summary>
        public double InitTime
        {
            get
            {
                return ts.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the total time the library has spent in the
        /// <para/>InitalizeLanguage method.
        /// <para/>This property is mostly for testing and optimization purposes.
        /// </summary>
        public TimeSpan InitTimeSpan
        {
            get
            {
                return ts;
            }
        }

        /// <summary>
        /// Gets a message based on a name and current culture from cache.
        /// <para/>If the cache does not have the message, a database search is executed
        /// <para/>and <see cref="FallBackCulture"/> culture is used as fallback.
        /// <para/>If the database does not have a message the default message is used.
        /// </summary>
        /// <param name="name">The name of the message to get</param>
        /// <param name="defaultMessage">The default message</param>
        /// <param name="items">Parameters for formatting the message.</param>
        /// <returns>A message based on the rules in the summary.</returns>
        public static string GetStatMessage(string name, string defaultMessage, params object[] items)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return GetStatMessageSQLite(name, CultureInfo.CurrentCulture, defaultMessage, items);
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                return GetStatMessageMySQL(name, CultureInfo.CurrentCulture, defaultMessage, items);
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return GetStatMessagePostgreSQL(name, CultureInfo.CurrentCulture, defaultMessage, items);
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return GetStatMessageMSSQL(name, CultureInfo.CurrentCulture, defaultMessage, items);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a message based on a name and current culture from cache.
        /// <para/>If the cache does not have the message, a database search is executed
        /// <para/>and FallBackCulture culture is used as fallback.
        /// <para/>If the database does not have a message the default message is used.
        /// </summary>
        /// <param name="name">The name of the message to get</param>
        /// <param name="defaultMessage">The default message</param>
        /// <param name="items">Parameters for formatting the message.</param>
        /// <returns>A message based on the rules in the summary.</returns>
        public string GetMessage(string name, string defaultMessage, params object[] items)
        {
            return GetMessage(name, CultureInfo.CurrentCulture, defaultMessage, items);
        }

        /// <summary>
        /// Gets a list of CultureInfo class instances which are localized.
        /// <para/>(Exists in the database).
        /// </summary>
        /// <returns>A list of CultureInfo class instances which are localized.</returns>
        public List<CultureInfo> GetLocalizedCultures()
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return GetLocalizedCulturesSQLite();
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                return GetLocalizedCulturesMySQL();
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return GetLocalizedCulturesPostgreSQL();
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return GetLocalizedCulturesMSSQL();
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Begins a buffer for SQL sentences to avoid
        /// <para/>small transactions. The "buffer" is executed
        /// <para/>when the EndBuffer method is called and
        /// <para/>then cleared.
        /// </summary>
        public void BeginBuffer()
        {
            buffer = true;
            sql_entry = string.Empty;
        }

        /// <summary>
        /// Executes a transaction (a sequence of SQL sentences) from
        /// <para/>the "buffer". After completion of the "buffer" excecution,
        /// <para/>the "buffer" is cleared and buffering is disabled.
        /// </summary>
        public void EndBuffer()
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                EndBufferSQLite();
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                EndBufferMySQL();
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                EndBufferPostgreSQL();
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                EndBufferMSSQL();
            }
        }

        /// <summary>
        /// Splits a message into value and comment part. A message is split
        /// <para/>from the last occurence of an or character (|). 
        /// <para/>E.g. "A test message. Or characters (|) may also exist in the message.|As in a test message."
        /// </summary>
        /// <param name="i">A DictionaryEntry class instance which Value part is
        /// <para/>split into value and comment part.</param>
        /// <param name="value">A string where the message part of the message is placed.</param>
        /// <param name="comment">A string where the comment part of the message is placed.</param>
        private static void SplitMessage(DictionaryEntry i, out string value, out string comment)
        {            
            SplitMessage(i.Value.ToString(), out value, out comment);
        }

        /// <summary>
        /// Splits a message into value and comment part. A message is split
        /// <para/>from the last occurence of an or character (|). 
        /// <para/>E.g. "A test message. Or characters (|) may also exist in the message.|As in a test message."
        /// </summary>
        /// <param name="msg">A string which is split into value and comment part.</param>
        /// <param name="value">A string where the message part of the message is placed.</param>
        /// <param name="comment">A string where the comment part of the message is placed.</param>
        private static void SplitMessage(string msg, out string value, out string comment)
        {
            try
            {
                int ind = msg.LastIndexOf('|');
                if (ind != -1)
                {

                }
                value = msg.Substring(0, ind);
                comment = msg.Substring(ind + 1);
            }
            catch
            {
                value = msg;
                comment = string.Empty;
            }
        }


        /// <summary>
        /// Internal class for caching localization items.
        /// </summary>
        private class DBCacheHolder
        {
            /// <summary>
            /// Item name.
            /// </summary>
            public string Item = string.Empty;

            /// <summary>
            /// Property name.
            /// </summary>
            public string PropertyName = string.Empty;

            /// <summary>
            /// Value type.
            /// </summary>
            public string ValueType = string.Empty;

            /// <summary>
            /// Value.
            /// </summary>
            public string Value = string.Empty;

            /// <summary>
            /// If the item is in use or not as in the database "point of view".
            /// </summary>
            public bool InUse = true;

            /// <summary>
            /// If item from the database matches the culture
            /// <para/>one wished to get.
            /// </summary>
            public bool NotFallBackLang = true;

            /// <summary>
            /// Culture as in Culture.ToString().
            /// </summary>
            public string Culture = string.Empty;

            /// <summary>
            /// A combination of the applications assembly name and
            /// <para/>the underlying form / window name.
            /// </summary>
            public string AppForm = string.Empty;

            /// <summary>
            /// Basic constructor.
            /// </summary>
            public DBCacheHolder()
            {

            }

            /// <summary>
            /// A constructor that initializes DBCacheHolder
            /// <para/>properties with values gotten from a DataReader instance
            /// <para/>implementing the IDataReader interface.
            /// </summary>
            /// <param name="dr">a DataReader instance implementing the IDataReader interface.</param>
            public DBCacheHolder(IDataReader dr)
            {
                Item = dr.GetString(3);
                PropertyName = dr.GetString(2);
                ValueType = dr.GetString(0);
                Value = dr.GetString(1);
                InUse = dr.GetInt32(4) == 1;
                NotFallBackLang = dr.GetInt32(7) == 0;
                Culture = dr.GetString(5);
                AppForm = dr.GetString(6);
            }

            /// <summary>
            /// Checks if the AppForm is already in the cache.
            /// </summary>
            /// <param name="appForm">A combination of the applications assembly name and
            /// <para/>the underlying form / window name.</param>
            /// <param name="list">A list of DBCacheHolder instances.</param>
            /// <returns>True if the AppForm is already in the given list.</returns>
            public static bool ListContains(string appForm, List<DBCacheHolder> list)
            {
                foreach (DBCacheHolder dc in list)
                {
                    if (dc.AppForm == appForm)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Internal cache for form / window objects to avoid running same SQL queries.
        /// </summary>
        private static List<DBCacheHolder> DBCache = new List<DBCacheHolder>();

        /// <summary>
        /// Run this to clear the internal cache.
        /// <para/>This is usefull if you want change the
        /// <para/>UI language on the fly.
        /// </summary>
        public static void ClearInternalCache()
        {
            DBCache.Clear();
        }

        /// <summary>
        /// Load all the language items for the underlying 
        /// <para/>form / window using the system's current culture.
        /// <para/>If the culture is not found in the database, the FallBackCulture is used.
        /// </summary>
        public void LoadLanguageItems()
        {
            LoadLanguageItems(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Load all the language items for the underlying 
        /// form / window using the given culture.
        /// </summary>
        /// <param name="ci">Culture to use. If the given culture 
        /// <para/>is not found in the database, the FallBackCulture is used.</param>
        /// <returns>True if the operation was successfull, otherwise false.</returns>
        public bool LoadLanguageItems(CultureInfo ci)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                RunDBCacheSQLite(AppForm, ci);
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                RunDBCacheMySQL(AppForm, ci);
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                RunDBCachePostgreSQL(AppForm, ci);
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                RunDBCacheMSSQL(AppForm, ci);
            }
            else
            {
                throw new NotImplementedException();
            }

            bool retVal = true;
            foreach (GuiObject go in this)
            {
                if (!go.SetValue())
                {
                    retVal = false;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Saves the object's items to be localized into the language database.
        /// The system's current culture is used.
        /// </summary>
        /// <param name="obj">An object instance which items should be saved.</param>
        public void SaveLanguageItems(object obj)
        {
            SaveLanguageItems(obj, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the object of the underlying form / window
        /// <para/>as GuiObject class instances and "marks" them with
        /// <para/>given culture.
        /// </summary>
        /// <param name="ci">A culture to "mark" the GuiObject class instance.</param>
        /// <param name="propertyNames">Names of the properties to include in the object list.
        /// <para/>If the value is null, no property names are prevented.</param>
        public void GetGuiObjets(CultureInfo ci, List<string> propertyNames = null)
        {
            if (this.appType == Misc.AppType.Winforms)
            {
                GetObjects(this.BaseInstance as System.Windows.Forms.Form, ci, true, propertyNames);
            }
            else if (this.appType == Misc.AppType.WPF)
            {
                GetObjects(this.BaseInstance as System.Windows.Window, ci, true, propertyNames);
            }

            if (formNames.Contains(BaseInstanceName))
            {
                return;
            }
            else
            {
                formNames.Add(BaseInstanceName);
                parentItem = BaseInstanceProduct + "." + BaseInstanceName;
            }
        }

        /// <summary>
        /// Saves the object's items to be localized into the language database.
        /// The given current culture is used.
        /// </summary>
        /// <param name="obj">An object instance which items should be saved.</param>
        /// <param name="ci">Culture to use.</param>
        public void SaveLanguageItems(object obj, CultureInfo ci)
        {
            BeginBuffer();

            foreach (GuiObject go in this)
            {
                if (dbType == DatabaseType.dtSQLite)
                {
                    InsertLangItemSQLite(go.AppForm, go.Item, go.PropertyName, go.ValueType, go.Value.ToString(), ci);
                }
                else if (dbType == DatabaseType.dtMySQL)
                {
                    InsertLangItemMySQL(go.AppForm, go.Item, go.PropertyName, go.ValueType, go.Value.ToString(), ci);
                }
                else if (dbType == DatabaseType.dtPostgreSQL)
                {
                    InsertLangItemPostgreSQL(go.AppForm, go.Item, go.PropertyName, go.ValueType, go.Value.ToString(), ci);
                }
                else if (dbType == DatabaseType.dtMSSQL)
                {
                    InsertLangItemMSSQL(go.AppForm, go.Item, go.PropertyName, go.ValueType, go.Value.ToString(), ci);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            EndBuffer();
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
        public string GetMessage(string name, CultureInfo ci, string defaultMessage, params object[] items)
        {
            if (dbType == DatabaseType.dtSQLite)
            {
                return GetMessageSQLite(name, ci, defaultMessage, items);
            }
            else if (dbType == DatabaseType.dtMySQL)
            {
                return GetMessageMySQL(name, ci, defaultMessage, items);
            }
            else if (dbType == DatabaseType.dtPostgreSQL)
            {
                return GetMessagePostgreSQL(name, ci, defaultMessage, items);
            }
            else if (dbType == DatabaseType.dtMSSQL)
            {
                return GetMessageMSSQL(name, ci, defaultMessage, items);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
