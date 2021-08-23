using System.Collections.Generic;
using DG.Tweening;
using Mek.Extensions;
using UnityEngine;

namespace Game.Scripts.Behaviours.Piles
{
    public class FoundationPile : Pile
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
                if (DOTween.IsTweening(card.transform))
                {
                    DOTween.Kill(card.transform);
                }
                card.transform.localPosition = Vector3.zero;
            }
        }

        public override bool ShouldFlip(out List<Card> flippedCards)
        {
            flippedCards = null;
            return false;
        }

        public override bool CanCardBeDraggable(Card card) => _cards.Count == 0 || card == _cards.Last();

        public override bool CanCardsPutHere(List<Card> cards)
        {
            if (cards.Count > 1) return false;

            var card = cards.First();
            var cardData = card.CardData;
            
            if (_cards.Count == 0)
            {
                return cardData.Number == 1;
            }

            var lastCardInPile = _cards.Last();

            if (cardData.CardSeedType != lastCardInPile.CardData.CardSeedType) return false;

            return cardData.Number - lastCardInPile.CardData.Number == 1;
        }

        public bool HasCompletedSuccessfully()
        {
            return _cards.Count == 13;
        }
    }
}
