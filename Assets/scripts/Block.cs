using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
    public int health = 500;
    public float mod = 1600f;
	// Use this for initialization
	void Start () {
        float newX = transform.position.x;
        float newZ = transform.position.z;
        newX *= 10000;
        newZ *= 10000;
        newX = newX % mod;
        newZ = newZ % mod;
        newX = newX / 10000;
        newZ = newZ / 10000;
        //transform.position = new Vector2(transform.position.x % mod, transform.position.z);
        transform.position = new Vector2(newX, newZ);
    }
	
	// Update is called once per frame
	void Update () {
	
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
