using UnityEngine;
using System.Collections;

public class SpaceStation : MonoBehaviour, SaveData {

    //HEALTH
    public int health = 8000;
    private int maxHealth;
    private float healthBarlenght;

    //GAMEMANAGER
    GameManager gm;

    //LAUNCHBAYS
    private Gun[] launchbays;
    public int unitCap = 15;
    public int units = 0;
    public GameObject target;
    public GameObject death;
    public bool canRotate = true;
    private int side = 0;
    string guntoshoot;

    public float LD = 20;
    private float LDact;

    // Use this for initialization
    void Start () {
        gm = Camera.main.GetComponent<GameManager>();
        gm.created(gameObject);
        maxHealth = health;
        launchbays = GetComponentsInChildren<Gun>();
        LDact = LD;
    }
	
	// Update is called once per frame
	void Update () {
	    
        if(target != null)
        {
            if(LDact > 0)
            {
                LDact -= Time.smoothDeltaTime;
            }
            else
            {
                shoot();
                LDact = LD;
            }
            
        }
	}

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Death();
        }
    }

    private void shoot()
    {
        if (side < launchbays.Length - 1)
        {
            side++;
        }
        else
        {
            side = 0;
        }
        if (units < unitCap)
        {
            launchbays[side].ShipLaunch(target);
            units++;
        }
    }

    void Death()
    {
        gm.destroyed(transform.gameObject);
        Instantiate(death, transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(gameObject);
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
        Debug.Log("SpaceStation::load -- " + s);
        string[] values = s.Split(new char[] { ',' });//, System.StringSplitOptions.RemoveEmptyEntries);
        gameObject.name = values[0];
        transform.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
        health = int.Parse(values[3]);
        //Init();
    }

}
