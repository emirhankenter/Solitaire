using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Behaviours.Piles
{
    public class OpenedDeckPile : Pile
    {
        public const float OpenedDeckPileXOffset = 0.3f;

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
            ArrangeOrders();
        }

        protected override void OnUndo()
        {
            ArrangeOrders();
        }

        public override void ArrangeOrders()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                card.Order = i * 10;

                var dif = _cards.Count - i;

                if (_cards.Count <= 2)
                {
                    dif = Math.Max(2 - i, 3 - i);
                }

                // card.transform.localPosition = Vector3.zero + Vector3.right * (dif > 2 ? 0 : ((2 - dif + 1) * OpenedDeckPileXOffset));

                card.transform.DOLocalMove(Vector3.zero + Vector3.right * (dif > 2 ? 0 : ((2 - dif + 1) * OpenedDeckPileXOffset)), 0.1f)
                    .SetEase(Ease.Linear);
            }
        }

        public override bool ShouldFlip(out List<Card> flippedCards)
        {
            flippedCards = null;
            return false;
        }

        public override bool CanCardBeDraggable(Card card)
        {
            return _cards.IndexOf(card) == _cards.Count - 1;
        }

        public override bool CanCardsPutHere(List<Card> cards) => false;
        
        public override int GetScoreChangeAfterAddition() => 0;
        
        public override int GetScoreChangeAfterRemoval() => 0;

        public List<Card> GetAllCards() => _cards;
    }
}
