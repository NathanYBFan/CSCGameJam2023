using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelTimer : MonoBehaviour
{
    public static LevelTimer _LevelTimerInstance { get; private set; }
    [SerializeField] private float levelTime = 30;
    [SerializeField] private TextMeshProUGUI timeText;
    private Slider timerBar;

    private float timeStart;

    // Start is called before the first frame update
    void Start()
    {
        timeStart = Time.time;
        timerBar = GetComponent<Slider>();

        if (_LevelTimerInstance != null && _LevelTimerInstance != this)
            Destroy(this);
        else
            _LevelTimerInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimesUp())
        {
            KillPlayer();
            ResetTime();
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
        PlayerHandler._PlayerHandlerInstance.PlayerIsDead();
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
        timeText.enabled = false;
        timerBar.enabled = false;
        this.enabled = false;
    }

    public void StartLevelTimer (float timeToStart)
    {
        Image[] imgs;
        imgs = timerBar.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            img.enabled = true;
        }
        timeText.enabled = true;
        timerBar.enabled = true;
        this.enabled = true;
        levelTime = timeToStart;
        ResetTime();
    }
}
