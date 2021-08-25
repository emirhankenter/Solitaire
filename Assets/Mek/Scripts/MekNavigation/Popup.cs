using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mek.Audio;
using UnityEngine;

namespace Mek.Navigation
{
    public class Popup : ContentBase
    {
        [SerializeField] private bool _animate;
        [SerializeField] private AudioClip _openingSound;
        [SerializeField] private AudioClip _closingSound;

        public bool IsAnimating { get; private set; } = false;
        
        public override void Open(ViewParams viewParams)
        {
            if (_openingSound)
            {
                AudioController.Play(_openingSound);
            }

            if (!_animate)
            {
                base.Open(viewParams);
            }
            else
            {
                transform.localScale = Vector3.zero;
                IsAnimating = true;
                transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                    .OnComplete(() => IsAnimating = false);
                base.Open(viewParams);
            }
        }

        public override void Close()
        {
            if (_closingSound)
            {
                AudioController.Play(_closingSound);
            }

            if (!_animate)
            {
                base.Close();
            }
            else
            {
                IsAnimating = true;
                transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    transform.localScale = Vector3.one;
                    base.Close();
                    IsAnimating = false;
                });
            }
        }
    }
}