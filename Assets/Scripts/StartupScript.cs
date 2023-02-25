using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("StartupSetup") == 0)
        {
            PlayerPrefs.SetFloat("masterVolume", 1f);
            PlayerPrefs.SetFloat("musicVolume", 1f);
            PlayerPrefs.SetFloat("playerVolume", 1f);
            PlayerPrefs.SetFloat("enemyVolume", 1f);
            PlayerPrefs.SetFloat("systemVolume", 1f);
            PlayerPrefs.SetInt("StartupSetup", 1);
        }
    }
}
