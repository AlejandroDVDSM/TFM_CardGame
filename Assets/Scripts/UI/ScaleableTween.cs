using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleableTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool m_loop;

    private void Start()
    {
        if (m_loop)
        {
            transform.DOScale(Vector3.one * 1.2f, 1.2f)
                .SetEase(Ease.OutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_loop)
        {
            transform.DOScale(Vector3.one * 1.1f, 0.2f)
                .SetEase(Ease.OutSine);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_loop)
        {
            transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.InSine);
        }
    }
    
    
}
