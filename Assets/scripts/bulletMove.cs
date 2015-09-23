using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {

	public float speed;
	Vector2 vectorMove;
	int timetolive = 50;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		timetolive--;
		this.transform.Translate( Vector2.up * speed * Time.deltaTime );

		if ((!this.GetComponent<Renderer>().isVisible) && (timetolive < 0)) {
						Destroy (this.gameObject);
				}

	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        coll.gameObject.SendMessage("ApplyDamage", 10, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
    }

}
