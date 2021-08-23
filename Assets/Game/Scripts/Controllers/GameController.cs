using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Game.Scripts.Models.ViewParams;
using Mek.Controllers;
using Mek.Localization;
using Mek.Models;
using Mek.Navigation;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : SingletonBehaviour<GameController>
    {
        public bool IsPaused { get; private set; }
        [SerializeField] private BoardController _boardController;
        [SerializeField] private PlayerController _playerController;
        protected override void OnAwake()
        {
            DOTween.SetTweensCapacity(400, 50);
            LocalizationManager.Init();
            PrefsManager.Initialize();
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Mek GM Ready");
            });

            _playerController.CanPlay = false;
            _boardController.ReadyToPlay += OnBoardReadyToPlay;
            _boardController.Init();
            Navigation.Panel.Change(new InGamePanelParams(TogglePause));
        }

        private void OnBoardReadyToPlay()
        {
            _playerController.CanPlay = true;
        }

        private void TogglePause(bool state)
        {
            IsPaused = state;
            Time.timeScale = IsPaused ? 0f : 1f;
            _playerController.CanPlay = !state;
        }
    }
}
