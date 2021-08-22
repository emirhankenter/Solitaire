using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Scripts.Behaviours.Piles
{
    public abstract class Pile : MonoBehaviour
    {
        public List<Card> _cards = new List<Card>();

        public void Add(Card card)
        {
            _cards.Add(card);
            card.transform.SetParent(transform, true);
            card.SetPile(this);
            OnCardAdded(card);
            ArrangeOrders();
        }
        
        public void Add(List<Card> cards)
        {
            _cards.AddRange(cards);

            foreach (var card in cards)
            {
                card.transform.SetParent(transform, true);
                card.SetPile(this);
            }
            
            OnCardsAdded(cards);
            ArrangeOrders();
        }

        public void Remove(Card card)
        {
            _cards.Remove(card);
            card.transform.SetParent(null, true);
            OnCardRemoved(card);
            // ArrangeOrders();
        }

        public void Remove(List<Card> cards)
        {
            _cards.RemoveRange(_cards.Count - cards.Count, cards.Count);

            foreach (var card in cards)
            {
                card.transform.SetParent(null, true);
            }
            OnCardsRemoved(cards);
            // ArrangeOrders();
        }

        protected abstract void OnCardAdded(Card card);
        protected abstract void OnCardRemoved(Card card);

        protected abstract void OnCardsAdded(List<Card> cards);
        protected abstract void OnCardsRemoved(List<Card> cards);

        public abstract void ArrangeOrders();

        public abstract bool CanCardBeDraggable(Card card);
        public abstract bool CanCardPutHere(Card card);
    }
}
