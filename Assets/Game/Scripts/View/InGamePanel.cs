using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
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
        [SerializeField] private Button _undoButton;
        [SerializeField] private GameObject _pausedContent;
        
        private InGamePanelParams _params;
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as InGamePanelParams;
            if (_params == null) return;
            
            _pauseButton.gameObject.SetActive(true);
            _pausedContent.gameObject.SetActive(false);
            
            UpdateUndoButton();

            HistoryController.Instance.HistoryChanged += OnHistoryChanged;
            
            base.Open(viewParams);
        }

        public override void Close()
        {
            HistoryController.Instance.HistoryChanged -= OnHistoryChanged;
            base.Close();
        }

        private void OnHistoryChanged()
        {
            UpdateUndoButton();
        }

        private void UpdateUndoButton()
        {
            _undoButton.interactable = !HistoryController.Instance.IsHistoryEmpty;
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

        public void OnUndoButtonClicked()
        {
            _params?.Undo?.Invoke();
        }

        public void OnRestartButtonClicked()
        {
            _params?.Restart?.Invoke();
        }
    }
}