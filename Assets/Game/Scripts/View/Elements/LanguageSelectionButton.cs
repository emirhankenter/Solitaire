using System;
using Game.Scripts.Models;
using Mek.Localization;
using Mek.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.Elements
{
    [Serializable] public class LanguageFlagDictionary : SerializableDictionary<SystemLanguage, Sprite> { }
    public class LanguageSelectionButton : MonoBehaviour
    {
        public event Action<SystemLanguage> Clicked;

        [SerializeField] private Image _flag;
        [SerializeField] private Image _checkMark;

        [SerializeField] private LanguageFlagDictionary _flagDictionary;

        public bool Checked => _checkMark.enabled;

        private SystemLanguage _language;

        public void Init(SystemLanguage language)
        {
            _language = language;
            if (_flagDictionary.TryGetValue(language, out Sprite sprite))
            {
                _flag.sprite = sprite;
            }

            Check();
        }

        public void Check()
        {
            _checkMark.enabled = (SystemLanguage)PlayerData.Instance.Language == _language;
        }

        public void OnClick()
        {
            PlayerData.Instance.Language = (int) _language;
            LocalizationManager.SetLanguage(_language);
            Clicked?.Invoke(_language);
        }
    }
}
