using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{

    [SerializeField] private float levelTime = 30;
    [SerializeField] private Slider timerBar;
    [SerializeField] private GameObject player;

    private float timeStart;

    // Start is called before the first frame update
    void Start()
    {
        timeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimesUp())
        {
            KillPlayer();
            ResetTime();
            StopLevelTimer();
        }
        else
        {
            float timeLeft = Time.time - timeStart;
            float barPercent = timeLeft / levelTime;
            UpdateBar(1 - barPercent);
        }
    }

    private void UpdateBar(float percent)
    {
        timerBar.value = percent;
    }
    
    private bool TimesUp()
    {
        return Time.time - timeStart >= levelTime;
    }

    private void KillPlayer()
    {
        //add code to kill the player here, not sure if there's a public method that does that.
    }

    private void ResetTime()
    {
        timeStart = Time.time;
    }

    public void StopLevelTimer()
    {
        Image[] imgs;
        imgs = timerBar.GetComponentsInChildren<Image>();
        foreach(Image img in imgs)
        {
            img.enabled = false;
        }

        timerBar.enabled = false;
        this.enabled = false;
    }
    
}
