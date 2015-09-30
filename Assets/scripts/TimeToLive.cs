using UnityEngine;
using System.Collections;

public class TimeToLive : MonoBehaviour {

    public int FramesToLive = 20;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        FramesToLive--;
        if (FramesToLive <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
