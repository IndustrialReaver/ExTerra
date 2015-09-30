using UnityEngine;
using System.Collections;

public class EnemyFighterControls : MonoBehaviour {

    //HP
    public int health = 750;
    private int maxHealth;
    private float healthBarlenght;

    public GameObject deathObject;

    public Vector2 speed = new Vector2(1f, 2.5f);

    private Vector3 movement;

    //public Vector2 topSpeed = new Vector2(5, 5);
    public float topSpeed = 7.0f;

    public int shootInc = 2;
    private int shootTime = 0;

    public GameObject target;
    public GameObject death;
    public bool canRotate = true;
    private int side = 1;
    string guntoshoot;

    // Use this for initialization
    void Start()
    {
        maxHealth = health;
        healthBarlenght = (Screen.width / 6) * (health / (float)maxHealth);
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

        
        if (GetComponent<Rigidbody2D>().velocity.magnitude < topSpeed) // Vector2.Distance(this.transform.position, target.transform.position) > 0.2f)
        {
            //Vector2.MoveTowards(this.transform.position, target.transform.position, .01f);
            //transform.rotation.SetFromToRotation(this.transform.rotation.eulerAngles, this.transform.rotation.eulerAngles+target.transform.rotation.eulerAngles);
            //transform.rotation.SetLookRotation(this.transform.rotation.eulerAngles + target.transform.rotation.eulerAngles);

            movement += transform.up; //new Vector2(speed.x * transform.up.y, speed.y * transform.up.x);
            movement *= speed.x;
            movement.z = 0;
            //GetComponent<Rigidbody2D>().AddForce(movement);
            Vector3 n = transform.position;
            movement *= Time.smoothDeltaTime;
            n += movement;
            transform.position = n;
            //transform.rotation = Quaternion.SetLookRotation(GetComponent<Rigidbody2D>().velocity);
        }
        //else
        //{
            //GetComponent<Rigidbody2D>().velocity *= 0.95f;
        //}


        shootTime++;
        if ((shootTime % shootInc == 0))
        {
            shoot();
        }
        if ((shootTime % shootInc == 0) && Input.GetMouseButton(1))
        {
            mine();
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

    private void mine()
    {
        transform.BroadcastMessage("Mine", SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //stuff
    }

    void Death()
    {
        Instantiate(deathObject, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    void OnGUI()
    {
        Vector2 newPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Box(new Rect(newPos.x - (Screen.width / 12), Screen.height - newPos.y + 80, healthBarlenght, 20), health + "/" + maxHealth);
    }
}
