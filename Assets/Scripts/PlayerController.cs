using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jump_height;
	public float offset_theta;
	public float height_offset;

	private bool is_grounded;
	private float cosTheta;
	private float sinTheta;
	private Vector3 velocity;

	void Start(){
		// Precalculate the cos and sin of the world offset
		float theta_rads = offset_theta * Mathf.PI / 180.0f;
		cosTheta = Mathf.Cos(theta_rads);
		sinTheta = Mathf.Sin(theta_rads);

		// We start on the ground
		is_grounded = true;
	}

	void FixedUpdate(){

		if(is_grounded){
			bool shouldZeroVelocity = true;

			if(Input.GetButton("Fire2")){
				HandleMouseInput();
				shouldZeroVelocity = false;
			}

			if(Input.GetKeyDown("space")) {
				HandleSpacePressed();
				shouldZeroVelocity = false;
			}

			if(shouldZeroVelocity){
				ZeroVelocity();
			}
		} else {
			FallDownward();
		}

		transform.position += velocity;
		correctHeight();

	}

	Vector2 MouseVectorToWorld(){
		// Convert mouseposition into vector originating from 1/2 width, 1/2 height
		// With bounds -1.0 < x < 1.0 and -1.0 < y < 1.0
		Vector2 screenMovement = new Vector2(
			((Input.mousePosition.x / Screen.width) - 0.5f) / 0.5f,
			((Input.mousePosition.y / Screen.height) - 0.5f) / 0.5f
		);

		// Transform this vector into world coordinates using the rotation from the orthographcis projection
		Vector3 worldMovement = new Vector2(
			(screenMovement.x * cosTheta) + (screenMovement.y * sinTheta),
			(screenMovement.x * -1.0f * sinTheta) + (screenMovement.y * cosTheta)
		);

		// Make it so the maximum (1.0f * speed) is reached at 0.5 rather than at 1.0
		Vector2 NormalizedMovement = new Vector2(
			Mathf.Max(Mathf.Min(worldMovement.x * 2, 1.0f), -1.0f),
			Mathf.Max(Mathf.Min(worldMovement.y * 2, 1.0f), -1.0f)
		);

		return NormalizedMovement;

	}

	void HandleMouseInput(){
		// Get the top-down (2D) velocity vector
		Vector2 velocity2D = MouseVectorToWorld() * speed;

		Vector3 temp_position = new Vector3(
			velocity2D.x + transform.position.x,
			transform.position.y,
			velocity2D.y + transform.position.z
		);

		// Cast a ray downward from where we're going to be
		RaycastHit hit;
		if(Physics.Raycast(temp_position, Vector3.down, out hit, height_offset + 0.3f)){
			// We've hit the ground! Clamp to it
			is_grounded = true;

			// TODO change the x/z velocities to be scalar of the amount traveled upward
			velocity = new Vector3(
				velocity2D.x,
				hit.point.y - transform.position.y + height_offset,
				velocity2D.y
			);
		} else {
			is_grounded = false;
		}
	}

	void ZeroVelocity(){
		velocity = new Vector3(0.0f, 0.0f, 0.0f);
	}

	void FallDownward(){
		// Fall downward in addition to any of the other current velocities
		velocity = new Vector3(velocity.x, velocity.y - 0.01f, velocity.z);

		UpdateIsGrounded();
	}

	void HandleSpacePressed(){
		// Jump upward!
		velocity = new Vector3(velocity.x, jump_height, velocity.z);

		UpdateIsGrounded();
	}

	void UpdateIsGrounded(){
		// Detect if we're going to hit the ground next step
		is_grounded = Physics.Raycast(transform.position, Vector3.down, (velocity.y * -1.0f) + height_offset);
	}

	void correctHeight(){
		// Fixes slight clipping bug player has after falling
		RaycastHit hit;

		if(Physics.Raycast(transform.position, Vector3.down, out hit, height_offset + 0.3f) && velocity.y < 0){
			transform.position = new Vector3(
				transform.position.x,
				hit.point.y + height_offset,
				transform.position.z
			);
		}
	}

}
