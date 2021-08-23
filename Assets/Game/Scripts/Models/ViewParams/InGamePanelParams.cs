using System;
using Mek.Navigation;

namespace Game.Scripts.Models.ViewParams
{
    public class InGamePanelParams : Mek.Navigation.ViewParams
    {
        public Action<bool> TogglePause;
        
        public InGamePanelParams(Action<bool> togglePause) : base(ViewTypes.InGamePanel)
        {
            TogglePause = togglePause;
        }
    }
}
