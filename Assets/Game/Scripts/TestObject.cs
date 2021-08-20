﻿using System;
using Mek.Interfaces;
using UnityEngine;

namespace Game.Scripts
{
    public class TestObject : MonoBehaviour, IDisposable, IRecyclable
    {
        public void Dispose()
        {
            Debug.Log("Disposed");
        }

        public void OnRecycle()
        {
            Debug.Log("Recycled");
        }
    }
}
