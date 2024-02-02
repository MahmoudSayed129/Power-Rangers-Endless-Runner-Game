using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Toggle muteToggle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetMuteState(PlayerPrefs.GetInt("MuteState", 0) == 1);
    }

    private void Start()
    {
        muteToggle.onValueChanged.AddListener(HandleMuteToggle);
        muteToggle.isOn = (PlayerPrefs.GetInt("MuteState", 0) == 1);
    }

    private void HandleMuteToggle(bool isMuted)
    {
        SetMuteState(isMuted);
    }

    private void SetMuteState(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0f : 1f; 
        PlayerPrefs.SetInt("MuteState", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
