using System;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Models
{
    public class Session
    {
        public static Session New => new Session();

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }
        
        public Session()
        {
            
        }

        public void Init()
        {
            BoardController.Instance.ScoreMade += OnScoreMade;
        }

        public void Dispose()
        {
            BoardController.Instance.ScoreMade -= OnScoreMade;
        }

        private void OnScoreMade(int scoreDelta)
        {
            Score += scoreDelta;

            Score = Mathf.RoundToInt(Mathf.Max(0, Score));
            
            ScoreChanged?.Invoke(Score);
        }
    }
}
