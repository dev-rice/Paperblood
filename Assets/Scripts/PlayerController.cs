using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
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
			if(Input.GetButton("Fire2")){
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

			} else {
				// Stop since we're on the ground
				velocity = new Vector3(0.0f, 0.0f, 0.0f);
			}
		} else {
			// Fall downward in addition to any of the other velocities
			velocity = new Vector3(velocity.x, velocity.y - 0.01f, velocity.z);

			// Detect if we're going to hit the ground next step
			is_grounded = Physics.Raycast(transform.position, Vector3.down, (velocity.y * -1.0f) + height_offset);
		}

		transform.position += velocity;

	}

	Vector2 MouseVectorToWorld(){
		Vector2 screenMovement = new Vector2(
			((Input.mousePosition.x / Screen.width) - 0.5f) / 0.5f,
			((Input.mousePosition.y / Screen.height) - 0.5f) / 0.5f
		);

		Vector3 worldMovement = new Vector2(
			(screenMovement.x * cosTheta) + (screenMovement.y * sinTheta),
			(screenMovement.x * -1.0f * sinTheta) + (screenMovement.y * cosTheta)
		);

		return worldMovement.normalized;
	}

}
