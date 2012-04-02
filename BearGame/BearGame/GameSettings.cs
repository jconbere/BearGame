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
        public enum VillagerNames
        {
            Luke,
            Frank,
            John,
            Avery,
            Dave,
            Tania
        }



        // Map Stuff
        public int Map_MaxWidth =64;
        public int Map_MaxHeight= 64;

        //People Stuff
        public int Person_HealthMin = 0;
        public int Person_HealthMax = 3;
        public int Person_HealthDefault = 3;

        public int Person_HealthLossDuringHug = 1;
        public int Person_LoveLossDuringHug = -2;

        public int Person_HealthRegen = 0;

        public int Person_LoveIncreaseForHoney = 2;
        public int Person_HealthIncreaseForHoney = 2;

        public int Person_LoveMin = 0;
        public int Person_LoveMax = 7;
        public int Person_InitialLove = 3;
        public float Person_Speed = 1.0f; //How many seconds before moving
        public float Person_SpeedStep = 0.2f; //Decreases this many seconds before moving for each love from initial
        public int Person_ActivityThreshold = 2; // # of tiles person will run away/to bear
        public int Person_BaseApproachDistance = 4; //How Close a Person will get to the bear initially

        public int Person_HealthDecreaseForRunOver = 2;

        public int Person_TricycleLove = 1;
        public int Person_MaxTricycleLove = 3;

        public int Person_MaxHoneyLove = 3;
        
        // Bear Stuff
        public float Bear_HealthGainDuringHug = 30; // helath per second
        public float Bear_HealthMax = 100;
        public float Bear_HealthMin = 0;
        public float Bear_HealthPerHoney = 15;
        public float Bear_HealthDefault = 100;
        public float Bear_HealthDecreaseRate = 3; // healths / second

        public float Bear_HealthGainForHoney = 15;

        public double Bear_MoveInterval = 1.0/3; // Seconds

        public double Bear_RidingMoveInterval = 1.0 / 2; // Seconds
        public double Bear_RidingImpressPeopleDistance = 3; // tile distance
        public double Bear_RidingImpressInterval = 4; // Seconds
        public int Bear_RidingImpressLoveIncrease = 1; // Loves

        public double Bear_InteractionInterval = 1.0/3; // Seconds
    }
}
