using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Models
{
    public class CardData
    {
        public CardSeedType CardSeedType { get; }
        public CardColorType CardColorType { get; }
        public Sprite SeedSprite { get; }
        public Sprite NumberSprite { get; }
        public int Number { get; }

        public CardData(CardSeedType cardSeedType, CardColorType cardColorType, Sprite seedSprite, Sprite numberSprite, int number)
        {
            CardSeedType = cardSeedType;
            CardColorType = cardColorType;
            NumberSprite = numberSprite;
            SeedSprite = seedSprite;
            Number = number;
        }

        private string EvaluateNumber()
        {
            return Number switch
            {
                1 => "Ace",
                11 => "Jack",
                12 => "Queen",
                13 => "King",
                _ => Number.ToString()
            };
        }

        public string GetCardInfo() => $"{CardColorType} {CardSeedType} {EvaluateNumber()}";
    }
}