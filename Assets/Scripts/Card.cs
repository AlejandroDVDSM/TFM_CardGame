using UnityEngine;

public abstract class Card: MonoBehaviour
{
    public bool IsInPool { get; set; }

    void Start()
    {
        LogCard();
    }


    protected virtual void LogCard()
    {
        Debug.Log("Card Log" + gameObject.name);
    }
    
    protected abstract void ApplyEffect();

    public void SelectCard()
    {
        ApplyEffect();
        
    }
    
    
}
