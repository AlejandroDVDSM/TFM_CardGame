using CardGame.Enums;
using UnityEngine;

public abstract class MagicAttack : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] protected MagicData m_magicData;
    public MagicData MagicData => m_magicData;
    
    [Header("UI")]
    [SerializeField] private GameObject m_magicUIPrefab;
    
    [Header("Debug")]
    [SerializeField] private bool m_infiniteUses;
    
    protected Player m_player;
    protected MagicView m_magicUI;
    protected bool hasUsedMagic;
    
    protected virtual void Start()
    {
        m_player = GetComponent<Player>();
        GameManager.Instance.OnTurnCommited.AddListener(RestoreMagicUse);
        m_magicUI = Instantiate(m_magicUIPrefab, FindAnyObjectByType<Canvas>().transform).GetComponent<MagicView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cast();
        }
    }

    public abstract void Cast();
    
    protected bool CanCast()
    {
        // If the player has already used the magic in this turn...
        if (hasUsedMagic && !m_infiniteUses)
        {
            Debug.Log($"The player can't use <{nameof(MagicAttack)}> as it has already been used in this turn");   
            return false;
        }
        
        // If the player doesn't have enough mana...
        if (m_player.CurrentMana < m_magicData.ManaCost)
        {
            Debug.Log($"There is no enough mana to cast <{nameof(MagicAttack)}> | Player Mana: {m_player.CurrentMana} <-> Cost: {m_magicData.ManaCost}");
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
