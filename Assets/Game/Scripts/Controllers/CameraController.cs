using System;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class CameraController : SingletonBehaviour<CameraController>
    {
        [SerializeField] private Camera _mainCamera;
        
        public Camera MainCamera => _mainCamera;

        protected override void OnAwake()
        {
            DeviceController.DeviceOrientationChanged += OnDeviceOrientationChanged;
        }

        private void Start()
        {
            OnDeviceOrientationChanged(DeviceController.Instance.DeviceOrientation);
        }

        private void OnDestroy()
        {
            DeviceController.DeviceOrientationChanged -= OnDeviceOrientationChanged;
        }

        private void OnDeviceOrientationChanged(DeviceOrientation deviceOrientation)
        {
            switch (deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                    _mainCamera.orthographicSize = 10;
                    break;
                case DeviceOrientation.LandscapeLeft:
                    _mainCamera.orthographicSize = 5;
                    break;
            }
        }
    }
}
