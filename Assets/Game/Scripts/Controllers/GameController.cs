using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Behaviours;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Mek.Controllers;
using Mek.Localization;
using Mek.Models;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : SingletonBehaviour<GameController>
    {
        [SerializeField] private BoardController _boardController;
        protected override void OnAwake()
        {
            LocalizationManager.Init(SystemLanguage.Turkish);
            PrefsManager.Initialize();
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Ready");
            });
            
            _boardController.PopulateCards();
        }
    }
}
