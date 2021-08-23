using System;
using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Game.Scripts.Behaviours.Piles;

namespace Game.Scripts.Models
{
    public class MovementData
    {
        public static event Action MovementMade;
        
        public List<Card> SourceCards { get; }
        public Pile FromFile { get; }
        public Pile ToPile { get; }

        public List<Card> FlippedCards { get; }

        public MovementData(List<Card> sourceCards, Pile fromPile, Pile toPile , List<Card> flippedCards = null)
        {
            SourceCards = sourceCards;
            FromFile = fromPile;
            ToPile = toPile;
            FlippedCards = flippedCards;
            MovementMade?.Invoke();
        }

        public void Undo()
        {
            var cards = new List<Card>(SourceCards);

            if (FlippedCards != null)
            {
                foreach (var flippedCard in FlippedCards)
                {
                    flippedCard.Flip(false);
                }
            }
                
            ToPile.Remove(SourceCards);
            ToPile.Undo();
                
            FromFile.Add(cards);
            FromFile.Undo();
        }
    }
}
