using UnityEngine;
using TMPro;
using NaughtyAttributes;
public class VideoSettings : MonoBehaviour
{
    [Header("FPS Initializations")]
    [SerializeField, Required] private TMP_InputField fpsCap;
    [Header("Vsync Initializations")]
    [SerializeField, Required] private GameObject VSyncCheckmark;
    private bool VSyncOn;

    // Start is called before the first frame update
    void Awake()
    {
        WindowStateChanged(PlayerPrefs.GetInt("windowState"));
        ResolutionChanged(PlayerPrefs.GetInt("resolution"));
        fpsCap.text = PlayerPrefs.GetInt("fpsCap").ToString();
        if (PlayerPrefs.GetInt("vSync") == 0)
        {
            VSyncOn = false;
            VSyncCheckmark.SetActive(VSyncOn);
        }
        else
        {
            VSyncOn = true;
            VSyncCheckmark.SetActive(VSyncOn);
        }
    }

    public void WindowStateChanged(int value)
    {
        Screen.fullScreenMode = value switch
        {
            0 => // Borderless
                FullScreenMode.FullScreenWindow,
            1 => // Windowed
                FullScreenMode.Windowed,
            2 => // Fullscreen
                FullScreenMode.ExclusiveFullScreen,
            _ => Screen.fullScreenMode
        };
        PlayerPrefs.SetInt("windowState", value); // Save the value to the player prefs
    }

    public void ResolutionChanged(int value)
    {
        // We need to check if the window state is fullscreen, as we need to pass that to the Screen.SetResolution method
        bool fullscreen = PlayerPrefs.GetInt("windowState", 0) == 2;
        PlayerPrefs.SetInt("resolution", value);
        switch (value)
        {
            case 0: // 2560x1440
                Screen.SetResolution(2560, 1440, fullscreen);
                break;
            case 1: // 1920x1080
                Screen.SetResolution(1920, 1080, fullscreen);
                break;
            case 2: // 1280x720
                Screen.SetResolution(1280, 720, fullscreen);
                break;
        }
    }

    public void FPSCapChanged()
    {
        Application.targetFrameRate = int.Parse(fpsCap.text);
        PlayerPrefs.SetInt("fpsCap", int.Parse(fpsCap.text));
    }

    public void VSyncChanged()
    {
        if (PlayerPrefs.GetInt("vSync") == 1) // Turn VSync off
        {
            QualitySettings.vSyncCount = 0; // VSync turned off
            VSyncCheckmark.SetActive(false);
            PlayerPrefs.SetInt("vSync", 0);
        }
        else // Turn VSync on
        {
            QualitySettings.vSyncCount = 1; // Vsync On
            VSyncCheckmark.SetActive(true);
            PlayerPrefs.SetInt("vSync", 1);
        }
        VSyncOn = !VSyncOn;
    }
}
