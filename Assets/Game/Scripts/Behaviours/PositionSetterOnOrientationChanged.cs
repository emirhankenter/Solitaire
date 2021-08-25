using System;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class PositionSetterOnOrientationChanged : MonoBehaviour
    {
        [SerializeField] private Space _simulationSpace = Space.Self;
        [SerializeField] private Vector3 _portraitPosition = Vector3.zero;
        [SerializeField] private Vector3 _landscapePosition = Vector3.zero;
        
        private DeviceOrientation _deviceOrientation;
        
        private void Start()
        {
            _deviceOrientation = DeviceController.Instance.DeviceOrientation;

            DeviceController.DeviceOrientationChanged += OnDeviceOrientationChanged;
            
            SetPosition();
        }

        private void OnDestroy()
        {
            DeviceController.DeviceOrientationChanged -= OnDeviceOrientationChanged;
        }

        private void OnDeviceOrientationChanged(DeviceOrientation deviceOrientation)
        {
            _deviceOrientation = deviceOrientation;
            
            SetPosition();
        }

        private void SetPosition()
        {
            switch (_simulationSpace)
            {
                case Space.World:
                    transform.position = _deviceOrientation == DeviceOrientation.Portrait ? _portraitPosition : _landscapePosition;
                    break;
                case Space.Self:
                    transform.localPosition = _deviceOrientation == DeviceOrientation.Portrait ? _portraitPosition : _landscapePosition;
                    break;
            }
        }
    }
}
