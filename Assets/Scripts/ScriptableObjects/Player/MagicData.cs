using UnityEngine;

[CreateAssetMenu(fileName = "NewMagicData", menuName = "Player / Magic Data")]
public class MagicData : ScriptableObject
{
    public string Name;

    public int ManaCost;
    
    public string Description;
}
