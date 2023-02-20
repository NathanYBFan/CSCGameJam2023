using UnityEngine;
using NaughtyAttributes;

public class SingletonStrapper : MonoBehaviour
{
    [SerializeField, Required] GameObject gameManagerPrefab;
    private void Awake()
    {
        if (GameManager.gameManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instantiate(gameManagerPrefab);
        }
        Destroy(this.gameObject);
    }
}
