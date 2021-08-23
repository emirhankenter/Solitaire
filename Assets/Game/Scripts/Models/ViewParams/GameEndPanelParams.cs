using System;
using Mek.Navigation;

namespace Game.Scripts.Models.ViewParams
{
    public class GameEndPanelParams : Mek.Navigation.ViewParams
    {
        public Action Restart;
        
        public GameEndPanelParams(Action restart) : base(ViewTypes.GameEndPanel)
        {
            Restart = restart;
        }
    }
}
