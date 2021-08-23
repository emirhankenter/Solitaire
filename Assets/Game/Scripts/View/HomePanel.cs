using System;
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

#if UNITY_EDITOR
        [NonSerialized, ShowInInspector, OnValueChanged(nameof(OnRectTransformDimensionsChange))] private ScreenOrientation _screenOrientation = ScreenOrientation.Unknown;
#endif

        private HomePanelParams _params;
        
        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as HomePanelParams;
            if(_params == null) return;
            
            OnRectTransformDimensionsChange();
            
            base.Open(viewParams);
        }

        public override void Close()
        {
            base.Close();
        }

        private void OnRectTransformDimensionsChange()
        {
            var orientation = Screen.orientation;

            if (orientation != ScreenOrientation.Portrait || orientation != ScreenOrientation.LandscapeLeft)
                orientation = ScreenOrientation.Portrait;

#if UNITY_EDITOR
            orientation = _screenOrientation;
#endif
            
            _seedsPortraitVariation.gameObject.SetActive(orientation == ScreenOrientation.Portrait);
            _seedsLandscapeVariation.gameObject.SetActive(orientation == ScreenOrientation.LandscapeLeft);

            _title.anchoredPosition = orientation == ScreenOrientation.Portrait ? new Vector2(0, -60.79f) : new Vector2(0, -10f);
            _cards.anchoredPosition = orientation == ScreenOrientation.Portrait ? new Vector2(0, -120.58f) : new Vector2(0, -37.5f);
            _newMatchButton.anchoredPosition = orientation == ScreenOrientation.Portrait ? new Vector2(0, -70.1f) : new Vector2(0, -20.9f);
            _continueButton.anchoredPosition = orientation == ScreenOrientation.Portrait ? new Vector2(0, -126.3f) : new Vector2(0, -46.9f);

            var scale = orientation == ScreenOrientation.Portrait ? Vector3.one : Vector3.one * 0.6f;
            _title.localScale = scale;
            _cards.localScale = scale;
            _newMatchButton.localScale = scale;
            _continueButton.localScale = scale;
        }

        public void OnNewMatchButtonClicked()
        {
            _params?.NewMatchButtonClicked?.Invoke();
        }

        public void OnContinueButtonClicked() // todo: interactivity of button when there is no match
        {
            _params?.ContinueButtonClicked?.Invoke();
        }
    }
}
