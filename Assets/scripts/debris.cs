using UnityEngine;
using System.Collections;

public class debris : MonoBehaviour {

    float vel;

	// Use this for initialization
	void Start () {
        this.transform.Rotate(0, Random.Range(0, 360), 0);
        Destroy(gameObject, Random.Range(0.5f, 2.0f));
        vel = Random.Range(0.03f, 0.08f);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(0, 0, vel);
	}
}
