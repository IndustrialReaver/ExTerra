using UnityEngine;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

    //player vars
    public int health = 5000;
    public Dictionary<string, int> inv;


    public int BPdelay = 10;
    int curBPdelay;

    public Vector2 speed = new Vector2(25, 25);
	
	private Vector2 movement;
	
	//public Vector2 topSpeed = new Vector2(5, 5);
	public float topSpeed = 5.0f;
	
	public int shootInc = 2;
	private int shootTime = 0;

	public GameObject death;
	public bool canRotate = true;
	private int side = 1;
	string guntoshoot;

	// Use this for initialization
	void Start () {
        inv = new Dictionary<string, int>();
        curBPdelay = BPdelay;
    }


	// Update is called once per frame
	void FixedUpdate () {

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		movement = new Vector2(speed.x * inputX, speed.y * inputY);

		GetComponent<Rigidbody2D>().AddForce(movement);


		if (inputX != 0 && inputY != 0) {
			movement *= 0.707106781f;
		}

		if (GetComponent<Rigidbody2D>().velocity.magnitude >= topSpeed) {
			GetComponent<Rigidbody2D>().velocity *= 0.99f;
		}
		
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
                else if (block == null && Input.GetKey(KeyCode.LeftShift))
                {
                    place("dirt_block", Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    curBPdelay = BPdelay;
                }
            
            
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
        transform.BroadcastMessage ("Mine", SendMessageOptions.DontRequireReceiver);

        //check inv
        if (inv.ContainsKey(block.name))
        {
            inv[block.name] += 1;
        } else
        {
            inv.Add(block.name, 1);
        }


        Destroy(block.gameObject);
    }

    private void place(string block, Vector2 pos)
    {
        Debug.Log("PlayerControls::place -- block: " + block);
        transform.BroadcastMessage("Mine", SendMessageOptions.DontRequireReceiver);

        //check inv
        if (inv.ContainsKey(block))
        {
            if (inv[block] >= 1)
            {
                inv[block] -= 1;
                GameObject placedBlock = Instantiate(Resources.Load(block), pos, new Quaternion()) as GameObject;
                placedBlock.name = block;
            }
        }

        

    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
		//stuff
	}
}
