using Mek.Models.Stats;
using System;
using System.Collections.Generic;
using Mek.Models;
using UnityEngine;

namespace Game.Scripts.Models
{
    public class PlayerData : MekPlayerData
    {
        public static readonly Dictionary<string, BaseStat> Prefs = new Dictionary<string, BaseStat> {

            // MekPlayerData
            { PrefStats.IsStatSet, new BoolStat() },
            { PrefStats.MusicOpened, new BoolStat() },

            // Game Specific PLayerData
            { PrefStats.PlayerLevel, new IntStat(0, Int32.MaxValue, 1) },
            { PrefStats.LastActive, new DateStat(DateTime.UtcNow) },
            // { PrefStats.MyTest, new ObjectStat<TestStruct>(JsonUtility.ToJson(new TestStruct(){Number = 100}))}
        };

        public DateTime LastActive
        {
            get => PrefsManager.GetDate(PrefStats.LastActive);
            set => PrefsManager.SetDate(PrefStats.LastActive, value);
        }

        public int PlayerLevel
        {
            get => PrefsManager.GetInt(PrefStats.PlayerLevel);
            set => PrefsManager.SetInt(PrefStats.PlayerLevel, value);
        }

        // public TestStruct MyTest
        // {
        //     get => PrefsManager.GetObject<TestStruct>(PrefStats.MyTest);
        //     set => PrefsManager.SetObject(PrefStats.MyTest, value);
        // }

        #region Singleton

        private static PlayerData _instance = new PlayerData();

        public static PlayerData Instance => _instance;

        #endregion
    }
}