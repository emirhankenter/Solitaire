using System.Collections.Generic;

namespace Game.Scripts.Behaviours.Piles
{
    public class OpenedDeckPile : Pile
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

        protected override void ArrangeOrders()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].Order = i * 10;
            }
        }

        public override bool CanCardBeDraggable(Card card)
        {
            return _cards.IndexOf(card) == _cards.Count - 1;
        }

        public override bool CanCardPutHere(Card card) => false;
    }
}
