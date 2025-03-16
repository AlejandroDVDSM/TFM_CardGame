using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Player / Character Data")]
public class CharacterData : ScriptableObject
{
    public string Name;
    
    public Sprite Sprite;

    public int HealthPoints;

    public int Armor; 
        
    public int Mana;

    public MagicData MagicData;
}
