using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movement_sensitivity;

	public float offset_theta;

	void Update(){
		if(Input.GetButton("Fire1")){
			Vector2 screenMovement = new Vector2(
				(Input.mousePosition.x / Screen.width - 0.5f) / 0.5f,
				(Input.mousePosition.y / Screen.height - 0.5f) / 0.5f
			);
			screenMovement *= movement_sensitivity;

			// Clusterfuck right now, and also doesn't work
			Vector3 gameMovement = new Vector3(
				screenMovement.x * Mathf.Cos(offset_theta * Mathf.PI / 180.0f) + screenMovement.y * Mathf.Sin(offset_theta * Mathf.PI / 180.0f),
				0.0f,
				screenMovement.x * Mathf.Sin(offset_theta * Mathf.PI / 180.0f) + screenMovement.y * Mathf.Cos(offset_theta * Mathf.PI / 180.0f)
			);

			transform.position += gameMovement;
		}
	}
}
