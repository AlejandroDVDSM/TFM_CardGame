using CardGame.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class EnemyCard : Card
    {
        private ParticleSystem m_particleSystem;

        private Sequence m_attackSequence;
        private Sequence m_chargeSequence;
        private Sequence m_knockbackSequence;
        private Sequence m_destroySequence;
        
        protected override void Start()
        {
            base.Start();
            m_particleSystem = FindAnyObjectByType<ParticleSystem>();
            GameManager.Instance.OnTurnCommited.AddListener(AutoAttack);
        }

        public override void PerformAction()
        {
            m_isPerformingAction = true;
            
            // The enemy will avoid the player if it is invisible
            if (GameManager.Instance.Player.Status.HasStatusApplied(EStatusType.Invisibility))
            {
                GameManager.Instance.CommitTurn();
                m_isPerformingAction = false;
                return;
            }

            Attack();
        }
        
        private void Attack()
        {   
            // Create animation where the card charges forward against the player
            m_chargeSequence = DOTween.Sequence();
            m_chargeSequence
                .Append(transform.DOScale(Vector3.one * 1.3f, 0.4f).SetEase(Ease.OutQuint))
                .Append(transform.DOMoveY(GameManager.Instance.Player.transform.position.y, 0.3f).SetEase(Ease.InOutBack))
                .OnComplete(() =>
                {
                    // Hit player
                    GameManager.Instance.Player.Hit(m_value);
                });
            
                
            // Create animation where the card returns to the original position
            m_knockbackSequence = DOTween.Sequence();
            m_knockbackSequence
                .Insert(0, transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuint))
                .Insert(0, transform.DOLocalMoveY(0, 0.25f).SetEase(Ease.InBack))
                .Append(transform.DOShakePosition(0.6f, 25f));
            
            // Create animation where the card is destroyed and replaced by a coin
            m_destroySequence = DOTween.Sequence();
            m_destroySequence
                .Append(m_canvasGroup.DOFade(0, 0.4f).SetEase(Ease.InBack))
                .OnComplete(() =>
                {
                    m_particleSystem.transform.position = transform.position;
                    var sh = m_particleSystem.shape;
                    sh.texture = (Texture2D)image.mainTexture;
                    m_particleSystem.Play();
                
                    // Replace this enemy card for a coin card
                    ItemCard coinCard = GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Coin);
                    coinCard.UpdateValue(Value);
                    GetComponentInParent<CardRow>().PlaceSingleCard(coinCard, (int)Lane, transform.GetSiblingIndex());
                    GameManager.Instance.CardPool.DestroyCard(this);
                });
            
            // Play full animation
            m_attackSequence = DOTween.Sequence();
            m_attackSequence
                .Append(m_chargeSequence)
                .Append(m_knockbackSequence)
                .Append(m_destroySequence)
                .OnComplete(() => m_isPerformingAction = false);
            
            m_attackSequence.Play();
        }

        public void Hit(int damage)
        {
            UpdateValue(Mathf.Clamp(m_value - damage, 0, m_value));
            transform.DOShakePosition(0.75f, 10f);
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