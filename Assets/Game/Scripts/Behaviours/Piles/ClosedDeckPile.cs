using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Behaviours.Piles
{
    public class ClosedDeckPile : Pile
    {
        protected override void OnCardAdded(Card card)
        {
        }

        protected override void OnCardRemoved(Card card)
        {
        }

        protected override void OnCardsAdded(List<Card> cards)
        {
        }

        protected override void OnCardsRemoved(List<Card> cards)
        {
        }

        protected override void OnUndo()
        {
        }

        public override void ArrangeOrders()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                card.Order = i * 10;
                
                card.transform.DOLocalMove(Vector3.zero, 0.1f)
                    .SetEase(Ease.Linear);
            }
        }

        public override bool ShouldFlip(out List<Card> flippedCards)
        {
            flippedCards = null;
            return false;
        }

        public override bool CanCardBeDraggable(Card card) => false;
        public override bool CanCardsPutHere(List<Card> cards) => false;

        public bool Draw(out List<Card> cards, int count = 1)
        {
            cards = null;
            var cardsCount = _cards.Count;
            if (cardsCount == 0) return false;

            cards = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                if (cardsCount > i)
                {
                    cards.Add(_cards[cardsCount - 1 - i]);
                }
            }
            return true;
        }
    }
}
