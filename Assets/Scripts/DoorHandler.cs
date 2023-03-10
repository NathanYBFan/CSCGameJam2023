using UnityEngine;
using NaughtyAttributes;

public class DoorHandler : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioPlayer;
    [Header("AudioClips")]
    [SerializeField] private AudioClip [] doorOpenAudio;
    [SerializeField] private AudioClip [] doorLockedAudio;
    [Header("Colliders")]
    [SerializeField] private BoxCollider2D triggerCollider;
    [SerializeField] private GameObject boxCollider2D;
    [SerializeField] private bool isBlueDoor;
    [SerializeField] private bool isSideDoor;
    [SerializeField] private int keysRequiredToOpen;
    [SerializeField] private float timeForTimer;

    // Start is called before the first frame update
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        triggerCollider.enabled = true;
        boxCollider2D.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Script check if has key
        {
            if (isBlueDoor)
            {
                if (keysRequiredToOpen <= PlayerHandler._PlayerHandlerInstance.GetNumbBlueKeys()) // Open Door
                {
                    audioPlayer.clip = doorOpenAudio[Random.Range(0, doorOpenAudio.Length - 1)];
                    audioPlayer.Play();
                    anim.SetBool("IsOpen", true);
                    if (LevelTimer._LevelTimerInstance.isActiveAndEnabled)
                        LevelTimer._LevelTimerInstance.StopLevelTimer();
                    PlayerHandler._PlayerHandlerInstance.SetBlueKeys(-keysRequiredToOpen);
                    triggerCollider.enabled = false;
                    boxCollider2D.SetActive(false);
                }
                else
                {
                    audioPlayer.clip = doorLockedAudio[Random.Range(0, doorLockedAudio.Length - 1)];
                    audioPlayer.Play();
                }
            }
            else
            {
                if (keysRequiredToOpen <= PlayerHandler._PlayerHandlerInstance.GetNumbOrangeKeys()) // Open door
                {
                    audioPlayer.clip = doorOpenAudio[Random.Range(0, doorOpenAudio.Length - 1)];
                    audioPlayer.Play();
                    anim.SetBool("IsOpen", true);
                    LevelTimer._LevelTimerInstance.StartLevelTimer(timeForTimer);
                    PlayerHandler._PlayerHandlerInstance.SetOrangeKeys(-keysRequiredToOpen);
                    triggerCollider.enabled = false;
                    boxCollider2D.SetActive(false);
                }
                else
                {
                    audioPlayer.clip = doorLockedAudio[Random.Range(0, doorLockedAudio.Length - 1)];
                    audioPlayer.Play();
                }
            }
        }
    }
}
