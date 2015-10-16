using UnityEngine;
using System.Collections;

public class MiningLaser : MonoBehaviour {

    int timer = 2;
    LineRenderer line;

    public Color SM;
    public Color FM;
    public Color SP;
    public Color FP;

	// Use this for initialization
	void Start () {
        line = transform.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (line.enabled == true)
        {
            if (timer < 0)
            {
                line.enabled = false;
            }
            else
            {
                timer--;
            }
        }
        
	}

	public void Mine(){
        line.enabled = true;
        timer = 4;
		//Physics2D.Linecast ((Vector2)transform.position, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        line.SetColors(SM, FM);
        line.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        line.SetPosition(1, transform.position);
		//Debug.DrawLine (transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		Debug.Log("MiningLaser::Mine -- mining... ");
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawLine (transform.position, Input.mousePosition);
	}
    public void Place()
    {
        line.enabled = true;
        timer = 4;
        //Physics2D.Linecast ((Vector2)transform.position, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        line.SetColors(SP, FP);
        line.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        line.SetPosition(1, transform.position);
        //Debug.DrawLine (transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log("MiningLaser::Mine -- mining... ");
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine (transform.position, Input.mousePosition);
    }
}
