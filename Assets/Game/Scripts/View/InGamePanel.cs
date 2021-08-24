using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Behaviours.Piles;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Game.Scripts.Models.ViewParams;
using Mek.Helpers;
using Mek.Localization;
using Mek.Models;
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
        [SerializeField] private Text _scoreText;
        
        private InGamePanelParams _params;
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as InGamePanelParams;
            if (_params == null) return;
            
            _pauseButton.gameObject.SetActive(true);
            _pausedContent.gameObject.SetActive(false);
            
            UpdateUndoButton();
            OnScoreChanged(0);
            
            HistoryController.Instance.HistoryChanged += OnHistoryChanged;
            GameController.Instance.CurrentSession.ScoreChanged += OnScoreChanged;
            
            base.Open(viewParams);
        }

        public override void Close()
        {
            HistoryController.Instance.HistoryChanged -= OnHistoryChanged;
            GameController.Instance.CurrentSession.ScoreChanged -= OnScoreChanged;
            base.Close();
        }

        private void OnHistoryChanged()
        {
            UpdateUndoButton();
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = score.ToString();
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
            _undoButton.gameObject.SetActive(!state);
            _pausedContent.gameObject.SetActive(state);
        }

        public void OnUndoButtonClicked()
        {
            _params?.Undo?.Invoke();
        }

        public void OnRestartButtonClicked()
        {
            _params?.Restart?.Invoke();
            OnScoreChanged(0);
        }
    }
}