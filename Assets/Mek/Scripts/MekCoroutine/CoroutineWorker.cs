using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mek.Coroutines
{
    public class CoroutineWorker : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private List<string> RunningRoutines => CoroutineController.RoutineKeys;
        
        private static CoroutineWorker _instance;
        public static CoroutineWorker Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("CoroutineWorker");
                    _instance = go.AddComponent<CoroutineWorker>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }
    }
}