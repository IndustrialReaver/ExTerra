using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public Vector2 speed = new Vector2(25, 25);
	
	private Vector2 movement;
	
	//public Vector2 topSpeed = new Vector2(5, 5);
	public float topSpeed = 5.0f;
	
	public int shootInc = 2;
	private int shootTime = 0;

	public GameObject death;
	public bool canRotate = true;
	private int side = 1;
	string guntoshoot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		movement = new Vector2(speed.x * inputX, speed.y * inputY);

		GetComponent<Rigidbody2D>().AddForce(movement);


		if (inputX != 0 && inputY != 0) {
			movement *= 0.707106781f;
		}

		if (GetComponent<Rigidbody2D>().velocity.magnitude <= topSpeed) {
			GetComponent<Rigidbody2D>().velocity *= 0.99f;
		}
		
		if(canRotate){
			var mouse = Input.mousePosition;
			var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
			var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
			var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle-90);
		}
		shootTime++;
		if ((shootTime % shootInc == 0) && Input.GetMouseButton(0)) {
			shoot();
		}
		if ((shootTime % shootInc == 0) && Input.GetMouseButton(1)) {
			mine();
		}
		
		
	}

	private void shoot(){
		if(side > 0) {
			guntoshoot = "Right";
		} else {
			guntoshoot = "Left";
		}
		transform.BroadcastMessage ("Fire", guntoshoot, SendMessageOptions.DontRequireReceiver);
		side *= -1;
	}

	private void mine(){
		transform.BroadcastMessage ("Mine", SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		//stuff
	}
}
