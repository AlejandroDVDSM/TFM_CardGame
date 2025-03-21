using CardGame.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class EnemyCard : Card
    {
        private ParticleSystem m_particleSystem;
        
        protected override void Start()
        {
            base.Start();
            m_particleSystem = FindAnyObjectByType<ParticleSystem>();
            GameManager.Instance.OnTurnCommited.AddListener(AutoAttack);
        }

        public override void PerformAction()
        {
            // The enemy will avoid the player if it is invisible
            if (GameManager.Instance.Player.Status.HasStatusApplied(EStatusType.Invisibility))
            {
                GameManager.Instance.CommitTurn();
                return;
            }
            
            // Animation where the card charges forward against the player
            Sequence attackSequence = DOTween.Sequence();
            attackSequence
                .Append(transform.DOScale(Vector3.one * 1.2f, .6f).SetEase(Ease.OutQuint))
                .Append(transform.DOMoveY(GameManager.Instance.Player.transform.position.y, .4f).SetEase(Ease.InOutBack));
                
            // Animation where the card returns to the original position
            Sequence knockbackSequence = DOTween.Sequence();
            knockbackSequence
                .Insert(0, transform.DOScale(Vector3.one, .4f).SetEase(Ease.InQuint))
                .Insert(0, transform.DOLocalMoveY(0, 0.4f).SetEase(Ease.InBack))
                .Append(transform.DOShakePosition(1.2f, 20f));
            
            Sequence destroySequence = DOTween.Sequence();
            destroySequence
                .Append(m_canvasGroup.DOFade(0, 1.0f).SetEase(Ease.InBack));
            
            attackSequence.Play();
            attackSequence.OnComplete(() =>
            {
                // Hit player
                GameManager.Instance.Player.Hit(m_value);
                knockbackSequence.Play();
            });

            knockbackSequence.OnComplete(() =>
            {
                destroySequence.Play();
            });

            destroySequence.OnComplete(() =>
            {
                m_particleSystem.transform.position = transform.position;
                var sh = m_particleSystem.shape;
                sh.texture = (Texture2D)image.mainTexture;
                m_particleSystem.Play();
                
                // Replace this enemy card for a coin card
                ItemCard coinCard = GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Coin);
                coinCard.UpdateValue(Value);
                GetComponentInParent<CardRow>().PlaceSingleCard(coinCard, Lane, transform.GetSiblingIndex());
                GameManager.Instance.CardPool.DestroyCard(this);
            });
        }

        public void Hit(int damage)
        {
            UpdateValue(Mathf.Clamp(m_value - damage, 0, m_value));
        }

        /// <summary>
        /// Automatically attacks a player after commiting a turn if the player is in the enemy line of sight
        /// </summary>
        private void AutoAttack()
        {
            if (GameManager.Instance.Player.Status.HasStatusApplied(EStatusType.Invisibility))
            {
                return;
            }
            
            if (GameManager.Instance.Player.Movement.CurrentLane == Lane && m_currentRow == ERow.Bottom)
            {
                PerformAction();
            }
        }
    }
}