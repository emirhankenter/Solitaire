using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Behaviours.Piles;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Mek.Coroutines;
using Mek.Extensions;
using Mek.Models;
using Mek.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class BoardController : SingletonBehaviour<BoardController>
    {
        public event Action ReadyToPlay;
        public event Action Completed;
        public event Action<int> ScoreMade;
        
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private List<Pile> _mainPiles;
        [SerializeField] private List<FoundationPile> _foundationPiles;
        [SerializeField] private OpenedDeckPile _openedDeckPile;
        [SerializeField] private ClosedDeckPile _closedDeckPile;

        [SerializeField] private AudioClip _dealCardAudioClip;
        [SerializeField] private AudioClip _mainPileAudioClip;
        [SerializeField] private AudioClip _errorAudioClip;

        public readonly Dictionary<CardSeedType, CardColorType> CardColorDictionary =
            new Dictionary<CardSeedType, CardColorType>()
            {
                {CardSeedType.Clubs, CardColorType.Black},
                {CardSeedType.Diamonds, CardColorType.Red},
                {CardSeedType.Hearts, CardColorType.Red},
                {CardSeedType.Spades, CardColorType.Black},
            };

        private List<Sprite> _numberSprites;
        private List<Sprite> _seedSprites;
        
        private List<Card> _spawnedCards = new List<Card>();

        protected override void OnAwake()
        {
            _numberSprites = Resources.LoadAll<Sprite>("Numbers").OrderBy(obj => int.Parse(obj.name)).ToList();
            _seedSprites = Resources.LoadAll<Sprite>("Seeds").OrderBy(obj => int.Parse(obj.name)).ToList();
        }

        public void Init()
        {
            _closedDeckPile.Clicked += OnDrawCardClicked;
            MovementData.MovementMade += OnMovementMade;
            Pile.ScoreMade += OnScoreMade;
        }

        public void Dispose()//todo: dispose!!!!
        {
            _closedDeckPile.Clicked -= OnDrawCardClicked;
            MovementData.MovementMade -= OnMovementMade;
            Pile.ScoreMade -= OnScoreMade;
        }

        [Button]
        public void ResetBoard()
        {
            foreach (var card in _spawnedCards)
            {
                card.Recycle();
            }
            
            _spawnedCards.Clear();
            
            _closedDeckPile.Clear();
            _openedDeckPile.Clear();
            _foundationPiles.ForEach(pile => pile.Clear());
            _mainPiles.ForEach(pile => pile.Clear());
        }

        public void DealCards()
        {
            var cards = new List<Card>();
            
            foreach (var pair in CardColorDictionary)
            {
                var seedSprite = _seedSprites[(int) Enum.Parse(typeof(CardSeedType), pair.Key.ToString())];
                
                for (int i = 1; i <= 13; i++)
                {
                    var cardData = new CardData(pair.Key, pair.Value, seedSprite, _numberSprites[i - 1], i);
                    var card = _cardPrefab.Spawn();
                    card.Collider.enabled = true;
                    _spawnedCards.Add(card);
                    card.SetData(cardData);
                    cards.Add(card);
                }
            }

            var shuffledCards = new List<Card>();
            shuffledCards.AddRange(cards);
            shuffledCards.Shuffle();
            
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
                            .SetEase(Ease.Linear)
                            .OnStart(() =>
                            {
                                if (MekPlayerData.SoundFXEnabled)
                                {
                                    _dealCardAudioClip.Play(0.2f);
                                }
                            });
                        
                        card.Init(count == 0);
                        card.SetPile(pile);
                        pile.Add(card);
                        
                        i1 += 0.1f;
                    }
                }
            }
            CoroutineController.DoAfterGivenTime(i1, () =>
            {
                ReadyToPlay?.Invoke();
            });

            
            foreach (var card in shuffledCards)
            {
                _closedDeckPile.Add(card);
                card.transform.position = _closedDeckPile.transform.position;
                card.Init(false);
            }
        }

        private void OnMovementMade()
        {
            if (HasCompletedSuccessfully())
            {
                Completed?.Invoke();
            }
        }

        private void OnScoreMade(int score)
        {
            // ScoreMade?.Invoke(score);
        }

        private bool HasCompletedSuccessfully()
        {
            return _foundationPiles.All(pile => pile.HasCompletedSuccessfully());
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

            if (card.CurrentPile.CanCardBeDraggable(card))
            {
                return true;
            }

            foreach (var c in cards)
            {
                c.transform.DOShakePosition(0.1f, Vector3.one);
            }

            return false;

            return card.CurrentPile.CanCardBeDraggable(card);
        }

        public bool CanCardsPutOnPile(List<Card> cards, Pile pile)
        {
            var canBePut = pile.CanCardsPutHere(cards);

            if (!canBePut)
            {
                if (MekPlayerData.SoundFXEnabled)
                {
                    _errorAudioClip.Play();
                }
            }

            return canBePut;
        }

        [Button]
        public void OnDrawCardClicked()
        {
            if (GameController.Instance.IsPaused) return;
            
            if (_closedDeckPile.Draw(out List<Card> cards, PlayerData.Instance.Draw3Enabled ? 3 : 1)) // todo: can draw there
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
                
                ScoreMade?.Invoke(-GameController.Instance.CurrentSession.Score);
                
                HistoryController.Instance.SaveMovement(new MovementData(new List<Card>(cardsToDeal), _openedDeckPile, _closedDeckPile, flippedCards));
            }
        }

        public bool TryToPutCards(List<Card> cards, Pile from, Pile to)
        {
            if (from == to)
            {
                from.ArrangeOrders();
                return false;
            }
            if (!to.CanCardsPutHere(cards))
            {
                from.ArrangeOrders();
                if (MekPlayerData.SoundFXEnabled)
                {
                    _errorAudioClip.Play();
                }
                return false;
            }

            int score = CalculateScore(from, to);
            ScoreMade?.Invoke(score);
            
            from.Remove(cards);
            to.Add(cards);
            from.ShouldFlip(out List<Card> flippedCards);

            if (to is MainPile mainPile)
            {
                if (MekPlayerData.SoundFXEnabled)
                {
                    _mainPileAudioClip.Play(0.2f);
                }
            }
            
            HistoryController.Instance.SaveMovement(new MovementData(new List<Card>(cards), from, to, flippedCards, score));
            return true;
        }

        private int CalculateScore(Pile from, Pile to)
        {
            if (from == to) return 0;

            var score = 0;

            if (to is FoundationPile)
            {
                var minusScore = from.GetScoreChangeAfterRemoval();
                var additionScore = to.GetScoreChangeAfterAddition();

                score = additionScore + minusScore;
            }
            else
            {
                if (to._cards.Count == 0) return 0;
                
                var minusScore = from.GetScoreChangeAfterRemoval();
                var additionScore = to.GetScoreChangeAfterAddition();

                score = additionScore + minusScore;
            }
            return score;
        }

        [Button]
        public void Undo()
        {
            if (HistoryController.Instance.TryUndo(out MovementData movementData))
            {
                movementData.Undo();
                
                ScoreMade?.Invoke(-movementData.Score);
            }
        }
    }
}
