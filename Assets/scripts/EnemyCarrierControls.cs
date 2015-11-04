using UnityEngine;
using System.Collections;

public class EnemyCarrierControls : MonoBehaviour, SaveData {

    //HP
    public int health = 3000;
    private int maxHealth;
    private float healthBarlenght;

    //SEEKING
    public int seekDistance = 10;
    public int fireDistance = 8;

    public Vector2 speed = new Vector2(5, 5);
    public float topSpeed = 5.0f;

    public int unitCap = 8;
    public int units = 0;


    private Vector2 movement;
    private Rigidbody2D rgdb;

    private Gun[] launchbays;

    public int shootInc = 2;
    private int tshootInc;

    public GameObject target;
    public GameObject death;
    public bool canRotate = true;
    private int side = 0;
    string guntoshoot;

    GameManager gm;

    // Use this for initialization
    void Start()
    {
        maxHealth = health;
        tshootInc = shootInc;
        healthBarlenght = (Screen.width / 6) * (health / (float)maxHealth);
        rgdb = GetComponent<Rigidbody2D>();
        gm = Camera.main.GetComponent<GameManager>();
        launchbays = GetComponentsInChildren<Gun>();
    }

    void Update()
    {
        if (!globals.pause)
        {
            if (rgdb.velocity.magnitude > 0)
            {
                Vector2 dir = rgdb.velocity;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, q.eulerAngles.z - 90), 2f);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!globals.pause)
        {
            if (target != null && Vector2.Distance(transform.position, target.transform.position) > seekDistance)
            {
                target = null;
                gm.wartime = false;
            }
            if (target != null)
            {
                gm.wartime = true;
            }
            if (target == null)
            {
                RaycastHit2D[] search;
                search = Physics2D.CircleCastAll(transform.position, seekDistance, Vector2.zero);

                foreach (RaycastHit2D n in search)
                {
                    if (n.transform.tag == "Player")
                    {
                        target = n.transform.gameObject;
                    }
                }

            }

            if (rgdb.velocity.magnitude > 1 && target != null)
            {
                Vector2 tarRot = target.transform.position;
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
                Vector2 offset = new Vector2(tarRot.x - screenPoint.x, tarRot.y - screenPoint.y);
                var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg * Time.smoothDeltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90),2f);
            }

            //movement

            if (target != null && Vector2.Distance(transform.position, target.transform.position) > (fireDistance / 2))
            {
                movement = new Vector2(speed.x * (target.transform.position.x - transform.position.x), speed.y * (target.transform.position.y - transform.position.y));
                rgdb.AddForce(movement);
            }
            else
            {
                if (rgdb.velocity.magnitude < 1 || rgdb.velocity.magnitude > topSpeed)
                {
                    rgdb.velocity *= 0.75f;
                }
                else
                {
                    rgdb.velocity *= 0.25f;
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
        healthBarlenght = (Screen.width / 6) * (health / (float)maxHealth);
        if (health < 0)
        {
            Death();
        }
    }

    private void shoot()
    {
        if (side < launchbays.Length-1)
        {
            side++;
        }
        else
        {
            side = 0;
        }
        if (units < unitCap)
        {
            launchbays[side].EnemyLaunch(target);
            units++;
        }
    }

    void Death()
    {
        gm.destroyed(transform.gameObject);
        gm.wartime = false;
        Instantiate(death, transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(this.gameObject);
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
        GUI.Box(new Rect(newPos.x - (Screen.width / 12), Screen.height - newPos.y + 80, healthBarlenght, 5), "", clr);
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
        Debug.Log("EnemyCarrierControls::load -- " + s);
        string[] values = s.Split(':');
        gameObject.name = values[0];
        transform.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
        health = int.Parse(values[3]);
    }
}
