using Game.Scripts.Models.ViewParams;
using Mek.Navigation;

namespace Game.Scripts.View
{
    public class GameEndPanel : Panel
    {
        private GameEndPanelParams _params;
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as GameEndPanelParams;
            if(_params == null) return;
            
            base.Open(viewParams);
        }

        public override void Close()
        {
            base.Close();
        }

        public void OnRestartButtonClicked()
        {
            _params?.Restart?.Invoke();
        }
    }
}
