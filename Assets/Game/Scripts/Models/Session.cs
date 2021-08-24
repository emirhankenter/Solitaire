using System;
using System.Collections;
using Game.Scripts.Controllers;
using Mek.Coroutines;
using Mek.Extensions;
using UnityEngine;

namespace Game.Scripts.Models
{
    public class Session
    {
        public static Session New => new Session();

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }
        
        public TimeSpan TimeSpan { get; private set; }

        public void Init()
        {
            TimeSpan = new TimeSpan();

            BoardController.Instance.ScoreMade += OnScoreMade;
        }

        public void Dispose()
        {
            BoardController.Instance.ScoreMade -= OnScoreMade;
            if (CoroutineController.IsCoroutineRunning(TimerRoutine()))
            {
                CoroutineController.StopCoroutine(TimerRoutine());
            }
        }

        public void StartTimer()
        {
            TimerRoutine().StartCoroutine();
        }

        private void OnScoreMade(int scoreDelta)
        {
            Score += scoreDelta;

            Score = Mathf.RoundToInt(Mathf.Max(0, Score));
            
            ScoreChanged?.Invoke(Score);
        }

        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                TimeSpan = TimeSpan.Add(TimeSpan.FromSeconds(1));
            }
        }
    }
}
