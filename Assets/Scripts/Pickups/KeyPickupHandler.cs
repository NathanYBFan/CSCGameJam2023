using UnityEngine;

public class KeyPickupHandler : MonoBehaviour
{
    public AnimationCurve myCurve;
    [SerializeField] private bool isBlueKey;
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
