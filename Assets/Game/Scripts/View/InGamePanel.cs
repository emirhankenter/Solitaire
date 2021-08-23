using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Models.ViewParams;
using Mek.Helpers;
using Mek.Localization;
using Mek.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class InGamePanel : Panel
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private GameObject _pausedContent;
        
        private InGamePanelParams _params;
        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);
            _params = viewParams as InGamePanelParams;
            if (_params == null) return;
            
            _pauseButton.gameObject.SetActive(true);
            _pausedContent.gameObject.SetActive(false);
        }

        public override void Close()
        {
            base.Close();
        }

        public void OnPauseButtonClicked()
        {
            TogglePause(true);
        }

        public void OnContinueButtonClicked()
        {
            TogglePause(false);
        }

        private void TogglePause(bool state)
        {
            _params?.TogglePause?.Invoke(state);
            _pauseButton.gameObject.SetActive(!state);
            _pausedContent.gameObject.SetActive(state);
        }
    }
}