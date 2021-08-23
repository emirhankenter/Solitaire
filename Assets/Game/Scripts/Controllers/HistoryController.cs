using System;
using System.Collections.Generic;
using Game.Scripts.Models;
using Mek.Utilities;

namespace Game.Scripts.Controllers
{
    public class HistoryController : SingletonBehaviour<HistoryController>
    {
        public event Action HistoryChanged;
        
        public Stack<MovementData> Movements { get; } = new Stack<MovementData>();
        public bool IsHistoryEmpty => Movements.Count == 0;
        
        protected override void OnAwake()
        {
        }

        public void ClearHistory()
        {
            Movements.Clear();
        }

        public void SaveMovement(MovementData movementData)
        {
            Movements.Push(movementData);
            HistoryChanged?.Invoke();
        }

        public bool TryUndo(out MovementData movementData)
        {
            movementData = null;

            if (Movements.Count == 0) return false;

            movementData = Movements.Pop();
            HistoryChanged?.Invoke();
            
            return true;
        }
    }
}
