using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform m_landingPage;
    [SerializeField] private Transform m_characterSelector;

    [Header("Debug")] 
    [SerializeField] private bool m_characterSelectorAtStart;
    
    private void Start()
    {
        // !--- DEBUG ---!
        if (m_characterSelectorAtStart)
        {
            ShowCharacterSelector();
        }
        else
        {
            m_landingPage.gameObject.SetActive(true);
            m_characterSelector.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Hide the landing page and show the character selection panel
    /// </summary>
    public void ShowCharacterSelector()
    {
        m_landingPage.gameObject.SetActive(false);
        m_characterSelector.gameObject.SetActive(true);
    }

    /// <summary>
    /// Load the game scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
