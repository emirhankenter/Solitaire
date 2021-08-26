using Mek.Utilities;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Game.Scripts.Controllers
{
    public class AdsController : SingletonBehaviour<AdsController>, IUnityAdsInitializationListener
    {
        [SerializeField] string _androidGameId;
        [SerializeField] string _iOsGameId;
        [SerializeField] bool _testMode = true;
        [SerializeField] bool _enablePerPlacementMode = true;
        
        private const string _androidBannerAdUnitId = "Banner_Android";
        private const string _iOsBannerAdUnitId = "Banner_iOS";
        
        private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

        private string _gameId;

        public bool IsInitialized { get; private set; }
        
        protected override void OnAwake()
        {
        }
        public void InitializeAds()
        {
            _bannerAdUnitId = Application.platform == RuntimePlatform.IPhonePlayer ? _iOsBannerAdUnitId : _androidBannerAdUnitId;
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode, this);
            Advertisement.Banner.SetPosition(_bannerPosition);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            IsInitialized = true;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        #region Banner

        private string _bannerAdUnitId;

        public void LoadBanner()
        {
            if (!IsInitialized) return;
            if (!Advertisement.isInitialized) return;
            
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            Advertisement.Banner.Load(_bannerAdUnitId, options);
        }

        public void ShowBannerAd()
        {
            if (!IsInitialized) return;
            if (!Advertisement.isInitialized) return;

            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            Advertisement.Banner.Show(_bannerAdUnitId, options);
        }

        public void HideBannerAd()
        {
            if (!IsInitialized) return;
            if (!Advertisement.isInitialized) return;
            
            Advertisement.Banner.Hide();
        }

        void OnBannerLoaded()
        {
            Debug.Log("Banner loaded");
        }

        void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
        }

        void OnBannerClicked() { }
        void OnBannerShown() { }
        void OnBannerHidden() { }

        #endregion
    }
}
