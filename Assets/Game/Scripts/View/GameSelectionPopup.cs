using Game.Scripts.Models;
using Game.Scripts.Models.ViewParams;
using Mek.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class GameSelectionPopup : Popup
    {
        [SerializeField] private Toggle _draw3Toggle;

        private GameSelectionPopupParams _params;
        
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as GameSelectionPopupParams;
            if (_params == null) return;
            
            base.Open(viewParams);

            _draw3Toggle.isOn = PlayerData.Instance.Draw3Enabled;
            
        }

        public override void Close()
        {
            base.Close();
        }

        public void OnToggleValueChanged()
        {
            PlayerData.Instance.Draw3Enabled = _draw3Toggle.isOn;
        }

        public void OnStartButtonClicked()
        {
            _params?.OnStartButtonClicked?.Invoke();
            Close();
        }
    }
}
