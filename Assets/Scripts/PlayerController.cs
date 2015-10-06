using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movement_sensitivity;

	void Update () {
		if(Input.GetButton("Fire1")){
			Vector2 mousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
			Vector2 screenCenter = new Vector2(0.5f, 0.5f);
			Vector2 movementVector = movement_sensitivity * (mousePosition - screenCenter) / 0.5f;

			transform.Translate(movementVector.x, 0.0f, movementVector.y);
		}
	}
}
