using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [Header("Character Data")]
    [SerializeField] private CharacterData[] m_charactersData;
    
    [Header("Character UI")]
    [SerializeField] private Image m_characterImg;
    [SerializeField] private TMP_Text m_characterName;
    
    [Header("Character UI - Stats")]
    [SerializeField] private TMP_Text m_characterHp;
    [SerializeField] private TMP_Text m_characterArmor;
    [SerializeField] private TMP_Text m_characterMana;
    
    [Header("Character UI - Magic")]
    [SerializeField] private TMP_Text m_characterMagicName;
    [SerializeField] private TMP_Text m_characterMagicDesc;
    [SerializeField] private Image m_characterMagicIcon;
    [SerializeField] private TMP_Text m_characterMagicCost;
    
    private int m_currentCharacterIndex;

    private void Start()
    {
        UpdateCharacterData();
    }
    
    /// <summary>
    /// Display the previous character in the list
    /// </summary>
    public void PrevCharacter()
    {
        m_currentCharacterIndex = m_currentCharacterIndex - 1 < 0 ? m_charactersData.Length - 1 : m_currentCharacterIndex - 1;
        UpdateCharacterData();
    }
    
    /// <summary>
    /// Display the next character in the list
    /// </summary>
    public void NextCharacter()
    {
        m_currentCharacterIndex = m_currentCharacterIndex + 1 > m_charactersData.Length - 1 ? 0 : m_currentCharacterIndex + 1;
        UpdateCharacterData();
    }

    /// <summary>
    /// Update the character data according to the current character index
    /// </summary>
    private void UpdateCharacterData()
    {
        m_characterImg.sprite = m_charactersData[m_currentCharacterIndex].Sprite;
        m_characterName.text = m_charactersData[m_currentCharacterIndex].Name;
        
        // Stats
        m_characterHp.text = m_charactersData[m_currentCharacterIndex].HealthPoints.ToString();
        m_characterArmor.text = m_charactersData[m_currentCharacterIndex].Armor.ToString();
        m_characterMana.text = m_charactersData[m_currentCharacterIndex].Mana.ToString();
        
        // Magic
        m_characterMagicName.text = m_charactersData[m_currentCharacterIndex].MagicData.Name;
        m_characterMagicDesc.text = m_charactersData[m_currentCharacterIndex].MagicData.Description;
        m_characterMagicIcon.sprite = m_charactersData[m_currentCharacterIndex].MagicData.Icon;
        m_characterMagicCost.text = m_charactersData[m_currentCharacterIndex].MagicData.ManaCost.ToString();
    }

    /// <summary>
    /// Set the character that the player will use during a game
    /// </summary>
    public void SelectCharacter()
    {
        PlayerPrefs.SetString("Character", m_charactersData[m_currentCharacterIndex].Name);
    }
}
