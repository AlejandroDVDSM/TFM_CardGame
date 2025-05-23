﻿using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBaseCard", menuName = "Card / Base Card Data")]
    public class BaseCardData : ScriptableObject
    {
        [Header("Base Card Data")]
        public string Name;
        
        public Sprite Sprite;
        
        public Color BackgroundColor = Color.white;
        
        public int MinValue;
        
        public int MaxValue;

        [Range(0, 100)]        
        public float Weight;
        
        public bool IgnoreData;

    }
}