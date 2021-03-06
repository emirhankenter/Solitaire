using System;
using DG.Tweening;
using Game.Scripts.Behaviours.Piles;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Mek.Extensions;
using Mek.Interfaces;
using Mek.Models;
using Mek.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private SpriteRenderer _frontRenderer;
        [SerializeField] private SpriteRenderer _backRenderer;
        [SerializeField] private SpriteRenderer _seedRenderer;
        [SerializeField] private SpriteRenderer _numberRenderer;
        
        [SerializeField] private AudioClip _flipSound;

        public BoxCollider2D Collider => _collider;

        private int _order;

        private Tween _flipTween;
        
        [ShowInInspector, MinValue(0)]
        public int Order
        {
            get => _order;
            set
            {
                _order = value;
                OnOrderChanged();
            }
        }

        public CardData CardData { get; private set; }
        public Pile CurrentPile { get; private set; }
        public bool IsFacedFront { get; private set; }
        public bool IsFlipping => DOTween.IsTweening(transform);

        public void SetData(CardData cardData)
        {
            CardData = cardData;
        }

        public void Init(bool isFacedFront)
        {
            IsFacedFront = isFacedFront;
            
            var localEulerAngles = transform.localEulerAngles;
            localEulerAngles.y = isFacedFront ? 0 : 180f;
            transform.localEulerAngles = localEulerAngles;
            
            _frontRenderer.gameObject.SetActive(IsFacedFront);
            _backRenderer.gameObject.SetActive(!IsFacedFront);

            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            _seedRenderer.sprite = CardData.SeedSprite;
            _numberRenderer.sprite = CardData.NumberSprite;

            var color = CardData.CardColorType == CardColorType.Red ? Color.red : Color.black;
            _seedRenderer.color = color;
            _numberRenderer.color = color;
        }

        public void SetPile(Pile pile)
        {
            CurrentPile = pile;
        }

        [Button]
        public void Flip(bool withAnimation = true)
        {
            if (IsFlipping)
            {
                _flipTween.Kill();
                transform.localEulerAngles = new Vector3(0, IsFacedFront ? 0 : 180, 0);
            }
            if (withAnimation)
            {
                if (MekPlayerData.SoundFXEnabled)
                {
                    _flipSound.Play(0.2f);
                }
            }
            flip90Degree(() =>
            {
                SwitchFace();
                flip90Degree();
            });

            void flip90Degree(Action onComplete = null)
            {
                _flipTween = transform.DOLocalRotate(new Vector3(0, 90f, 0), withAnimation ? 0.1f : 0f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => onComplete?.Invoke());
            }
        }

        private void SwitchFace()
        {
            IsFacedFront = !IsFacedFront;
            _frontRenderer.gameObject.SetActive(IsFacedFront);
            _backRenderer.gameObject.SetActive(!IsFacedFront);
        }

        private void OnOrderChanged()
        {
            _frontRenderer.sortingOrder = Order;
            _backRenderer.sortingOrder = Order;
            _seedRenderer.sortingOrder = Order + 1;
            _numberRenderer.sortingOrder = Order + 1;
        }
    }
}
