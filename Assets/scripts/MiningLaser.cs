using UnityEngine;
using System.Collections;

public class MiningLaser : MonoBehaviour {

    int timer = 2;
    LineRenderer line;

    public Color SM;
    public Color FM;
    public Color SP;
    public Color FP;

    public bool canRotate = false;
    public AudioClip pewpew;
    AudioSource playpew;
    
    // Use this for initialization
    void Start () {
        line = transform.GetComponent<LineRenderer>();
        playpew = gameObject.AddComponent<AudioSource>() as AudioSource;
        playpew.clip = pewpew;
    }
	
	// Update is called once per frame
	void Update () {

        if (line.enabled == true)
        {
            if (timer < 0)
            {
                line.enabled = false;
                playpew.Stop();
            }
            else
            {
                timer--;
            }
        }
        if (canRotate)
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

    }

	public void Mine(){
        line.enabled = true;
        timer = 4;
		line.SetColors(SM, FM);
        line.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        line.SetPosition(1, transform.GetChild(0).transform.position);
        playpew.Play();
		Debug.Log("MiningLaser::Mine -- mining... ");
	}


    public void Place()
    {
        line.enabled = true;
        timer = 4;
        line.SetColors(SP, FP);
        line.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        line.SetPosition(1, transform.GetChild(0).transform.position);
        playpew.Play();
        Debug.Log("MiningLaser::Place -- placing... ");
    }
}
