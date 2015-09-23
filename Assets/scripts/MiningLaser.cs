using UnityEngine;
using System.Collections;

public class MiningLaser : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Mine(){
		Physics2D.Linecast ((Vector2)transform.position, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
		Debug.DrawLine (transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		Debug.Log("MiningLaser::Mine -- mining... ");
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawLine (transform.position, Input.mousePosition);
	}
}
