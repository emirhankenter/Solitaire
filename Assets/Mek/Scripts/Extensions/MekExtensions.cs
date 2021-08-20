﻿using System;
using System.Collections;
using Mek.Utilities;
using Mek.Audio;
using Mek.Coroutines;
using UnityEngine;

namespace Mek.Extensions
{
    public static class MekExtensions
    {
        #region Coroutine

        public static void StartCoroutine(this IEnumerator coroutine, string key)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentNullException("Null routine");
            }

            CoroutineController.StartCoroutine(key, coroutine);
        }
        public static void StartCoroutine(this IEnumerator coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentNullException("Null routine");
            }

            CoroutineController.StartCoroutine(coroutine);
        }

        public static void OnComplete(this IEnumerator routine, Action onComplete)
        {
            CoroutineController.RoutineFinished += OnRoutineFinished;
            void OnRoutineFinished(string obj)
            {
                CoroutineController.RoutineFinished -= OnRoutineFinished;
                onComplete?.Invoke();
            }
        }

        public static void OnRoutineWithKeyCompleted(this string key, Action onComplete)
        {
            CoroutineController.RoutineFinished += OnRoutineFinished;
            void OnRoutineFinished(string obj)
            {
                if (obj != key) return;
                CoroutineController.RoutineFinished -= OnRoutineFinished;
                onComplete?.Invoke();
            }
        }

        public static string GenerateUniqueString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=","");
            GuidString = GuidString.Replace("+","");

            return GuidString;
        }

        #endregion

        #region ObjectPooling

        public static T Spawn<T>(this T prefab) where T : Component
        {
            return ObjectPooling.Instance.Spawn(prefab);
        }

        public static GameObject Spawn(this GameObject obj)
        {
            return ObjectPooling.Instance.Spawn(obj);
        }

        public static ParticleSystem Spawn(this ParticleSystem target)
        {
            return ParticlePooling.Instance.Spawn(target);
        }

        public static ParticleSystem Spawn(this ParticleSystem target, Transform t)
        {
            return ParticlePooling.Instance.Spawn(target, t);
        }

        public static ParticleSystem Spawn(this ParticleSystem target, Vector3 position, Quaternion rotation, bool keepPrefabsInitialRotation = false)
        {
            return ParticlePooling.Instance.Spawn(target, position, rotation, keepPrefabsInitialRotation);
        }

        public static void Recycle<T>(this T component) where T : Component
        {
            ObjectPooling.Instance.Recycle<T>(component);
        }
        
        public static void Recycle(this GameObject obj)
        {
            ObjectPooling.Instance.Recycle(obj);
        }
        
        public static void Recycle(this ParticleSystem target)
        {
            ParticlePooling.Instance.Recycle(target);
        }

        #endregion

        #region Audio

        public static void Play(this AudioClip clip, float volume = 1f, Vector3 position = default, bool in3DSpace = false, float minDistance = 0f, float maxDistance = 100f)
        {
            AudioController.Play(clip, volume, position, in3DSpace, minDistance, maxDistance);
        }

        public static bool IsPlaying(this AudioClip clip)
        {
            return AudioController.IsPlaying(clip);
        }

        #endregion
    }
}