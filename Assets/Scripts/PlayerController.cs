using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movement_sensitivity;
	public float offset_theta;

	private float cosTheta;
	private float sinTheta;

	void Start(){
		float theta_rads = offset_theta * Mathf.PI / 180.0f;
		cosTheta = Mathf.Cos(theta_rads);
		sinTheta = Mathf.Sin(theta_rads);
	}

	void Update(){
		// Left mouse button
		if(Input.GetButton("Fire2")){
			Vector2 movementVector = new Vector2(
				((Input.mousePosition.x / Screen.width) - 0.5f) / 0.5f,
				((Input.mousePosition.y / Screen.height) - 0.5f) / 0.5f
			);

			Vector3 something = new Vector3(
				(movementVector.x * cosTheta) + (movementVector.y * sinTheta),
				0.0f,
				(movementVector.x * -1.0f * sinTheta) + (movementVector.y * cosTheta)
			);

			transform.position += something;
		}
	}
}
