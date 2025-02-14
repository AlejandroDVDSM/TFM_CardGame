using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

public class TestCard : Card
{
    protected override void ApplyEffect()
    {
        
        Debug.Log("TestCard");
    }
}
