using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public GameObject player;
	private float newx;
	private float newy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		newx = player.transform.position.x;
		newy = player.transform.position.y;
		transform.position = new Vector3(newx, newy, transform.position.z);
	}
}
