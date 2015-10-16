using UnityEngine;
using System.Collections;

public class MMship : MonoBehaviour {

    public float speed = 0.01f;

    private bool screenoffset = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(speed * Time.deltaTime, 0, 0);

        Vector2 pos = transform.position;

        if ((!this.GetComponent<Renderer>().isVisible) && screenoffset)
        {
            pos.x *= -1;
            transform.position = pos;
            screenoffset = false;
        }

        if (this.GetComponent<Renderer>().isVisible)
        {
            screenoffset = true;
        }

	}
}
