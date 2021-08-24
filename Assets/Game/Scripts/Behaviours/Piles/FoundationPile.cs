using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Mek.Extensions;
using UnityEngine;

namespace Game.Scripts.Behaviours.Piles
{
    public class FoundationPile : Pile
    {
        [SerializeField] private AudioClip _foundationPileAudioClip;
        protected override void OnCardAdded(Card card)
        {
            _foundationPileAudioClip.Play();
            MakeScore(GameConfig.ScoreOnFoundationPile);
        }

        protected override void OnCardRemoved(Card card)
        {
            MakeScore(-GameConfig.ScoreOnFoundationPile);
        }

        protected override void OnCardsAdded(List<Card> cards)
        {
            _foundationPileAudioClip.Play();
            MakeScore(GameConfig.ScoreOnFoundationPile);
        }

        protected override void OnCardsRemoved(List<Card> cards)
        {
            MakeScore(Mathf.RoundToInt(-GameConfig.ScoreOnFoundationPile * 1.5f));
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
