using UnityEngine;
using NaughtyAttributes;

public class DoorHandler : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioPlayer;
    [Header("AudioClips")]
    [SerializeField] private AudioClip [] doorOpenAudio;
    [SerializeField] private AudioClip [] doorLockedAudio;

    // Start is called before the first frame update
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Script check if has key
        {
            if (collision.GetComponent<Collider2D>())
            {
                audioPlayer.clip = doorOpenAudio[Random.Range(0, doorOpenAudio.Length)];
                audioPlayer.Play();
                anim.SetBool("IsOpen", true);
            }
            else
            {
                audioPlayer.clip = doorLockedAudio[Random.Range(0, doorLockedAudio.Length)];
                audioPlayer.Play();
            }
        }
    }
}
