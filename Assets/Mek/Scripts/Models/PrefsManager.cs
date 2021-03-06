using System;
using Game.Scripts.Models;
using UnityEngine;

namespace Mek.Models
{
    public static class PrefsManager
    {
        #region Config

        public static void Initialize()
        {
            if (!MekPlayerData.IsStatSet)
            {
                InitializePlayerPrefs();
            }
            else
            {
                InitializeUnsetPlayerPrefs();
            }
        }
        

        private static void InitializePlayerPrefs()
        {
            foreach (var stat in PlayerData.Prefs)
            {
                var type = stat.Value.GetStatType();

                SetByType(stat.Key, type);
            }

            MekPlayerData.IsStatSet = true;
            Debug.LogWarning("Prefs has Initialized.");
        }

        private static void InitializeUnsetPlayerPrefs()
        {
            foreach (var stat in PlayerData.Prefs)
            {
                if (!HasKey(stat.Key))
                {
                    var type = stat.Value.GetStatType();

                    SetByType(stat.Key, type);
                }
            }
        }

        #endregion

        public static void SetByType(string key, Type type)
        {
            if (type == typeof(int))
            {
                var value = PlayerData.Prefs[key].Get<int>();

                SetInt(key, value);
            }
            else if (type == typeof(float))
            {
                var value = PlayerData.Prefs[key].Get<float>();

                SetFloat(key, value);
            }
            else if (type == typeof(long))
            {
                var value = PlayerData.Prefs[key].Get<long>();

                SetLong(key, value);
            }
            else if (type == typeof(bool))
            {
                var value = PlayerData.Prefs[key].Get<bool>();

                SetBool(key, value);
            }
            else if (type == typeof(string))
            {
                var value = PlayerData.Prefs[key].Get<string>();

                SetString(key, value);
            }
            else if (type == typeof(Vector2))
            {
                var value = PlayerData.Prefs[key].Get<Vector2>();

                SetVector2(key, value);
            }
            else if (type == typeof(Vector3))
            {
                var value = PlayerData.Prefs[key].Get<Vector3>();

                SetVector3(key, value);
            }
            else if (type == typeof(DateTime))
            {
                var value = PlayerData.Prefs[key].Get<DateTime>();

                SetDate(key, value);
            }
            else
            {
                var value = PlayerData.Prefs[key].Get<string>();

                SetString(key, value);
            }
        }

        private static bool SetData<T>(string key, T value)
        {
            if (PlayerData.Prefs[key] != null)
            {
                PlayerData.Prefs[key].Set(value);
                return true;
            }
            return false;
        }

        private static T GetData<T>(string key)
        {
            return PlayerData.Prefs[key].Get<T>();
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        #region Int
        public static void SetInt(string prefKey, int value = 0)
        {
            PlayerPrefs.SetInt(prefKey, value);
            SetData(prefKey, value);
        }

        public static int GetInt(string prefKey)
        {
            return PlayerPrefs.GetInt(prefKey);
        }

        #endregion

        #region Float
        public static void SetFloat(string prefKey, float value)
        {
            PlayerPrefs.SetFloat(prefKey, value);
            SetData(prefKey, value);
        }

        public static float GetFloat(string prefKey)
        {
            return PlayerPrefs.GetFloat(prefKey);
        }

        #endregion

        #region Long
        public static void SetLong(string prefKey, long value)
        {
            PlayerPrefs.SetString(prefKey, value.ToString());
            SetData(prefKey, value);
        }

        public static long GetLong(string prefKey)
        {
            return long.Parse(PlayerPrefs.GetString(prefKey));
        }

        #endregion

        #region Bool
        public static void SetBool(string prefKey, bool value)
        {
            PlayerPrefs.SetInt(prefKey, value ? 1 : 0);
            SetData(prefKey, value);
        }

        public static bool GetBool(string prefKey)
        {
            return PlayerPrefs.GetInt(prefKey) == 1;
        }

        #endregion

        #region String
        public static void SetString(string prefKey, string value)
        {
            PlayerPrefs.SetString(prefKey, value);
            SetData(prefKey, value);
        }

        public static string GetString(string prefKey)
        {
            return PlayerPrefs.GetString(prefKey);
        }

        #endregion

        #region Vector2
        public static void SetVector2(string prefKey, Vector2 value)
        {
            PlayerPrefs.SetFloat($"{prefKey}.X", value.x);
            PlayerPrefs.SetFloat($"{prefKey}.Y", value.y);
            SetData(prefKey, value);
        }

        public static Vector2 GetVector2(string prefKey)
        {
            var value = new Vector2(PlayerPrefs.GetFloat($"{prefKey}.X"), PlayerPrefs.GetFloat($"{prefKey}.Y"));
            return value;
        }

        #endregion

        #region Vector3
        public static void SetVector3(string prefKey, Vector3 value)
        {
            PlayerPrefs.SetFloat($"{prefKey}.X", value.x);
            PlayerPrefs.SetFloat($"{prefKey}.Y", value.y);
            PlayerPrefs.SetFloat($"{prefKey}.Z", value.z);
            SetData(prefKey, value);
        }

        public static Vector3 GetVector3(string prefKey)
        {
            var value = new Vector3(PlayerPrefs.GetFloat($"{prefKey}.X"), PlayerPrefs.GetFloat($"{prefKey}.Y"), PlayerPrefs.GetFloat($"{prefKey}.Z"));
            return value;
        }

        #endregion

        #region Date
        public static void SetDate(string prefKey, DateTime value)
        {
            PlayerPrefs.SetString(prefKey, DateTime.Now.ToBinary().ToString());
            SetData(prefKey, value);
        }

        public static DateTime GetDate(string prefKey)
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString(prefKey));

            return DateTime.FromBinary(temp);
        }

        #endregion

        #region Object

        public static void SetObject<T>(string prefKey, T value)
        {
            var json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(prefKey, json);
            SetData(prefKey, json);
        }

        public static T GetObject<T>(string prefKey)
        {
            var objFromJson = JsonUtility.FromJson<T>(PlayerPrefs.GetString(prefKey));
            return objFromJson;
        }

        #endregion
    }
}