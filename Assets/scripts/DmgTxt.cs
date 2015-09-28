using UnityEngine;
using System.Collections;

public class DmgTxt : MonoBehaviour {

    public Color color = new Color(0.8f,0.8f,0,1.0f);
    public float scroll = 0.05f;  
    public float duration = 1.5f; 
    public float alpha;

    // Use this for initialization
    void Start () {
        GetComponent<GUIText>().material.color = color; // set text color
        alpha = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (alpha > 0)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + scroll * Time.deltaTime);
            alpha -= Time.deltaTime / duration;
            GetComponent<GUIText>().material.color = new Color(0.8f, 0.8f, 0, alpha);
        }
        else
        {
            Destroy(gameObject); // text vanished - destroy itself
        }
    }
}
