using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Vector2 movement;

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
    }
    void FixedUpdate()
    {
        PlayerHandler._PlayerHandlerInstance._playerMovement.Move(movement * Time.fixedDeltaTime);
    }
}
