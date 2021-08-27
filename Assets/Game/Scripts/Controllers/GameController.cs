using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Models;
using Game.Scripts.Models.ViewParams;
using Mek.Controllers;
using Mek.Coroutines;
using Mek.Localization;
using Mek.Models;
using Mek.Navigation;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : SingletonBehaviour<GameController>
    {
        public List<SystemLanguage> Languages = new List<SystemLanguage>() {SystemLanguage.Turkish, SystemLanguage.English, SystemLanguage.Italian};
        
        [SerializeField] private BoardController _boardController;
        [SerializeField] private PlayerController _playerController;
        public bool IsPaused { get; private set; }
        
        public Session CurrentSession { get; private set; }
        
        protected override void OnAwake()
        {
            DOTween.SetTweensCapacity(400, 50);
            LocalizationManager.Init((SystemLanguage)PlayerData.Instance.Language);
            PrefsManager.Initialize();
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Mek GM Ready");
            });
            
            _boardController.ReadyToPlay += OnBoardReadyToPlay;
            _boardController.Completed += OnBoardCompleted;
            _boardController.Init();
            
            CoroutineController.DoAfterFixedUpdate(() =>
            {
                Navigation.Panel.Change(new HomePanelParams(StartGame, StartGame));
            });
            
            CoroutineController.DoAfterCondition(() => AdsController.Instance.IsInitialized, () =>
            {
                AdsController.Instance.LoadBanner();
            });
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void StartGame()
        {
            CurrentSession?.Dispose();
            
            CurrentSession = Session.New;
            CurrentSession?.Init();
            _boardController.DealCards();
            
            
            Navigation.Panel.Change(new InGamePanelParams(TogglePause, UndoMovement, RestartGame, BackToHomeScreen));
            
            AdsController.Instance.ShowBannerAd();
        }

        private void Dispose()
        {
            CurrentSession?.Dispose();
            _boardController.ReadyToPlay -= OnBoardReadyToPlay;
            _boardController.Completed -= OnBoardCompleted;
        }

        private void OnBoardReadyToPlay()
        {
            CurrentSession.StartTimer();
        }

        private void OnBoardCompleted()
        {
            Navigation.Panel.Change(new GameEndPanelParams(() =>
            {
                ResetGame();
                StartGame();
            }));
        }

        private void TogglePause(bool state)
        {
            IsPaused = state;
            Time.timeScale = IsPaused ? 0f : 1f;
        }

        private void UndoMovement()
        {
            _boardController.Undo();
        }

        private void RestartGame()
        {
            ResetGame();
            StartGame();
        }

        private void ResetGame()
        {
            TogglePause(false);
            HistoryController.Instance.ClearHistory();
            CurrentSession?.Dispose();
            _boardController.ResetBoard();
        }

        private void BackToHomeScreen()
        {
            ResetGame();
            Navigation.Panel.Change(new HomePanelParams(StartGame, StartGame));
            
            AdsController.Instance.HideBannerAd();
        }
    }
}
