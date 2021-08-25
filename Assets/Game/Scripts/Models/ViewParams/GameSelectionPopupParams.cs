using System;
using Mek.Navigation;
using UnityEngine;

namespace Game.Scripts.Models.ViewParams
{
    public class GameSelectionPopupParams : Mek.Navigation.ViewParams
    {
        public Action OnStartButtonClicked;
        
        public GameSelectionPopupParams(Action onStartButtonClicked) : base(ViewTypes.GameSelectionPopup)
        {
            OnStartButtonClicked = onStartButtonClicked;
        }
    }
}
