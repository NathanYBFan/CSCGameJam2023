using UnityEngine;

public class KeyPickupHandler : MonoBehaviour
{
    private AudioSource audioPlayer;
    [SerializeField] private AnimationCurve myCurve;
    [SerializeField] private bool isBlueKey;
    [SerializeField] private AudioClip[] keysGetClips;

    private void Awake()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioPlayer.clip = keysGetClips[Random.Range(0, keysGetClips.Length)];
            audioPlayer.Play();
            if (isBlueKey)
            {

            }
            else // Is orange key
            {

            }
            // Give player key 
        }
    }


}
