using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Audio Settings")] 
    [SerializeField] private Image m_audioBtnImg;

    [SerializeField] private Sprite m_audioOnSprite;
    [SerializeField] private Sprite m_audioOffSprite;
    
    public void ToggleAudio()
    {
        AudioManager audioManager = AudioManager.Instance;
        
        audioManager.MuteAudio(!audioManager.IsAudioMuted());
        
        m_audioBtnImg.sprite = audioManager.IsAudioMuted() ? m_audioOffSprite : m_audioOnSprite;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
