using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour, SaveData {

    public byte[,] grid = new byte[10,10];
    public Canvas PauseMenu;
    public Image map;
    private Color[] mapP = new Color[100];
    public GameObject[] blocks;
    public Dictionary<string, GameObject> blockmaps;

    public Slider PlayerHealthBar;
    public Image PlayerDamageImage;

    ArrayList gameobjects = new ArrayList();
    Vector2 oldplayerpos;
    GameObject player;
    public Text warning;
    int warningTime = 75;
    int curWarningTime = 0;
    GameObject[] enemy;

    int EnemyCount = 0;
    int EnemyMin = 5;


    //Render Dsitance
    public int RenderDistance = 150;
    

    public bool wartime = false;

    private bool init = false;

    //AUDIO
    public AudioClip peace;
    public AudioClip war;
    AudioSource audso;

    //PLAYER ALLIES
    ArrayList SpaceStations = new ArrayList();

    // Use this for initialization
    void Start () {
        globals.pause = false;
        blocks = new GameObject[4];
        blockmaps = new Dictionary<string, GameObject>();
        blocks[0] = Resources.Load<GameObject>("dirt_block");
        blockmaps.Add(blocks[0].name, blocks[0]);
        blocks[1] = Resources.Load<GameObject>("stone_block");
        blockmaps.Add(blocks[1].name, blocks[1]);
        blocks[2] = Resources.Load<GameObject>("water_block");
        blockmaps.Add(blocks[2].name, blocks[2]);
        blocks[3] = Resources.Load<GameObject>("grass_block");
        blockmaps.Add(blocks[3].name, blocks[3]);
        blockmaps.Add("space_station", Resources.Load<GameObject>("space_station"));

        mapP = new Color[100];

        enemy = new GameObject[2];
        enemy[0] = Resources.Load<GameObject>("Enemy");
        enemy[1] = Resources.Load<GameObject>("Enemy_Carrier");

        if (globals.shouldload)
        {
            load(globals.currentSave);
            globals.shouldload = false;
        }
        else
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    float x = i - (grid.GetLength(0) / 2);
                    float y = j - (grid.GetLength(1) / 2);
                    if (x != 0 && y != 0)
                    {
                        float chance = Random.value;
                        x *= 100;
                        y *= 100;

                        if (chance > 0.95)
                        {
                            GameObject planet = Instantiate(Resources.Load("Planet"), new Vector2(x, y), Quaternion.identity) as GameObject;
                            planet.GetComponent<Planet>().init();
                            planet.name = "Planet";// _" + Mathf.Abs(x) + "" + Mathf.Abs(y);
                            gameobjects.Add(planet);
                            map.sprite.texture.SetPixel(i, j, Color.blue);
                        }
                        else if (chance > 0.8)
                        {
                            //x -= Random.Range(0, 100);
                            //y -= Random.Range(0, 100);

                            GameObject newenemy;

                            float e = Random.Range(-1, 1);
                            if (e < 0)
                            {
                                newenemy = Instantiate(enemy[0], new Vector2(x, y), Quaternion.identity) as GameObject;
                                newenemy.name = "Enemy";
                            }
                            else
                            {
                                newenemy = Instantiate(enemy[1], new Vector2(x, y), Quaternion.identity) as GameObject;
                                newenemy.name = "Enemy_Carrier";
                            }
                            gameobjects.Add(newenemy);
                            map.sprite.texture.SetPixel(i, j, Color.red);

                            EnemyCount++;
                        }
                        else
                        {
                            map.sprite.texture.SetPixel(i, j, Color.black);
                        }
                    }

                }
            }

            //map.sprite.texture.Apply();

            for (int i = 0; i < mapP.Length; i++)
            {
                mapP[i] = Color.black;
            }

            player = Instantiate(Resources.Load("Player"), Vector3.zero, Quaternion.identity) as GameObject;
            player.name = "Player";
            player.GetComponent<PlayerControls>().Init();
            gameobjects.Add(player);
            GetComponent<CameraControls>().player = player;
            oldplayerpos = player.transform.position;
            map.sprite.texture.SetPixel(5, 5, Color.green);
            map.sprite.texture.Apply();


            audso = GetComponent<AudioSource>();
            audso.loop = true;
            audso.clip = peace;
            audso.Play();
        }

        init = true;
    }
	
    void LateUpdate()
    {
        if (init)
        {
            //AUDIO
            if (wartime && audso.clip != war)
            {
                audso.clip = war;
                audso.PlayDelayed(2);
            }
            if (!wartime && audso.clip != peace)
            {
                audso.clip = peace;
                audso.PlayDelayed(2);
            }
        }
    }
	// Update is called once per frame
	void Update () {
         

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.enabled = !PauseMenu.enabled;
            globals.pause = PauseMenu.enabled;
        }

        if (init)
        {

            map.sprite.texture.SetPixels(mapP);
            if (player != null && !oldplayerpos.Equals(player.transform.position))
            {
                foreach (GameObject g in gameobjects)
                {
                    if (g.Equals(null))
                    {
                        destroyed(g);
                    }
                    else
                    {
                        //CHECK IF IN RENDER DISTANCE
                        if (Vector2.Distance(g.transform.position, player.transform.position) > RenderDistance)
                        {
                            g.SetActive(false);
                        }
                        else
                        {
                            g.SetActive(true);
                        }

                        //UPDATE MAP
                        if (g.name.ToLower().Contains("planet"))
                        {
                            setMapP(g.transform.position, Color.blue);
                        }
                        else if (g.name.ToLower().Contains("enemy"))
                        {
                            setMapP(g.transform.position, Color.red);
                        }
                        else if (g.name.ToLower().Contains("player"))
                        {
                            setMapP(g.transform.position, Color.green);
                            oldplayerpos = player.transform.position;
                        }
                    }
                }
                //setMapP(player.transform.position, Color.green);
                map.sprite.texture.Apply();
            }


            if(EnemyCount <= EnemyMin)
            {
                float x = Random.Range(-500, 500);
                float y = Random.Range(-500, 500);
                spawnEnemy(x,y);
            }

            
        }
    }

    public void destroyed(GameObject o)
    {
        if (gameobjects.Contains(o))
        {
            if (o.Equals(player))
            {
                player = null;
            }
            if (o.name.ToLower().Contains("enemy"))
            {
                EnemyCount--;
            }
            gameobjects.Remove(o);
        }
    }

    public void created(GameObject o)
    {
        if (o.name.ToLower().Contains("space_station"))
        {
            SpaceStations.Add(o);
            if (EnemyMin > 0)
            {
                EnemyMin--;
            }
            if (SpaceStations.Count > 1)
            {
                GameObject temp = null;
                foreach (GameObject g in SpaceStations)
                {
                    if(temp != null)
                    {
                        temp.GetComponent<SpaceStation>().target = g;
                    }
                    temp = g;
                }
            }
        }
        else
        {
            gameobjects.Add(o);
        }
    }

    public void spawnEnemy(float x, float y)
    {
        GameObject newenemy;

        float e = Random.Range(-1, 1);
        if (e < 0)
        {
            newenemy = Instantiate(enemy[0], new Vector2(x, y), Quaternion.identity) as GameObject;
            newenemy.name = "Enemy";
        }
        else
        {
            newenemy = Instantiate(enemy[1], new Vector2(x, y), Quaternion.identity) as GameObject;
            newenemy.name = "Enemy_Carrier";
        }

        gameobjects.Add(newenemy);
        EnemyCount++;
    }

    private void setMapP(Vector2 p, Color c)
    {
        map.sprite.texture.SetPixel((int)(Mathf.Round(p.x + 550) / 100), (int)(Mathf.Round(p.y + 550) / 100), c);
    }

    private void drawArrow(GameObject t, Color c)
    {

    }

    public GameObject getPlayer()
    {
        return player;
    }

    public string save()
    {
        string SaveData = "";
        foreach (GameObject o in gameobjects)
        {
            SaveData += o.name + "/" + o.GetComponent<SaveData>().save() + ";";
        }
        foreach (GameObject o in SpaceStations)
        {
            SaveData += o.name + "/" + o.GetComponent<SaveData>().save() + ";";
        }
        return SaveData;
    }

    public void load(string s)
    {
        Debug.Log("GameManager::load -- " + s);
        string[] values = s.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach(string v in values)
        {
            string[] temp = v.Split('/');
            
            if (temp[0].ToLower().Contains("player"))
            {
                player = Instantiate(Resources.Load(temp[0])) as GameObject;
                gameobjects.Add(player);
                player.GetComponent<PlayerControls>().load(temp[1]);

                GetComponent<CameraControls>().player = player;
                oldplayerpos = new Vector2(1000.00057f,-45.79999f);
            }
            else
            {
                if (temp[0].ToLower().Contains("enemy"))
                {
                    EnemyCount++;
                }
                GameObject nobj = Instantiate(Resources.Load(temp[0])) as GameObject;
                nobj.GetComponent<SaveData>().load(temp[1]);
                gameobjects.Add(nobj);
            }
            
        }
        
        for (int i = 0; i < mapP.Length; i++)
        {
            mapP[i] = Color.black;
        }
        
        audso = GetComponent<AudioSource>();
        audso.loop = true;
        audso.clip = peace;
        audso.Play();

        init = true;
    }

}
