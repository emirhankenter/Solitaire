﻿using System;
using DG.Tweening;
using Game.Scripts.Behaviours.Piles;
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
        [SerializeField] private BoardController _boardController;
        [SerializeField] private PlayerController _playerController;
        public bool IsPaused { get; private set; }
        
        public Session CurrentSession { get; private set; }
        
        protected override void OnAwake()
        {
            DOTween.SetTweensCapacity(400, 50);
            LocalizationManager.Init();
            PrefsManager.Initialize();
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Mek GM Ready");
            });
            
            
            CoroutineController.DoAfterFixedUpdate(() =>
            {
                _boardController.ReadyToPlay += OnBoardReadyToPlay;
                _boardController.Completed += OnBoardCompleted;
                _boardController.Init();
            
                CurrentSession?.Dispose();
                CurrentSession = Session.New;

                // Navigation.Panel.Change(new HomePanelParams(StartGame, StartGame)); //todo: implement new Match, continue
                StartGame();
            });
        }

        private void StartGame()
        {
            CurrentSession.Init();
            
            _playerController.CanPlay = false;
            
            Navigation.Panel.Change(new InGamePanelParams(TogglePause, UndoMovement, RestartGame));
        }

        private void Dispose()
        {
            CurrentSession?.Dispose();
            _boardController.ReadyToPlay -= OnBoardReadyToPlay;
            _boardController.Completed -= OnBoardCompleted;
        }

        private void OnBoardReadyToPlay()
        {
            _playerController.CanPlay = true;
            CurrentSession.StartTimer();
        }

        private void OnBoardCompleted()
        {
            Navigation.Panel.Change(new GameEndPanelParams(RestartGame));
        }

        private void TogglePause(bool state)
        {
            IsPaused = state;
            Time.timeScale = IsPaused ? 0f : 1f;
            _playerController.CanPlay = !state;
        }

        private void UndoMovement()
        {
            _boardController.Undo();
        }

        private void RestartGame()
        {
            TogglePause(false);
            HistoryController.Instance.ClearHistory();
            CurrentSession?.Dispose();
            _boardController.ResetBoard();
            
            StartGame();
        }
    }
}
