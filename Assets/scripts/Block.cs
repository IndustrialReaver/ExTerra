using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour, SaveData {
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
        float newX = Mathf.Round(transform.position.x);
        float newY = Mathf.Round(transform.position.y);
        return new Vector2(newX, newY);
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public string save()
    {
        string SaveData = "";
        //name
        SaveData += gameObject.name + ",";
        //location
        SaveData += transform.position.x + "," + transform.position.y + ",";
        //health
        SaveData += health + ",";
        return SaveData;
    }

    public void load(string s)
    {
        Debug.Log("Block::load -- " + s);
        string[] values = s.Split(new char[] { ',' });//, System.StringSplitOptions.RemoveEmptyEntries);
        gameObject.name = values[0];
        transform.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
        health = int.Parse(values[3]);
    }
}
