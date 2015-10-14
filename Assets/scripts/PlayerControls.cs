using UnityEngine;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

    //player vars

    //HP
    public int health = 5000;
    private int maxHealth;
    private float healthBarlenght;
    

    //INV
    public UnityEngine.UI.Text[] invDispText = new UnityEngine.UI.Text[5];
    public UnityEngine.UI.Image[] invDispImg = new UnityEngine.UI.Image[5];
    public UnityEngine.UI.Image[] invSelectImg = new UnityEngine.UI.Image[5];
    public Dictionary<string, int> inv;
    private string selectedBlock = "";
    Color bkgc;

    //DEATH
    public GameObject deathObject;

    //
    public int BPdelay = 10;
    int curBPdelay;

    //MVMNT
    public Vector2 speed = new Vector2(5, 5);
	private Vector2 movement;
	//public Vector2 topSpeed = new Vector2(5, 5);
	public float topSpeed = 15f;
	
    //PEWPEW
	public int shootInc = 2;
	private int shootTime = 0;
	public bool canRotate = true;
	private int side = 1;
	string guntoshoot;

	// Use this for initialization
	void Start () {
        inv = new Dictionary<string, int>();
        curBPdelay = BPdelay;

        maxHealth = health;
        healthBarlenght = (Screen.width / 2) * (health / (float)maxHealth);

        bkgc = invSelectImg[0].color;
        updateSel();

    }
    
	// Update is called once per frame
	void FixedUpdate () {

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		movement = new Vector2(speed.x * inputX, speed.y * inputY);

        if (GetComponent<Rigidbody2D>().velocity.magnitude < topSpeed)
        {
            GetComponent<Rigidbody2D>().AddForce(movement);
        }

		if (inputX != 0 && inputY != 0) {
			movement *= 0.75f;
		}

		if (GetComponent<Rigidbody2D>().velocity.magnitude > topSpeed) {
			GetComponent<Rigidbody2D>().velocity *= 0.99f;
		}
		
		if(canRotate){
			//*//
            Vector3 mouse = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
			float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle-90);
            //*/

            /*//
            Vector2 dir = GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 100 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, q.eulerAngles.z - 90);
            //*/
        }
		shootTime++;
		if ((shootTime % shootInc == 0) && Input.GetMouseButton(0)) {
			shoot();
		}
        if ((shootTime % shootInc == 0) && Input.GetMouseButton(1) && curBPdelay <= 0) {

                //transform.BroadcastMessage("Mine", SendMessageOptions.DontRequireReceiver);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Transform block = hit.transform;
                if (block != null && block.name.Contains("block") && !Input.GetKey(KeyCode.LeftShift))
                {
                    mine(block);
                    curBPdelay = BPdelay;
                }
                else if (block == null && Input.GetKey(KeyCode.LeftShift) && selectedBlock != "")
                {
                    place(selectedBlock, Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

        updateInv();
        if (Input.GetKeyDown(KeyCode.Alpha1) && inv.Keys.Count > 0)
        {
            updateSel(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inv.Keys.Count > 1)
        {
            updateSel(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inv.Keys.Count > 2)
        {
            updateSel(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && inv.Keys.Count > 3)
        {
            updateSel(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && inv.Keys.Count > 4)
        {
            updateSel(4);
        }

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

    private void updateSel()
    {
        foreach (UnityEngine.UI.Image i in invSelectImg)
        {
            i.color = bkgc;
        }
    }

    private void updateSel(int n)
    {
        updateSel();
        invSelectImg[n].color = Color.cyan;
        selectedBlock = invDispText[n].text.Substring(0, invDispText[n].text.IndexOf(':'));
    }

    private void updateInv()
    {
        
        string[] invItems = new string[inv.Keys.Count];
        inv.Keys.CopyTo(invItems, 0);

        for (int i = 0; i < 5; i++)
        {
            if(invItems.Length > i && invItems[i] != null && inv[invItems[i]] > 0)
            {
                invDispText[i].text = invItems[i] + ": " + inv[invItems[i]];
                invDispImg[i].sprite = Resources.Load<Sprite>("images/" + invItems[i]);
            } else
            {
                invDispText[i].text = "";
                invDispImg[i].sprite = Resources.Load<Sprite>("images/blank_block");
            }
        }
    }

    private void mine(Transform block)
    {
        Debug.Log("PlayerControls::mine -- block: " + block.name);
        //transform.BroadcastMessage ("Mine", SendMessageOptions.DontRequireReceiver);

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
            if (inv[selectedBlock] >= 1)
            {
                inv[selectedBlock] -= 1;
                GameObject placedBlock = Instantiate(Resources.Load(selectedBlock), pos, new Quaternion()) as GameObject;
                placedBlock.name = selectedBlock;
                if (inv[selectedBlock] < 1)
                {
                    updateSel();
                    inv.Remove(selectedBlock);
                    selectedBlock = "";
                }
            }
        }

        

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
