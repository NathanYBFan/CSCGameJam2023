using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] string pauseScene;
    private Vector2 movement;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync(pauseScene);
        }
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
    }
    void FixedUpdate()
    {
        PlayerHandler._PlayerHandlerInstance._playerMovement.Move(movement * Time.fixedDeltaTime);
    }
}
