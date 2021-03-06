﻿using UnityEngine;
using System.Collections;

public class EnemyFighterControls : MonoBehaviour {

    //HP
    public int health = 100;
    private int maxHealth;
    private float healthBarlenght;
    

    public Vector2 speed = new Vector2(5, 5);
    public float topSpeed = 5.0f;

    private Vector2 movement;
    private Rigidbody2D rgdb;
	
	
	public int shootInc = 2;
    private int tshootInc;
    public int fireDistance = 8;


    public EnemyCarrierControls carrier;
    public GameObject target;
    public GameObject death;
    private int side = 1;
    string guntoshoot;
    

    // Use this for initialization
    void Start()
    {
        maxHealth = health;
        healthBarlenght = (Screen.width / 24) * (health / (float)maxHealth);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), carrier.GetComponent<Collider2D>());
        rgdb = GetComponent<Rigidbody2D>();
        //rgbd.AddForce();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!globals.pause)
        {
            if(target != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>());


                //rotation
                Vector2 targ = target.transform.position;
                Vector2 turr = transform.position;
                Vector2 offset = new Vector2(targ.x - turr.x, targ.y - turr.y);
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 2f);


                //movement
                if (Vector2.Distance(transform.position, target.transform.position) > (fireDistance / 2) && rgdb.velocity.magnitude < topSpeed)
                {
                    movement = new Vector2(speed.x * (target.transform.position.x - transform.position.x), speed.y * (target.transform.position.y - transform.position.y));
                    rgdb.AddForce(movement * Time.smoothDeltaTime);
                }
                //else if (Vector2.Distance(this.transform.position, target.transform.position) < (fireDistance / 3))
                //{
                //    movement = new Vector2(speed.x * (target.transform.position.x - transform.position.x), speed.y * (target.transform.position.y - transform.position.y)) * -1;
                //    rgdb.AddForce(movement * Time.smoothDeltaTime);
                //}
                else if(Vector2.Distance(transform.position, target.transform.position) > fireDistance)
                {
                    rgdb.velocity *= 0.9f * Time.smoothDeltaTime;
                }



            }
            

            if (shootInc > 0)
            {
                shootInc--;
            }
            if ((shootInc <= 0) && target != null && Vector2.Distance(this.transform.position, target.transform.position) < fireDistance)
            {
                shoot();
                shootInc = tshootInc;
            }
        }
    }


    public void ApplyDamage(int damage)
    {
        health -= damage;
        healthBarlenght = (Screen.width / 24) * (health / (float)maxHealth);
        if (health < 0)
        {
            Death();
        }
    }

    private void shoot()
    {
        if (side > 0)
        {
            guntoshoot = "Right";
        }
        else
        {
            guntoshoot = "Left";
        }
        transform.BroadcastMessage("EnemyFire", target, SendMessageOptions.DontRequireReceiver);
        side *= -1;
    }
    
    
    void Death()
    {
        Instantiate(death, transform.position, Quaternion.Euler(90, 0, 0));
        carrier.units--;
        Destroy(gameObject);
    }

    void OnGUI()
    {
        Vector2 newPos = Camera.main.WorldToScreenPoint(transform.position);
        Texture2D color = new Texture2D(1, 1);
        color.SetPixel(1, 1, Color.red);
        color.wrapMode = TextureWrapMode.Repeat;
        color.Apply();
        GUIStyle clr = new GUIStyle();
        clr.normal.background = color;
        GUI.Box(new Rect(newPos.x - (Screen.width / 48), Screen.height - newPos.y + 40, healthBarlenght, 2), "", clr);
    }
}
