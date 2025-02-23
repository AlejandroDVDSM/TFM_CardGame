using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Player /Player Data")]
public class PlayerData : ScriptableObject
{
    public string Name;
    
    public Sprite Sprite;

    public int HealthPoints;

    public int Armor; 
        
    public int Mana;
}
