using UnityEngine;

public class EnemyControls : MonoBehaviour {


    //HP
    public int health = 3000;
    private int maxHealth;
    private float healthBarlenght;

    //SEEKING
    public int seekDistance = 10;
    public int fireDistance = 8;

    public Vector2 speed = new Vector2(5, 5);
    public float topSpeed = 5.0f;

    private Vector2 movement;
    private Rigidbody2D rgdb;
	
	
	public int shootInc = 2;
    private int tshootInc;

	public GameObject target;
	public GameObject death;
	public bool canRotate = true;
	private int side = 1;
	string guntoshoot;

    GameManager gm;
	
	// Use this for initialization
	void Start () {
        maxHealth = health;
        tshootInc = shootInc;
        healthBarlenght = (Screen.width / 6) * (health / (float)maxHealth);
        rgdb = GetComponent<Rigidbody2D>();
        gm = Camera.main.GetComponent<GameManager>();
    }

    void Update()
    {
        if (rgdb.velocity.magnitude > 0)
        {
            Vector2 dir = rgdb.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Euler(0, 0, q.eulerAngles.z - 90);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (target != null && Vector2.Distance(transform.position, target.transform.position) > seekDistance)
        {
            target = null;
            gm.wartime = false;
        }
        if(target != null)
        {
            gm.wartime = true;
        }
        if (target == null)
        {
            RaycastHit2D[] search;
            search = Physics2D.CircleCastAll(transform.position, seekDistance, Vector2.zero);

            foreach(RaycastHit2D n in search){
                if (n.transform.tag == "Player")
                {
                    target = n.transform.gameObject;
                }
            }

        }

        if (canRotate && target != null)
        {
			Vector2 tarRot = target.transform.position;
			Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
			Vector2 offset = new Vector2(tarRot.x - screenPoint.x, tarRot.y - screenPoint.y);
			var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle-90);
		}

        //movement
        
        if (target != null && Vector2.Distance(this.transform.position, target.transform.position) > (fireDistance/2))
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

    public void ApplyDamage(int damage)
    {
        health -= damage;
        healthBarlenght = (Screen.width / 6) * (health / (float)maxHealth);
        if (health < 0)
        {
            Death();
        }
    }

	private void shoot(){
		if(side > 0) {
			guntoshoot = "Right";
		} else {
			guntoshoot = "Left";
		}
		transform.BroadcastMessage ("EnemyFire", target, SendMessageOptions.DontRequireReceiver);
		side *= -1;
	}

    void Death()
    {
        gm.destroyed(transform.gameObject);
        gm.wartime = false;
        Instantiate(death, transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(this.gameObject);
    }

    void OnGUI () 
    {
        Vector2 newPos = Camera.main.WorldToScreenPoint(transform.position);
        Texture2D color = new Texture2D(1, 1);
        color.SetPixel(1, 1, Color.red);
        color.wrapMode = TextureWrapMode.Repeat;
        color.Apply();
        GUIStyle clr = new GUIStyle();
        clr.normal.background = color;
        GUI.Box(new Rect(newPos.x - (Screen.width / 12), Screen.height - newPos.y + 80, healthBarlenght, 5),"",clr);
    }
    

}
