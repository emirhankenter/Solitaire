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
            { PrefStats.Score, new IntStat(0, Int32.MaxValue, 0) },
            { PrefStats.Draw3Enabled, new BoolStat() },
            { PrefStats.Language, new IntStat(0, Int32.MaxValue, (int)SystemLanguage.English) },
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

        public int Score
        {
            get => PrefsManager.GetInt(PrefStats.Score);
            set => PrefsManager.SetInt(PrefStats.Score, value);
        }

        public bool Draw3Enabled
        {
            get => PrefsManager.GetBool(PrefStats.Draw3Enabled);
            set => PrefsManager.SetBool(PrefStats.Draw3Enabled, value);
        }

        public int Language
        {
            get => PrefsManager.GetInt(PrefStats.Language);
            set => PrefsManager.SetInt(PrefStats.Language, value);
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