using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Behaviours;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Mek.Controllers;
using Mek.Localization;
using Mek.Models;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : SingletonBehaviour<GameController>
    {
        [SerializeField] private Card _cardPrefab;
        
        public readonly Dictionary<CardSeedType, CardColorType> CardColorDictionary =
            new Dictionary<CardSeedType, CardColorType>()
            {
                {CardSeedType.Clubs, CardColorType.Black},
                {CardSeedType.Diamonds, CardColorType.Red},
                {CardSeedType.Hearts, CardColorType.Red},
                {CardSeedType.Spades, CardColorType.Black},
            };
        
        protected override void OnAwake()
        {
            LocalizationManager.Init(SystemLanguage.Turkish);
            PrefsManager.Initialize();
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Ready");
            });
            
            PopulateCards();
        }
        
        private void PopulateCards()
        {
            var x = 0f;
            var y = 0f;
            var numbers = Resources.LoadAll<Sprite>("Numbers").OrderBy(obj => int.Parse(obj.name)).ToList();
            var seeds = Resources.LoadAll<Sprite>("Seeds").OrderBy(obj => int.Parse(obj.name)).ToList();
            
            foreach (var pair in CardColorDictionary)
            {
                var seedSprite = seeds[(int) Enum.Parse(typeof(CardSeedType), pair.Key.ToString())];
                
                for (int i = 1; i <= 13; i++)
                {
                    var cardData = new CardData(pair.Key, pair.Value, seedSprite, numbers[i - 1], i);
                    var card = Instantiate(_cardPrefab, new Vector3(x,y,0), Quaternion.identity);
                    card.Init(cardData, true);
                    x += 1.1f;
                }

                x = 0f;
                y -= 1.74f;
            }
        }
    }
}
