using System;
using Mek.Navigation;
using UnityEngine;

namespace Game.Scripts.Models.ViewParams
{
    public class HomePanelParams : Mek.Navigation.ViewParams
    {
        public Action NewMatchButtonClicked;
        public Action ContinueButtonClicked;
        
        public HomePanelParams(Action newMatchButtonClicked, Action continueButtonClicked) : base(ViewTypes.HomePanel)
        {
            NewMatchButtonClicked = newMatchButtonClicked;
            ContinueButtonClicked = continueButtonClicked;
        }
    }
}
