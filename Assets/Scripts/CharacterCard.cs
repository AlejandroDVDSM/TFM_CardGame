using System;
using CardGame.Enums;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnCardSelected.AddListener(OnCardSelected);
    }

    private void OnCardSelected(Card selectedCard)
    {
        PlaceCharacterInPosition(selectedCard.CardLane);
    }
    
    private void PlaceCharacterInPosition(ECardLane cardLane)
    {
        Vector3 newPos;
        switch (cardLane)
        {
            case ECardLane.LEFT:
                newPos = new Vector3(-160, -257, 0); // TODO: replace hardcoded values
                transform.SetLocalPositionAndRotation(newPos, Quaternion.identity);
                break;
            
            case ECardLane.MIDDLE:
                newPos = new Vector3(0, -257, 0); // TODO: replace hardcoded values
                transform.SetLocalPositionAndRotation(newPos, Quaternion.identity);
                
                break;
            
            case ECardLane.RIGHT:
                newPos = new Vector3(160, -257, 0); // TODO: replace hardcoded values
                transform.SetLocalPositionAndRotation(newPos, Quaternion.identity);
                
                break;
        }
    }

}
