using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform m_landingPage;
    [SerializeField] private Transform m_characterSelector;

    [Header("Debug")] 
    [SerializeField] private bool m_characterSelectorAtStart;
    
    private void Start()
    {
        if (m_characterSelectorAtStart)
        {
            m_landingPage.gameObject.SetActive(false);
            m_characterSelector.gameObject.SetActive(true);
        }
        else
        {
            m_landingPage.gameObject.SetActive(true);
            m_characterSelector.gameObject.SetActive(false);
        }
    }

    public void ShowCharacterSelector()
    {
        m_landingPage.gameObject.SetActive(false);
        m_characterSelector.gameObject.SetActive(true);
    }
}
