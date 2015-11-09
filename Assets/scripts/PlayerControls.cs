using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour, SaveData{

    //player vars

    //GM
    GameManager gm;

    //HP
    public int health = 5000;
    public float regain = 500;
    public int HPRegained = 1;
    private float curRegain = 0;
    private int maxHealth;
    public Slider healthBar;
    public Image damageImage;                                   
    public float flashSpeed = 5f;                               
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     
    AudioSource playerAudio;                          
    bool isDead;                                                
    bool damaged;

    //INV
    public Vector2 inventorySize = new Vector2(5, 10);
    private Inventory inv;
    private InvDisp invD;

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
	public void Init () {
        gm = Camera.main.GetComponent<GameManager>();
        curBPdelay = BPdelay;
        maxHealth = health;
        
        inv = transform.GetComponent<Inventory>();
        inv.Init((int)inventorySize.x, (int)inventorySize.y);
        
        invD = gm.GetComponent<InvDisp>();
        invD.Init((int)inventorySize.x, (int)inventorySize.y);

        healthBar = gm.PlayerHealthBar;
        damageImage = gm.PlayerDamageImage;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }


	// Update is called once per frame
	void FixedUpdate () {
        if (!globals.pause)
        {
            //MOVEMENT
            float dir = Input.GetAxis("Vertical");
            float actSpeed = dir * speed.x;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                actSpeed *= 1.05f;
            }

            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

            velocity += (Vector2)transform.up;
            velocity *= actSpeed;
            velocity *= Time.smoothDeltaTime;

            GetComponent<Rigidbody2D>().velocity = velocity;

            //Update Starfield velocity
            //Camera.main.GetComponent<Starfield>().RecieveVelocity(-velocity);

            //********************************************************************
            //********************************************************************
            //********************************************************************

            //ROTATION
            //zero out rotational velocity
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            if (dir < 0)
            {
                dir = 0;
            }
            float angle = transform.rotation.eulerAngles.z - (Mathf.Atan2(Input.GetAxis("Horizontal"), dir) * Mathf.Rad2Deg) * Time.smoothDeltaTime;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            //PEWPEW
            if (shootTime > 0)
            {
                shootTime--;
            }
            if ((shootTime <= 0) && Input.GetMouseButton(0))
            {
                shootTime = shootInc;
                shoot();
            }
            if (Input.GetMouseButton(1) && curBPdelay <= 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Transform block = hit.transform;
                if (block != null && block.name.Contains("block") && !Input.GetKey(KeyCode.LeftShift))
                {
                    mine(block);
                    curBPdelay = BPdelay;
                }
                else if (block == null && Input.GetKey(KeyCode.LeftShift))//&& inv.GetSelected() != null)
                {
                    place(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    curBPdelay = BPdelay;
                }
            }

            //LAYZARZ
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.BroadcastMessage("Place", SendMessageOptions.DontRequireReceiver);
            }
            else if (Input.GetMouseButton(1))
            {
                transform.BroadcastMessage("Mine", SendMessageOptions.DontRequireReceiver);
            }

            if (curBPdelay > 0) curBPdelay--;

            //HP
            if (damaged)
            {
                damageImage.color = flashColour;
            }
            else
            {
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;

            if (curRegain > 0)
            {
                curRegain -= Time.smoothDeltaTime;
            }
            if (curRegain <= 0)
            {
                if (health < maxHealth)
                {
                    health += HPRegained;
                    curRegain = regain;
                }
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                healthBar.value = health;
            }
            
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


    private void mine(Transform block)
    {
        Debug.Log("PlayerControls::mine -- block: " + block.name);

        //try and add to inventory
        if (inv.Add(block.name))
        {
            gm.destroyed(block.gameObject);
            Destroy(block.gameObject);
        }

        if (inv.invAct["dirt_block"] >= 10
                && inv.invAct["water_block"] >= 10
                && inv.invAct["grass_block"] >= 10
                && inv.invAct["stone_block"] >= 10
                || inv.invAct["space_station"] < 1)
        {
            if (!inv.invAct.ContainsKey("space_station")) { inv.invAct.Add("space_station", 0); }
            inv.Add("space_station");
        }

    }

    private void place(Vector2 pos)
    {
        string blockToPlace = inv.GetSelected().name;
        Debug.Log("PlayerControls::place -- block: " + blockToPlace);
        GameObject placedBlock = Instantiate(inv.PlaceSelected(), pos, new Quaternion()) as GameObject;
        placedBlock.name = blockToPlace;  
    }

    public void ApplyDamage(int damage)
    {
        damaged = true;
        curRegain = regain*4;
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Instantiate(deathObject, transform.position, Quaternion.Euler(90,0,0));
        gm.destroyed(gameObject);
        Destroy(gameObject);
        //Application.LoadLevel("ExTerraMainMenu");
    }

    public void Ping(Transform p){
        GameObject ping = Instantiate(Resources.Load("ShieldPing"), transform.position, transform.rotation) as GameObject;
        ping.transform.parent = this.transform;
        ping.transform.LookAt(p);
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
        SaveData += health + ":";
        //max health
        SaveData += maxHealth + ":";
        //inventory
        SaveData += inv.save();

        return SaveData;
    }

    public void load(string s)
    {
        Debug.Log("PlayerControls::load -- " + s);
        string[] values = s.Split(':');
        gameObject.name = values[0];
        transform.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
        health = int.Parse(values[3]);
        maxHealth = int.Parse(values[4]);

        gm = Camera.main.GetComponent<GameManager>();
        curBPdelay = BPdelay;

        inv = transform.GetComponent<Inventory>();
        inv.Init((int)inventorySize.x, (int)inventorySize.y);
        string loadinv = values[5];
        inv.load(loadinv);

        invD = gm.GetComponent<InvDisp>();
        invD.Init((int)inventorySize.x, (int)inventorySize.y);

        healthBar = gm.PlayerHealthBar;
        damageImage = gm.PlayerDamageImage;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        
    }

}
