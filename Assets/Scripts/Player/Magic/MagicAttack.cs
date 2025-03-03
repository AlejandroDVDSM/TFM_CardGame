using CardGame.Enums;
using UnityEngine;

public abstract class MagicAttack : MonoBehaviour
{
    [SerializeField]
    protected MagicData m_magicData;
    
    protected Player m_player;

    internal bool hasUsedMagic;
    
    private void Start()
    {
        m_player = GetComponent<Player>();
        GameManager.Instance.OnTurnCommited.AddListener(RestoreMagicUse);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cast();
        }
    }

    protected abstract void Cast();
    
    protected bool CanCast()
    {
        // If the player has already used the magic in this turn...
        if (hasUsedMagic)
        {
            Debug.Log($"The player can't use <{typeof(MagicAttack)}> as it has already been used in this turn");   
            return false;
        }
        
        // If the player doesn't have enough mana...
        if (m_player.CurrentMana < m_magicData.ManaCost)
        {
            Debug.Log($"There is no enough mana to cast <{typeof(MagicAttack)}> | Player Mana: {m_player.CurrentMana} <-> Cost: {m_magicData.ManaCost}");
            return false;
        }
        
        // If the player is silenced...
        if (m_player.Status.HasStatusApplied(EStatusType.Silence))
        {
            Debug.Log($"The player can't use <{typeof(MagicAttack)}> as it is silenced");
            return false;
        }

        return true;
    }
    
    private void RestoreMagicUse()
    {
        hasUsedMagic = false;
    }
}
