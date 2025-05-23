﻿using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using DG.Tweening;
using UnityEngine;

public class CardRow : MonoBehaviour
{
    [SerializeField] private ERow row;
    
    [Min(0)]
    [SerializeField] private float offsetX;

    /// <summary>
    /// Populate the row with random cards. If cards are given, populate it with the given cards
    /// </summary>
    /// <param name="cards">Cards to populate the row with</param>
    public void PopulateRow(List<Card> cards = null)
    {
        if (cards == null)
        {
            cards = GameManager.Instance.CardPool.ExtractRangeFromPool();
        }
        
        for (int i = 0; i < cards.Count; i++)
        {
            PlaceSingleCard(cards[i], i);
        }
    }
    
    public List<Card> GetCards()
    {
        return GetComponentsInChildren<Card>().ToList();
    }

    public void PlaceSingleCard(Card card, int lane, int siblingIndex = -1)
    {
        Vector3 pos = Vector3.zero;
        card.transform.SetParent(transform);

        if (siblingIndex > 0)
        {
            card.gameObject.transform.SetSiblingIndex(siblingIndex);
        }
        
        card.SetLaneAndRow(lane, row);
        
        pos.x = offsetX * lane;
        card.transform.localScale = Vector3.one;
        card.transform.DOLocalMove(pos, 0.2f).SetEase(Ease.OutSine);
        // card.transform.localPosition = pos;
    }
}