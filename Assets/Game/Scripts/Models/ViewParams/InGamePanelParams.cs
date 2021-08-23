using System;
using Mek.Navigation;

namespace Game.Scripts.Models.ViewParams
{
    public class InGamePanelParams : Mek.Navigation.ViewParams
    {
        public Action<bool> TogglePause;
        public Action Undo;
        public Action Restart;
        
        public InGamePanelParams(Action<bool> togglePause, Action undo, Action restart) : base(ViewTypes.InGamePanel)
        {
            TogglePause = togglePause;
            Undo = undo;
            Restart = restart;
        }
    }
}
