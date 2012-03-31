using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

            //GameSetting settings = new GameSetting();

            //if (SettingsManager.Write(settings)) 
            //{
            //    //write success
            //};

            //settings = SettingsManager.Read("Game.xml");

namespace BearGame
{
    public class SettingsManager
    {   
        public static bool Write(GameSetting settings)
        {
            try
            {
                XmlSerializer x = new XmlSerializer(settings.GetType());
                StreamWriter writer = new StreamWriter("Game.xml");
                x.Serialize(writer, settings);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static GameSetting Read(string path)
        {
            GameSetting settings = new GameSetting();
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(GameSetting));
                StreamReader reader = new StreamReader(path);
                settings = (GameSetting)x.Deserialize(reader);
                return settings;
            }
            catch
            {
                return settings;
            }
        }
    }
}
