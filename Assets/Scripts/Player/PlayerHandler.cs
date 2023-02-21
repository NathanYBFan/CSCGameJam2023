using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler _PlayerHandlerInstance { get; private set; }
    [SerializeField, Required] public PlayerMovement _playerMovement;
    [SerializeField, Required] public PlayerInput _playerInput;

    [SerializeField] private int playerMaxLives;
    [ShowNonSerializedField] private int playerLivesLeft;

    [ShowNonSerializedField] private Vector3 recentSpawnPoint;
    [SerializeField] private string gameOverSceneName;
    [ShowNonSerializedField] private int numberOfBlueKeys;
    [ShowNonSerializedField] private int numberOfOrangeKeys;

    // Start is called before the first frame update
    void Awake()
    {
        if (_PlayerHandlerInstance != null && _PlayerHandlerInstance != this)
            Destroy(this);
        else
            _PlayerHandlerInstance = this;

        recentSpawnPoint = transform.position;
        playerLivesLeft = playerMaxLives;
    }
    public void PlayerIsDead()
    {
        playerLivesLeft--;
        _playerMovement.SetAnimatorBool("IsDead", true);
        _playerMovement.canMove = false;

        if (playerLivesLeft <= 0)
            PermaDeath();
        else
            StartCoroutine(ResetLevel());
    }

    private IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        transform.position = recentSpawnPoint;
        _playerMovement.canMove = true;
        _playerMovement.SetAnimatorBool("IsDead", false);
    }

    private void PermaDeath()
    {
        SceneManager.LoadScene(gameOverSceneName, LoadSceneMode.Single);
    }

    private void SetSpawnPoint(Transform position) { recentSpawnPoint = position.position; } // Spawn Point Setter

    public void SetBlueKeys(int blueKeysToAdd) { numberOfBlueKeys += blueKeysToAdd; } // Blue Setter

    public int GetNumbBlueKeys() { return numberOfBlueKeys; } // Blue Getter

    public void SetOrangeKeys( int orangeKeysToAdd) { numberOfOrangeKeys += orangeKeysToAdd; } // Orange Setter
    public int GetNumbOrangeKeys() { return numberOfOrangeKeys; } // Orange Getter

}
