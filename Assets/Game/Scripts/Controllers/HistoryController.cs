using System.Collections.Generic;
using Game.Scripts.Models;
using Mek.Utilities;

namespace Game.Scripts.Controllers
{
    public class HistoryController : SingletonBehaviour<HistoryController>
    {
        public Stack<MovementData> Movements { get; } = new Stack<MovementData>();
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
        }

        public bool TryUndo(out MovementData movementData)
        {
            movementData = null;

            if (Movements.Count == 0) return false;

            movementData = Movements.Pop();
            
            return true;
        }
    }
}
