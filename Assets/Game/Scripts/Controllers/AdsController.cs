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
        
        private const string _androidAdUnitId = "Banner_Android";
        private const string _iOsAdUnitId = "Banner_iOS";
        
        private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

        private string _gameId;

        public bool IsInitialized { get; private set; }
        
        protected override void OnAwake()
        {
        }
        public void InitializeAds()
        {
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
        private bool _bannerShowRequested;

        public void LoadBanner()
        {
            if (!IsInitialized) return;
            if (!Advertisement.IsReady()) return;
            
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
            if (!Advertisement.IsReady()) return;
            
            if (!Advertisement.Banner.isLoaded)
            {
                _bannerShowRequested = true;
                LoadBanner();
                return;
            }
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
            if (!Advertisement.IsReady()) return;
            
            Advertisement.Banner.Hide();
        }

        void OnBannerLoaded()
        {
            Debug.Log("Banner loaded");
            if (_bannerShowRequested)
            {
                ShowBannerAd();
                _bannerShowRequested = false;
            }
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
