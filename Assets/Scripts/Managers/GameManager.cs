using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Initializations")]
    [SerializeField] private string settingsMenuSceneName;
    public static GameManager gameManager { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
