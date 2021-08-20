using System;
using DG.Tweening;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _frontRenderer;
        [SerializeField] private SpriteRenderer _backRenderer;
        [SerializeField] private SpriteRenderer _seedRenderer;
        [SerializeField] private SpriteRenderer _numberRenderer;

        private int _order;

        [ShowInInspector, MinValue(0)]
        private int Order
        {
            get => _order;
            set
            {
                _order = value;
                OnOrderChanged();
            }
        }

        private CardData _cardData;
        private bool _isFacedFront;
        public bool IsFlipping => DOTween.IsTweening(transform);
        

        public void Init(CardData cardData, bool isFacedFront)
        {
            _cardData = cardData;
            _isFacedFront = isFacedFront;
            
            var localEulerAngles = transform.localEulerAngles;
            localEulerAngles.y = isFacedFront ? 0 : 180f;
            transform.localEulerAngles = localEulerAngles;
            
            _frontRenderer.gameObject.SetActive(_isFacedFront);
            _backRenderer.gameObject.SetActive(!_isFacedFront);

            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            _seedRenderer.sprite = _cardData.SeedSprite;
            _numberRenderer.sprite = _cardData.NumberSprite;

            var color = _cardData.CardColorType == CardColorType.Red ? Color.red : Color.black;
            _seedRenderer.color = color;
            _numberRenderer.color = color;
        }

        [Button]
        public void Flip()
        {
            if (IsFlipping) return;
            flip90Degree(() =>
            {
                SwitchFace();
                flip90Degree();
            });

            void flip90Degree(Action onComplete = null)
            {
                transform.DOLocalRotate(new Vector3(0, 90f, 0), 0.1f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => onComplete?.Invoke());
            }
        }

        private void SwitchFace()
        {
            _isFacedFront = !_isFacedFront;
            _frontRenderer.gameObject.SetActive(_isFacedFront);
            _backRenderer.gameObject.SetActive(!_isFacedFront);
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
