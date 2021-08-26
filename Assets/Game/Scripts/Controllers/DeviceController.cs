using System;
using System.Collections;
using Mek.Extensions;
using Mek.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class DeviceController : SingletonBehaviour<DeviceController>
    {
        public static event Action<DeviceOrientation> DeviceOrientationChanged;

        public DeviceOrientation DeviceOrientation { get; private set; } = DeviceOrientation.Portrait;

        private const float _checkInterval = 1f;

        protected override void OnAwake()
        {
#if !UNITY_EDITOR
            OrientationCheckerRoutine().StartCoroutine();
#endif
        }
        
        private IEnumerator OrientationCheckerRoutine(){
            DeviceOrientation = Input.deviceOrientation;
 
            while (true) {
 
                // Check for an Orientation Change
                switch (Input.deviceOrientation) {
                    case DeviceOrientation.Unknown:
                    case DeviceOrientation.FaceUp:
                    case DeviceOrientation.FaceDown:
                        break;
                    default:
                        if (DeviceOrientation != Input.deviceOrientation) {
                            DeviceOrientation = Input.deviceOrientation;
                            DeviceOrientationChanged?.Invoke(DeviceOrientation);
                        }
                        break;
                }
 
                yield return new WaitForSeconds(_checkInterval);
            }
        }
        
#if UNITY_EDITOR
        [Button]
        private void TestOrientation(DeviceOrientation orientation)
        {
            DeviceOrientationChanged?.Invoke(orientation);
        }
#endif
    }
}
