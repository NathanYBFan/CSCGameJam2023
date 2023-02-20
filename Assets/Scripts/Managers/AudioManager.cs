using UnityEngine;
using UnityEngine.Audio;
using NaughtyAttributes;
public class AudioManager : MonoBehaviour
{
    [SerializeField, Required] private AudioMixer audioMixer;
    void Awake()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("masterVolume"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("musicVolume"));
        audioMixer.SetFloat("Player", PlayerPrefs.GetFloat("playerVolume"));
        audioMixer.SetFloat("Enemy", PlayerPrefs.GetFloat("enemyVolume"));
        audioMixer.SetFloat("System", PlayerPrefs.GetFloat("systemVolume"));
    }
}
