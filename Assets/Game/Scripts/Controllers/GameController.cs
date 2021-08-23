using DG.Tweening;
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
            
            _boardController.ReadyToPlay += OnBoardReadyToPlay;
            _boardController.Completed += OnBoardCompleted;
            _boardController.Init();

            Navigation.Panel.Change(new HomePanelParams(StartGame, StartGame)); //todo: implement new Match, continue
        }

        private void StartGame()
        {
            _playerController.CanPlay = false;
            Navigation.Panel.Change(new InGamePanelParams(TogglePause, UndoMovement, RestartGame));
        }

        private void OnBoardReadyToPlay()
        {
            _playerController.CanPlay = true;
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
            _boardController.ResetBoard();
            
            StartGame();
        }
    }
}
