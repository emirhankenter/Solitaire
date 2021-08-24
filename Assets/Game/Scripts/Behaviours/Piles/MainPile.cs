using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Models;
using Mek.Extensions;
using UnityEngine;

namespace Game.Scripts.Behaviours.Piles
{
    public class MainPile : Pile
    {
        public const float MainPileYOffset = -0.5f;
        
        protected override void OnCardAdded(Card card)
        {
        }
        protected override void OnCardRemoved(Card card)
        {
        }
        protected override void OnCardsAdded(List<Card> cards)
        {
            ArrangeOrders();

            for (int i = 0; i < cards.Count; i++)
            {
                MakeScore(GameConfig.ScoreOnMainPile);
            }
        }
        protected override void OnCardsRemoved(List<Card> cards)
        {
            ArrangeOrders();
        }

        protected override void OnUndo()
        {
            
        }

        public override void ArrangeOrders()
        {
            if (_cards.Count == 0) return;
            
            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                card.Order = i * 10;
                if (!DOTween.IsTweening(card.transform))
                {
                    card.transform.localPosition = Vector3.zero + Vector3.up * (MainPileYOffset * i);
                }

                if (i == _cards.Count - 1 && !card.IsFacedFront)
                {
                    // card.Flip();
                }
            }
        }

        public override bool ShouldFlip(out List<Card> flippedCards)
        {
            var hasFlipped = false;
            flippedCards = null;
            
            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];

                if (i != _cards.Count - 1 || card.IsFacedFront) continue;
                card.Flip();
                flippedCards ??= new List<Card>();
                flippedCards.Add(card);
                hasFlipped = true;
            }

            return hasFlipped;
        }

        public override bool CanCardBeDraggable(Card card)
        {
            var index = _cards.IndexOf(card);

            if (index == _cards.Count - 1) return true;
            
            Card lastCheckedCard = card;
            for (int i = index + 1; i < _cards.Count; i++)
            {
                var c = _cards[i];
                if (i != index + 1)
                {
                    if (lastCheckedCard.CardData.CardColorType == c.CardData.CardColorType)
                    {
                        return false;
                    }
                }
                lastCheckedCard = c;
                if (card.CardData.Number > lastCheckedCard.CardData.Number)
                {
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override bool CanCardsPutHere(List<Card> cards)
        {
            var topCard = cards.First();
            if (_cards.Count == 0) return topCard.CardData.Number == 13;
            var lastCard = _cards.Last();
            if (!lastCard || !lastCard.IsFacedFront) return true;

            return lastCard.CardData.Number - topCard.CardData.Number == 1 && lastCard.CardData.CardColorType != topCard.CardData.CardColorType;
        }

        public List<Card> GetCardStack(Card card)
        {
            var list = new List<Card>();
            if (!CanCardBeDraggable(card))
            {
                Debug.LogError($"Something is wrong while getting cards stack while holding {card.CardData.GetCardInfo()}! Returning empty card stack!");
                return list;
            }

            var index = _cards.IndexOf(card);

            for (var i = index; i < _cards.Count; i++)
            {
                list.Add(_cards[i]);
            }

            return list;
        }
    }
}
