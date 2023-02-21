using UnityEngine;

public class KeyPickupHandler : MonoBehaviour
{
    private AudioSource audioPlayer;
    [SerializeField] private AnimationCurve myCurve;
    [SerializeField] private float maxRotation;
    [SerializeField] private float speedValue;
    [SerializeField] private bool isBlueKey;
    [SerializeField] private AudioClip[] keysGetClips;

    private void Awake()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, maxRotation * Mathf.Sin(Time.time * speedValue));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            AudioClip temp = keysGetClips[Random.Range(0, keysGetClips.Length)];
            audioPlayer.PlayOneShot(temp);
            if (isBlueKey)
            {
                PlayerHandler._PlayerHandlerInstance.SetBlueKeys(1);
            }
            else // Is orange key
            {
                PlayerHandler._PlayerHandlerInstance.SetOrangeKeys(1);
            }
            Destroy(this.gameObject, temp.length);
        }
    }


}
