using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Behaviours;
using Game.Scripts.Behaviours.Piles;
using Game.Scripts.Models;
using Mek.Extensions;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2? _lastMousePos;
        private Vector2? _currentMousePos;

        private bool IsPressing => _lastMousePos.HasValue;
        
        public bool CanPlay { get; set; }

        private Camera _cam;

        private List<Card> _heldCards = new List<Card>();
        
        private void Awake()
        {
            _cam = CameraController.Instance.MainCamera;
        }

        private void Update()
        {
            if (!CanPlay) return;
            
            EvaluateInputs();
        }

        private void EvaluateInputs()
        {
            _currentMousePos = Input.mousePosition;
            
            if (!IsPressing && Input.GetMouseButtonDown(0))
            {
                _lastMousePos = _currentMousePos;
                OnPressPerformed();
                return;
            }

            if (IsPressing && Input.GetMouseButtonUp(0))
            {
                OnPressCanceled();
                _currentMousePos = null;
                _lastMousePos = null;
                return;
            }

            if (IsPressing && Input.GetMouseButton(0) && _currentMousePos != _lastMousePos)
            {
                OnDragPerformed();
                _lastMousePos = _currentMousePos;
            }
        }

        private void OnPressPerformed()
        {
            if (!CanPlay) return;
            if (_lastMousePos == null || _currentMousePos == null) return;

            var worldPoint = _cam.ScreenToWorldPoint(_currentMousePos.Value);
            
            Collider2D[] overlappingColliders = Physics2D.OverlapPointAll(worldPoint, LayerMask.GetMask("Card"));
            
            if (overlappingColliders.Length > 0)
            {
                var cards = overlappingColliders.Select(c => c.GetComponent<Card>()).ToList();
                var card = cards.OrderByDescending(c => c.Order).First();
                if (BoardController.Instance.CanCardBeDraggable(card, out var draggableCards))
                {
                    _heldCards = draggableCards;

                    for (int i = 0; i < _heldCards.Count; i++)
                    {
                        _heldCards[i].Order = 1000 * (i + 1);
                    }
                    // card.CurrentPile.Remove(_heldCards);
                }
            }
        }

        private void OnPressCanceled()
        {
            if (!CanPlay) return;
            if (_lastMousePos == null || _currentMousePos == null) return;
            if (!_heldCards.Any()) return;

            var topCard = Enumerable.First(_heldCards);
            var lastPile = topCard.CurrentPile;

            var overlappingCardColliders = Physics2D.OverlapBoxAll(topCard.transform.position, topCard.Collider.bounds.size, 0, LayerMask.GetMask("Card"), -Mathf.Infinity, Mathf.Infinity);
            var cards = new List<Card>();
            
            if (overlappingCardColliders.Length > 1)
            {
                cards = overlappingCardColliders.Select(c => c.GetComponent<Card>()).ToList();

                foreach (var card in _heldCards)
                {
                    cards.Remove(card); // removing held cards in overlapped cards
                }
            }
            if (cards.Count > 0)
            {
                var cardOnPile = cards.OrderByDescending(c => c.Order).First();

                if (cards.Any(c => c != cardOnPile && c.CardData.Number == cardOnPile.CardData.Number)) // dilemma between two possible cards, solution is put on closest pile
                {
                    cardOnPile = cards.OrderBy(cc => Vector2.Distance(topCard.transform.position, cc.transform.position)).First();
                }
                
                var pile = cardOnPile.CurrentPile;
                
                BoardController.Instance.TryToPutCards(_heldCards, lastPile, pile);
            }
            else
            {
                var overlappingPileColliders = Physics2D.OverlapBoxAll(topCard.transform.position, topCard.Collider.bounds.size, 0, LayerMask.GetMask("Pile", "Card"), -Mathf.Infinity, Mathf.Infinity);
                var piles = new List<Pile>();

                if (overlappingPileColliders.Length > 0)
                {
                    piles = overlappingPileColliders
                        .Where(c => c.gameObject.layer == LayerMask.NameToLayer("Pile"))
                        .Select(c => c.GetComponent<Pile>()).ToList();
                }
                if (piles.Count > 0)
                {
                    var pile = Enumerable.First(piles);
                    if (piles.Count > 1)
                    {
                        pile = piles.OrderBy(cc => Vector2.Distance(topCard.transform.position, cc.transform.position)).First();
                    }

                    BoardController.Instance.TryToPutCards(_heldCards, lastPile, pile);
                }
                else
                {
                    lastPile.ArrangeOrders();
                }
            }
            
            _heldCards.Clear();
        }

        private void OnDragPerformed()
        {
            if (!CanPlay) return;
            if (_lastMousePos == null || _currentMousePos == null) return;
            if (_heldCards == null) return;
            if (_heldCards.Count == 0) return;

            var currentScreenPoint = _cam.ScreenToWorldPoint(_currentMousePos.Value);
            var lastScreenPoint = _cam.ScreenToWorldPoint(_lastMousePos.Value);
            var delta = currentScreenPoint - lastScreenPoint;

            foreach (var card in _heldCards)
            {
                card.transform.position += new Vector3(delta.x, delta.y, 0);
            }
        }
    }
}
