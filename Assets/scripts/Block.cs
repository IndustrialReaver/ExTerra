using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
    public int health = 500;
    public float mod = 0.08f;
	// Use this for initialization
	void Start () {
        transform.position = new Vector2(transform.position.x % mod, transform.position.z % mod);
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
