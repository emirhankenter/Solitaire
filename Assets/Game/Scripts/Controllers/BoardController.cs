using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Behaviours.Piles;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Mek.Extensions;
using Mek.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class BoardController : SingletonBehaviour<BoardController>
    {
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private List<Pile> _mainPiles;
        [SerializeField] private OpenedDeckPile _openedDeckPile;
        [SerializeField] private ClosedDeckPile _closedDeckPile;
        
        public readonly Dictionary<CardSeedType, CardColorType> CardColorDictionary =
            new Dictionary<CardSeedType, CardColorType>()
            {
                {CardSeedType.Clubs, CardColorType.Black},
                {CardSeedType.Diamonds, CardColorType.Red},
                {CardSeedType.Hearts, CardColorType.Red},
                {CardSeedType.Spades, CardColorType.Black},
            };

        
        public void PopulateCards()
        {
            var x = 0f;
            var y = 0f;
            var numbers = Resources.LoadAll<Sprite>("Numbers").OrderBy(obj => int.Parse(obj.name)).ToList();
            var seeds = Resources.LoadAll<Sprite>("Seeds").OrderBy(obj => int.Parse(obj.name)).ToList();

            var cards = new List<Card>();
            
            foreach (var pair in CardColorDictionary)
            {
                var seedSprite = seeds[(int) Enum.Parse(typeof(CardSeedType), pair.Key.ToString())];
                
                for (int i = 1; i <= 13; i++)
                {
                    var cardData = new CardData(pair.Key, pair.Value, seedSprite, numbers[i - 1], i);
                    var card = Instantiate(_cardPrefab, new Vector3(x,y,0), Quaternion.identity);
                    card.SetData(cardData);
                    cards.Add(card);
                    // card.Init(true);
                    x += 1.1f;
                }

                x = 0f;
                y -= 1.74f;
            }

            var shuffledCards = new List<Card>();
            shuffledCards.AddRange(cards);
            shuffledCards.Shuffle();
            
            // var mainPileCards = suffledCards.DrawFirstXElement(25)


            foreach (var card in cards)
            {
                card.transform.SetParent(_closedDeckPile.transform, true);
                card.transform.position = _closedDeckPile.transform.position;
                card.SetPile(_closedDeckPile);
            }

            var i1 = 0f;
            for (int i = 0; i < 7; i++)
            {
                var count = i + 1;
                
                for (int j = 0; j < 7; j++)
                {
                    if (count > 0)
                    {
                        var card = shuffledCards.First();
                        shuffledCards.Remove(card);
                        var pile = _mainPiles[i];
                        // card.transform.position = pile.transform.position + Vector3.up * _mainPileYOffset * j;
                        count--;
                        card.transform.DOMove(pile.transform.position + Vector3.up * (MainPile.MainPileYOffset * j), 0.2f)
                            .SetDelay(i1)
                            .SetEase(Ease.Linear);
                        
                        card.Init(count == 0);
                        card.SetPile(pile);
                        pile.Add(card);
                        
                        i1 += 0.04f;
                    }
                }
            }

            
            foreach (var card in shuffledCards)
            {
                _closedDeckPile.Add(card);
                card.transform.position = _closedDeckPile.transform.position;
                card.Init(false);
            }
        }

        public bool CanCardBeDraggable(Card card, out List<Card> cards)
        {
            cards = null;

            if (!card.IsFacedFront)
            {
                // todo: Card can be shake here to inform user (optional)
                return false;
            }

            var pile = card.CurrentPile;
            cards = pile switch
            {
                MainPile mainPile => mainPile.GetCardStack(card),
                FoundationPile foundationPile => new List<Card>() {card},
                OpenedDeckPile openedDeckPile => new List<Card>() {card},
                _ => cards
            };

            return card.CurrentPile.CanCardBeDraggable(card);
        }

        [Button]
        public void OnDrawCardClicked()
        {
            if (_closedDeckPile.Draw(out List<Card> cards, 1)) // todo: can draw there
            {
                _closedDeckPile.Remove(cards);
                cards.Reverse();
                var flippedCards = new List<Card>();
                foreach (var card in cards)
                {
                    card.Flip();
                    _openedDeckPile.Add(card);
                    flippedCards.Add(card);
                }
                
                HistoryController.Instance.SaveMovement(new MovementData(new List<Card>(cards), _closedDeckPile, _openedDeckPile, flippedCards));
            }
            else
            {
                var cardsToDeal = new List<Card>(_openedDeckPile.GetAllCards());
                cardsToDeal.Reverse();
                _openedDeckPile.Remove(cardsToDeal);
                
                var flippedCards = new List<Card>();
                foreach (var card in cardsToDeal)
                {
                    card.Flip();
                    flippedCards.Add(card);
                }
                _closedDeckPile.Add(cardsToDeal);
                
                HistoryController.Instance.SaveMovement(new MovementData(new List<Card>(cardsToDeal), _openedDeckPile, _closedDeckPile, flippedCards));
            }
        }

        [Button]
        public void Undo()
        {
            if (HistoryController.Instance.TryUndo(out MovementData movementData))
            {
                movementData.Undo();
            }
        }

        protected override void OnAwake()
        {
            
        }
    }
}
