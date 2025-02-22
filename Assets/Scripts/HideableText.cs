using System;
using TMPro;
using UnityEngine;

public class HideableText : MonoBehaviour
{
    private TMP_Text m_text;

    private string m_unhiddenValue;

    private bool m_isValueHidden;
    
    private void Start()
    {
        m_text = GetComponent<TMP_Text>();
        m_unhiddenValue = m_text.text;
    }

    // private void Update()
    // {
    //     if (GameManager.Instance.Player.Status.IsBlind > 0)
    //     {
    //         Debug.Log("BLIND");
    //         m_text.text = "?";
    //     }
    // }
}
