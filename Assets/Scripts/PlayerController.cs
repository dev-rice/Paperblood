using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float offset_theta;
	public float height_offset;

	private float cosTheta;
	private float sinTheta;
	private Rigidbody rb;

	void Start(){
		// Precalculate the cos and sin of the world offset
		float theta_rads = offset_theta * Mathf.PI / 180.0f;
		cosTheta = Mathf.Cos(theta_rads);
		sinTheta = Mathf.Sin(theta_rads);

		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate(){

		if(Input.GetButton("Fire2")){
			Vector2 screenMovement = new Vector2(
				((Input.mousePosition.x / Screen.width) - 0.5f) / 0.5f,
				((Input.mousePosition.y / Screen.height) - 0.5f) / 0.5f
			);

			Vector3 worldMovement = new Vector3(
				(screenMovement.x * cosTheta) + (screenMovement.y * sinTheta),
				0.0f,
				(screenMovement.x * -1.0f * sinTheta) + (screenMovement.y * cosTheta)
			);

			transform.position += worldMovement * speed;
		}

	}

}
