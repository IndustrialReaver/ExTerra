using UnityEngine;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

    //player vars

    //GM
    GameManager gm;

    //HP
    public int health = 5000;
    private int maxHealth;
    private float healthBarlenght;
    
    //INV
    private Inventory inv;

    //DEATH
    public GameObject deathObject;

    //
    public int BPdelay = 10;
    int curBPdelay;

    //MVMNT
    public Vector2 speed = new Vector2(5, 5);
	private Vector2 movementX;
    private Vector2 movementY;
	public float topSpeed = 15f;
	
    //PEWPEW
	public int shootInc = 2;
	private int shootTime = 0;
	public bool canRotate = true;
	private int side = 1;
	string guntoshoot;

	// Use this for initialization
	void Start () {
        gm = Camera.main.GetComponent<GameManager>();
        curBPdelay = BPdelay;
        maxHealth = health;
        healthBarlenght = (Screen.width / 2) * (health / (float)maxHealth);
        inv = transform.GetComponent<Inventory>();
    }
    
	// Update is called once per frame
	void FixedUpdate () {

		float inputY = Input.GetAxis("Horizontal");
		float inputX = Input.GetAxis("Vertical");
        
        movementX += (Vector2)transform.up * (speed.x * inputX);


        if (GetComponent<Rigidbody2D>().velocity.magnitude < topSpeed)
        {
            GetComponent<Rigidbody2D>().AddForce(movementX);
        }

		if (inputX == 0) {
			movementX *= 0.75f;
		}

		if (GetComponent<Rigidbody2D>().velocity.magnitude > topSpeed) {
			GetComponent<Rigidbody2D>().velocity *= 0.99f;
		}

        //********************************************************************
        //********************************************************************
        //********************************************************************
		
		if(canRotate){
            Vector3 mouse = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
			float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle-90);
        }

		shootTime++;
		if ((shootTime % shootInc == 0) && Input.GetMouseButton(0)) {
			shoot();
		}
        if ((shootTime % shootInc == 0) && Input.GetMouseButton(1) && curBPdelay <= 0) {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Transform block = hit.transform;
                if (block != null && block.name.Contains("block") && !Input.GetKey(KeyCode.LeftShift))
                {
                    mine(block);
                    curBPdelay = BPdelay;
                }
                else if (block == null && Input.GetKey(KeyCode.LeftShift) )//&& inv.GetSelected() != null)
                {
                    place(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    curBPdelay = BPdelay;
                }
            
		}

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.BroadcastMessage("Place", SendMessageOptions.DontRequireReceiver);
        }
        else if (Input.GetMouseButton(1))
        {
            transform.BroadcastMessage("Mine", SendMessageOptions.DontRequireReceiver);
        }

        if (curBPdelay > 0) curBPdelay--;

    }

	private void shoot(){
		if(side > 0) {
			guntoshoot = "Right";
		} else {
			guntoshoot = "Left";
		}
		transform.BroadcastMessage ("Fire", guntoshoot, SendMessageOptions.DontRequireReceiver);
		side *= -1;
	}


    private void mine(Transform block)
    {
        Debug.Log("PlayerControls::mine -- block: " + block.name);

        //try and add to inventory
        if (inv.Add(block.name))
        {
            Destroy(block.gameObject);
        }
    }

    private void place(Vector2 pos)
    {
        Debug.Log("PlayerControls::place -- block: " + inv.GetSelected().name);
        //transform.BroadcastMessage("Mine", SendMessageOptions.DontRequireReceiver);
        GameObject placedBlock = Instantiate(inv.PlaceSelected(), pos, new Quaternion()) as GameObject;
        placedBlock.name = inv.GetSelected().name;  
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        healthBarlenght = (Screen.width / 2) * (health / (float)maxHealth);
        if (health <= 0)
        {
            Death();
        }
    }

    void OnGUI()
    {
        Vector2 newPos = new Vector2(Screen.width - (Screen.width / 4), Screen.height);
        GUI.Box(new Rect(newPos.x - (Screen.width / 2), Screen.height - newPos.y + 80, healthBarlenght, 20), health + "/" + maxHealth);
        
    }

    void Death()
    {
        Instantiate(deathObject, transform.position, Quaternion.Euler(90,0,0));
        Destroy(this.gameObject);
        //Application.LoadLevel("ExTerraMainMenu");
    }

    public void Ping(Transform p){
        GameObject ping = Instantiate(Resources.Load("ShieldPing"), transform.position, transform.rotation) as GameObject;
        ping.transform.parent = this.transform;
        ping.transform.LookAt(p);
    }

    void OnTriggerEnter2D(Collider2D coll) {
		//stuff
	}
}
