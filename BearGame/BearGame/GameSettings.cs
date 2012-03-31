﻿//                string path = "Global_Settings.xml";
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
        
        public int Map_MaxWidth =64;
        public int Map_MaxHeight= 64;

        //People Stuff
        public int Person_HealthMin=1;
        public int Person_HealthMax=7;
        public int Person_HealthDefault=3;

        public int Person_HealthPerHug=1;
        public int Person_HealthRegen=0;
        public int Person_HoneyWantedMin=1;
        public int Person_HoneyWantedMax=7;

        public int Person_LoveMin=0;
        public int Person_LoveMax=7;
        public int Person_Love = 3;
        public int Person_Speed = 1;
        

        public int Person_TricycleLove = 1;
        
        // Bear Stuff
        public float Bear_HealthMax = 100;
        public float Bear_HealthMin = 0;
        public float Bear_HealthPerHoney = 5;
        public float Bear_HealthDefault = 100;
        public float Bear_HealthDecreaseRate = 1; // healths / second

        public double Bear_MoveInterval = 1.0/3; // Seconds
        
        public GameSetting()
        {
        }
    }
}