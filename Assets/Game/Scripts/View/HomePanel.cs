using System;
using Game.Scripts.Controllers;
using Game.Scripts.Models.ViewParams;
using Mek.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.View
{
    public class HomePanel : Panel
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _cards;
        [SerializeField] private RectTransform _seedsPortraitVariation;
        [SerializeField] private RectTransform _seedsLandscapeVariation;
        [SerializeField] private RectTransform _newMatchButton;
        [SerializeField] private RectTransform _continueButton;

        private HomePanelParams _params;
        
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as HomePanelParams;
            if(_params == null) return;
            
            // OnRectTransformDimensionsChange();

            DeviceController.DeviceOrientationChanged += OnDeviceOrientationChanged;
            
            base.Open(viewParams);
        }

        public override void Close()
        {
            DeviceController.DeviceOrientationChanged -= OnDeviceOrientationChanged;
            base.Close();
        }

        private void OnDeviceOrientationChanged(DeviceOrientation orientation)
        {

            if (orientation != DeviceOrientation.Portrait || orientation != DeviceOrientation.LandscapeLeft)
                orientation = DeviceOrientation.Portrait;
            
            _seedsPortraitVariation.gameObject.SetActive(orientation == DeviceOrientation.Portrait);
            _seedsLandscapeVariation.gameObject.SetActive(orientation == DeviceOrientation.LandscapeLeft);

            // _title.anchoredPosition = orientation == ScreenOrientation.Portrait ? new Vector2(0, -60.79f) : new Vector2(0, -60.79f);
            _cards.anchoredPosition = orientation == DeviceOrientation.Portrait ? new Vector2(0, -419.5f) : new Vector2(0, -521.86f);
            // _newMatchButton.anchoredPosition = orientation == ScreenOrientation.Portrait ? new Vector2(0, -267f) : new Vector2(0, -267f);
            _continueButton.anchoredPosition = orientation == DeviceOrientation.Portrait ? new Vector2(0, -433.3f) : new Vector2(0, -519.7f);

            var scale = orientation == DeviceOrientation.Portrait ? Vector3.one : Vector3.one * 2f;
            _title.localScale = scale;
            _cards.localScale = scale;
            _newMatchButton.localScale = scale;
            _continueButton.localScale = scale;
        }

        private void OnRectTransformDimensionsChange()
        {
        }

        public void OnNewMatchButtonClicked()
        {
            Navigation.Popup.ToggleBlocker(true);
            Navigation.Popup.Open(new GameSelectionPopupParams(() =>
            {
                Navigation.Popup.ToggleBlocker(false);
                _params?.NewMatchButtonClicked?.Invoke();
            }));
        }

        public void OnContinueButtonClicked() // todo: interactivity of button when there is no match
        {
            _params?.ContinueButtonClicked?.Invoke();
        }
    }
}
