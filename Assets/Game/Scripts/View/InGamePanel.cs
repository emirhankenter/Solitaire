using System.Collections;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Game.Scripts.Models.ViewParams;
using Mek.Coroutines;
using Mek.Extensions;
using Mek.Navigation;
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
        [SerializeField] private Text _timerText;

        private Session _currentSession;
        
        private InGamePanelParams _params;
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as InGamePanelParams;
            if (_params == null) return;
            
            _pauseButton.gameObject.SetActive(true);
            _pausedContent.gameObject.SetActive(false);
            
            UpdateUndoButton();
            OnScoreChanged(0);
            TogglePause(false);
            
            HistoryController.Instance.HistoryChanged += OnHistoryChanged;
            GameController.Instance.CurrentSession.ScoreChanged += OnScoreChanged;
            BoardController.Instance.ReadyToPlay += OnReadyToPlay;

            _currentSession = GameController.Instance.CurrentSession;
            SetTimerText();
            
            base.Open(viewParams);
        }

        public override void Close()
        {
            HistoryController.Instance.HistoryChanged -= OnHistoryChanged;
            GameController.Instance.CurrentSession.ScoreChanged -= OnScoreChanged;
            BoardController.Instance.ReadyToPlay -= OnReadyToPlay;

            if (CoroutineController.IsCoroutineRunning(TimerRoutine()))
            {
                CoroutineController.StopCoroutine(TimerRoutine());
            }
            base.Close();
        }

        private void OnReadyToPlay()
        {
            TimerRoutine().StartCoroutine();
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
            SetTimerText();
        }

        public void OnBackToHomeScreenButtonClicked()
        {
            Close();
            _params?.BackToHomeScreenClicked?.Invoke();
        }

        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                SetTimerText();
                yield return new WaitForSeconds(Time.deltaTime);
                
            }
        }

        private void SetTimerText()
        {
            var timeSpan = _currentSession.TimeSpan;

            var format = $@"{(timeSpan.TotalDays >= 1 ? @"dd\:" : "")}{(timeSpan.TotalHours >= 1 ? @"hh\:" : "")}mm\:ss";

            _timerText.text = timeSpan.ToString(format);
        }
    }
}