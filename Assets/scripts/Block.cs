using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
    public int health = 500;
    public float mod = 1;
	// Use this for initialization
	void Start () {
        transform.position = calcPos(transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = calcPos(transform.position);
    }

    Vector2 calcPos(Vector2 pos)
    {
        //*
        //float newX = transform.position.x;
        //float newY = transform.position.y;
        //newX *= 10;
        //newY *= 10;
        //newX = newX % mod;
        //newY = newY % mod;
        //newX = Mathf.Round(newX);
        //newY = Mathf.Round(newY);
        //newX = newX / 10;
        //newY = newY / 10;
        //*/
        float newX = Mathf.Round(transform.position.x);// % mod;
        float newY = Mathf.Round(transform.position.y);// % mod;
        //Debug.Log(newX);
        //Debug.Log(newY);
        return new Vector2(newX, newY);
        //transform.position = new Vector2(newX, newZ);
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
