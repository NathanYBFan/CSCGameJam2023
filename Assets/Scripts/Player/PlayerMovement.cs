using UnityEngine;
using NaughtyAttributes;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField, Required] private Rigidbody2D m_Rigidbody2D;
	[SerializeField, Required] private Animator playerAnimator;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField, Required] private AudioSource audioPlayer;
	[SerializeField, Required] private AudioClip footsteps;
	private bool m_FacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;
	public bool canMove = true;

    private void Start()
    {
		audioPlayer.clip = footsteps;
    }

    public void Move(Vector2 move)
	{
		if (!canMove)
        {
			m_Rigidbody2D.velocity = Vector3.zero;
			return;
		}

		if (move == Vector2.zero)
        {
			SetAnimatorBool("IsWalking", false);
			audioPlayer.Stop();
		}
        else
        {
			SetAnimatorBool("IsWalking", true);
			audioPlayer.Play();
		}

		Vector3 targetVelocity = new Vector2(move.x * 10f, move.y * 10f);
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		if (move.x > 0)
		{
			if (!m_FacingRight)
				Flip();
			SetAnimatorInt("FacingDirection", 1);
		}
		else if (move.x < 0)
		{
			if (m_FacingRight)
				Flip();
			SetAnimatorInt("FacingDirection", 3);
		}

		if (move.y > 0)
			SetAnimatorInt("FacingDirection", 0);
		else if (move.y < 0)
			SetAnimatorInt("FacingDirection", 2);
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void SetAnimatorBool(string variableName, bool variableValue)
    {
		playerAnimator.SetBool(variableName, variableValue);
	}

	public void SetAnimatorInt(string variableName, int variableValue)
	{
		playerAnimator.SetInteger(variableName, variableValue);
	}
}
