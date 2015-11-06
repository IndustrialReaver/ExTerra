using UnityEngine;
using System.Collections;

public class CargoShipControls : MonoBehaviour
{

    //HP
    public int health = 50;
    private int maxHealth;
    private float healthBarlenght;
    
    public Vector2 speed = new Vector2(5, 5);
    public float topSpeed = 5.0f;

    private Vector2 movement;
    private Rigidbody2D rgdb;

    
    public GameObject target;
    public SpaceStation station;
    public GameObject death;
    public bool canRotate = true;

    GameManager gm;

    // Use this for initialization
    void Start()
    {
        maxHealth = health;
        healthBarlenght = (Screen.width / 12) * (health / (float)maxHealth);
        rgdb = GetComponent<Rigidbody2D>();
        gm = Camera.main.GetComponent<GameManager>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), station.GetComponent<Collider2D>());
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!globals.pause)
        {
            if (target != null)
            {
                //rotation
                Vector2 targ = target.transform.position;
                Vector2 turr = transform.position;
                Vector2 offset = new Vector2(targ.x - turr.x, targ.y - turr.y);
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 2f);
                
                //movement
                if (Vector2.Distance(transform.position, target.transform.position) > 0 && rgdb.velocity.magnitude < topSpeed)
                {
                    movement = new Vector2(speed.x * (target.transform.position.x - transform.position.x), speed.y * (target.transform.position.y - transform.position.y));
                    rgdb.AddForce(movement * Time.smoothDeltaTime);
                }
            }
        }

    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        healthBarlenght = (Screen.width / 12) * (health / (float)maxHealth);
        if (health < 0)
        {
            Death();
        }
    }

    void Death()
    {
        Instantiate(death, transform.position, Quaternion.Euler(90, 0, 0));
        if (station != null) { station.units--; }
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
        GUI.Box(new Rect(newPos.x - (Screen.width / 24), Screen.height - newPos.y + 30, healthBarlenght, 2), "", clr);
    }

    public string save()
    {
        //save data
        string SaveData = "";
        //name
        SaveData += gameObject.name + ":";
        //location
        SaveData += transform.position.x + ":" + transform.position.y + ":";
        //health
        SaveData += health;

        return SaveData;
    }

    public void load(string s)
    {
        Debug.Log("CargoShipControls::load -- " + s);
        string[] values = s.Split(':');
        gameObject.name = values[0];
        transform.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
        health = int.Parse(values[3]);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject == target)
        {
            Destroy(gameObject);
        }
    }

}


