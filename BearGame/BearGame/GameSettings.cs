//                string path = "Global_Settings.xml";
//                XmlSerializer x = new XmlSerializer(GameSettings.GetType());
//                StreamWriter writer = new StreamWriter(path);
//                x.Serialize(writer, settings);

//                MySettings settings = new MySettings();
//                string path = "Global_Settings.xml";
//                XmlSerializer x = new XmlSerializer(typeof(MySettings));
//                StreamReader reader = new StreamReader(path);
//                settings = (TVSettings)x.Deserialize(reader);


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearGame
{
    public class GameSetting
    {
        // Map Stuff
        public int Map_MaxWidth;
        public int Map_MaxHeight;

        //People Stuff
        public int Person_HealthMin;
        public int Person_HealthMax;
        public int Person_HealthDefault;

        public int Person_HealthPerHug;
        public int Person_HealthRegen;
        public int Person_HoneyWantedMin;
        public int Person_HoneyWantedMax;

        public int Person_LoveMin;
        public int Person_LoveMax;

        // Bear Stuff
        public int Bear_HealthMax;
        public int Bear_HealthMin;
        public int Bear_HealthPerHoney;
        public int Bear_HealthDefault;


        public GameSetting()
        {
        }
    }
}