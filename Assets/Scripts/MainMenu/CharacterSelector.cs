using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private CharacterData[] m_charactersData;
    
    [Header("Character UI")]
    [SerializeField] private Image m_characterImg;
    [SerializeField] private TMP_Text m_characterName;
    [SerializeField] private TMP_Text m_characterHp;
    [SerializeField] private TMP_Text m_characterArmor;
    [SerializeField] private TMP_Text m_characterMana;
    [SerializeField] private TMP_Text m_characterMagicName;
    [SerializeField] private TMP_Text m_characterMagicCost;
    
    private int m_currentCharacterIndex;

    private void Start()
    {
        UpdateCharacterData();
    }
    
    public void PrevCharacter()
    {
        m_currentCharacterIndex = m_currentCharacterIndex - 1 < 0 ? m_charactersData.Length - 1 : m_currentCharacterIndex - 1;
        UpdateCharacterData();
    }
    
    public void NextCharacter()
    {
        m_currentCharacterIndex = m_currentCharacterIndex + 1 > m_charactersData.Length - 1 ? 0 : m_currentCharacterIndex + 1;
        UpdateCharacterData();
    }

    private void UpdateCharacterData()
    {
        m_characterImg.sprite = m_charactersData[m_currentCharacterIndex].Sprite;
        m_characterName.text = m_charactersData[m_currentCharacterIndex].Name;
        m_characterHp.text = m_charactersData[m_currentCharacterIndex].HealthPoints.ToString();
        m_characterArmor.text = m_charactersData[m_currentCharacterIndex].Armor.ToString();
        m_characterMana.text = m_charactersData[m_currentCharacterIndex].Mana.ToString();
        m_characterMagicName.text = m_charactersData[m_currentCharacterIndex].MagicData.Name;
        m_characterMagicCost.text = m_charactersData[m_currentCharacterIndex].MagicData.ManaCost.ToString();
    }
}
