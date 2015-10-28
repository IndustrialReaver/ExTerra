using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public GameObject player = null;
	private float newx;
	private float newy;

	// Update is called once per frame
	void FixedUpdate () {
        if (player != null)
        {
            newx = player.transform.position.x;
            newy = player.transform.position.y;
            transform.position = new Vector3(newx, newy, transform.position.z);//Vector3.Lerp(transform.position, new Vector3(newx, newy, transform.position.z), 2f*Time.smoothDeltaTime);
        }
	}
}
