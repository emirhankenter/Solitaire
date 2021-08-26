using System;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Coroutines;
using Mek.Localization;
using Mek.Models;
using Mek.Navigation;
using UnityEngine;
using UnityEngine.UI;
using Navigation = Mek.Navigation.Navigation;

namespace Game.Scripts.View
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private RectTransform _languagesParent;
        [SerializeField] private LanguageSelectionButton _languageSelectionButtonPrefab;
        [SerializeField] private Image _volumeOn;
        [SerializeField] private Image _volumeOff;

        private List<LanguageSelectionButton> _selectionButtons = new List<LanguageSelectionButton>();

        private bool _isClosing;
        public override void Open(ViewParams viewParams)
        {
            InitializeElements();
            _isClosing = false;

            base.Open(viewParams);
        }

        public override void Close()
        {
            if (!_isClosing)
            {
                _isClosing = true;
            }
            else
            {
                return;
            }
            
            CoroutineController.DoAfterGivenTime(0.5f, () =>
            {
                Navigation.Popup.ToggleBlocker(false);
            });

            base.Close();
        }

        private void InitializeElements()
        {
            if (_languagesParent.childCount == 0)
            {
                foreach (var language in GameController.Instance.Languages)
                {
                    var button = Instantiate(_languageSelectionButtonPrefab, _languagesParent);
                    button.Init(language);
                    button.Clicked += OnLanguageButtonClicked;
                    _selectionButtons.Add(button);
                }
            }
            UpdateVolumeImages();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            foreach (var button in _selectionButtons)
            {
                button.Clicked -= OnLanguageButtonClicked;
                Destroy(button.gameObject);
            }
        }

        private void OnLanguageButtonClicked(SystemLanguage lang)
        {
            _selectionButtons.ForEach(button => button.Check());
        }

        public void OnClickedCloseButton()
        {
            Close();
        }

        public void ToggleVolume()
        {
            MekPlayerData.SoundFXEnabled = !MekPlayerData.SoundFXEnabled;

            UpdateVolumeImages();
        }

        private void UpdateVolumeImages()
        {
            _volumeOn.enabled = MekPlayerData.SoundFXEnabled;
            _volumeOff.enabled = !MekPlayerData.SoundFXEnabled;
        }
    }
}
