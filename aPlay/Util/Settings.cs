using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
namespace Xcom.aPlay.Util
{
    public class Settings
    {
        protected static string _defaultSettingsFilePath;



        static Settings()
        {
            string settingFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + "aPlay";

            _defaultSettingsFilePath = settingFolder + Path.DirectorySeparatorChar + "setting.xml";

            Default = new Settings();
            Default.LibraryDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + Path.DirectorySeparatorChar + "aPlay" + Path.DirectorySeparatorChar + "library.db";
            Default.MainMusicFolderPath = new DirectoryInfo( Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            //TODO Get video path.
            Default.MainVideoFolderPath = new DirectoryInfo( Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        public static Settings Default { get; private set; }








        Dictionary<string, string> _d_table;

        public Settings()
        {

            _d_table = new Dictionary<string, string>();
        }
        /// <summary>
        /// Loads the settings from the default settings file path. 
        /// </summary>
        public bool Load()
        {


           return Load(_defaultSettingsFilePath);

            
        }
        /// <summary>
        /// Load settings from a file.
        /// </summary>
        /// <param name="settingsFilePath">The file path.</param>
        public bool Load(string settingsFilePath)
        {

           //Don't bother loading the file if dosn't exist.
            if (!File.Exists(settingsFilePath))
            {
                SetToDefault();
                return false;
            }
            XmlSerializer loader = new XmlSerializer(typeof(Dictionary<string, object>));
            using (FileStream file = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _d_table =  loader.Deserialize(file) as Dictionary<string,string>;
                if (_d_table == null)
                {
                    SetToDefault();   
                    return false;
                }
            }
            return true;
           
         
        }
        /// <summary>
        /// Set all settings to default settings.
        /// </summary>
        void SetToDefault()
        {
            _d_table = new Dictionary<string, string>(Default._d_table);
        }

        public void Save()
        {
            Save(_defaultSettingsFilePath);
        }

        public void Save(string path)
        {
            XmlSerializer saver = new XmlSerializer(typeof(Dictionary<string, string>));
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                saver.Serialize(file, _d_table);
            }
        }
        

        /// <summary>
        /// Gets the value for a setting.
        /// </summary>
        /// <param name="setting_name">The setting name.</param>
        /// <returns>The setting's value. Returns null if not set.</returns>
        public string GetValue(string setting_name)
        {
            return _d_table[setting_name];

        }

        public void SetValue(string setting_name,string value)
        {
            if (_d_table.ContainsKey(setting_name))
                _d_table[setting_name] = value;
            else
                _d_table.Add(setting_name, value);
        }
        /// <summary>
        /// Gets the folder  to the main media folder for music.
        /// </summary>
        public DirectoryInfo MainMusicFolderPath { 
            get
            {
                return new DirectoryInfo(GetValue("MainMusicFolderPath").ToString());;
            }
            set
            {
                SetValue("MainMusicFolderPath",value.FullName);
            }
        }

        
        /// <summary>
        /// Gets the folder path to the main media folder for video
        /// </summary>
        public DirectoryInfo MainVideoFolderPath 
        {
            get
            {
                return new DirectoryInfo(GetValue("MainVideoFolderPath").ToString());
            }
            set
            {
                SetValue("MainVideoFolderPath", value.FullName);
            }
 
        }

        /// <summary>
        /// Gets the file path for the SQLite file that store the main library information.
        /// </summary>
        public string LibraryDatabaseFilePath 
        {
            get
            {
                return GetValue("LibraryDatabaseFilePath").ToString();
            }
            set
            {
                SetValue("LibraryDatabaseFilePath", value);
            }
        }

    }
}
