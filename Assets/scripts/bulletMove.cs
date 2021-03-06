﻿using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {

	public float speed;
    public int damage = 10;
    Transform parent;
    Vector2 vectorMove;
	int timetolive = 50;
    

	// Use this for initialization
	void Start () {

    }

	// Update is called once per frame
	void FixedUpdate () {
        if (!globals.pause)
        {
            timetolive--;
            this.transform.Translate(Vector2.up * speed * Time.deltaTime);

            if ((!this.GetComponent<Renderer>().isVisible) && (timetolive < 0))
            {
                Destroy(this.gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (parent == null)
        {
            parent = coll.transform;
        }
        if (coll.transform != parent)
        {
            coll.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            coll.gameObject.SendMessage("Ping",transform, SendMessageOptions.DontRequireReceiver);
            
            GameObject gui = Instantiate(Resources.Load("DamageText"), Camera.main.WorldToViewportPoint(transform.position), Quaternion.identity) as GameObject;
            gui.GetComponent<GUIText>().text = damage.ToString();


            Destroy(this.gameObject);
        }
    }

}
