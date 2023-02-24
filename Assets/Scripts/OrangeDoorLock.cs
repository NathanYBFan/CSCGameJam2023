using UnityEngine;

public class OrangeDoorLock : MonoBehaviour
{
    [SerializeField] private GameObject doorBlock;
    [SerializeField] private GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorBlock.SetActive(true);
            door.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}
