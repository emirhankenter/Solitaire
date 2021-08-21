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
        }
    }
}
