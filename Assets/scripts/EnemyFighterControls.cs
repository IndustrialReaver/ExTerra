using UnityEngine;
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
    public bool canRotate = true;
    private int side = 1;
    string guntoshoot;
    

    // Use this for initialization
    void Start()
    {
        maxHealth = health;
        healthBarlenght = (Screen.width / 24) * (health / (float)maxHealth);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), carrier.GetComponent<Collider2D>());
    }

    void Update()
    {
        var targ = target.transform.position;
        var turr = transform.position;
        var offset = new Vector2(targ.x - turr.x, targ.y - turr.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.01f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!globals.pause)
        {
            //float inputX = Input.GetAxis("Horizontal");
            //float inputY = Input.GetAxis("Vertical");

            //movement = new Vector2(speed.x * inputX, speed.y * inputY);

            //GetComponent<Rigidbody2D>().AddForce(movement);


            //if (inputX != 0 && inputY != 0) {
            //movement *= 0.707106781f;
            //}

            //if (GetComponent<Rigidbody2D>().velocity.magnitude <= topSpeed) {
            //GetComponent<Rigidbody2D>().velocity *= 0.99f;
            //}

            if (canRotate)
            {
                Vector2 tarRot = target.transform.position;
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
                Vector2 offset = new Vector2(tarRot.x - screenPoint.x, tarRot.y - screenPoint.y);
                var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }

            //movement


            if (GetComponent<Rigidbody2D>().velocity.magnitude < topSpeed && target != null) // Vector2.Distance(this.transform.position, target.transform.position) > 0.2f)
            {
                //Vector2.MoveTowards(this.transform.position, target.transform.position, .01f);
                //transform.rotation.SetFromToRotation(this.transform.rotation.eulerAngles, this.transform.rotation.eulerAngles+target.transform.rotation.eulerAngles);
                //transform.rotation.SetLookRotation(this.transform.rotation.eulerAngles + target.transform.rotation.eulerAngles);

                movement += (Vector2)transform.up; //new Vector2(speed.x * transform.up.y, speed.y * transform.up.x);
                movement *= speed.x;

                //GetComponent<Rigidbody2D>().AddForce(movement);
                Vector2 n = transform.position;
                movement *= Time.smoothDeltaTime;
                n += movement;
                transform.position = n;
                //transform.rotation = Quaternion.SetLookRotation(GetComponent<Rigidbody2D>().velocity);
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
