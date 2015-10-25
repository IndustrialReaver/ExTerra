using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

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
    GameObject enemy;

	// Use this for initialization
	void Start () {
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

        enemy = Resources.Load<GameObject>("Enemy");

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
                        planet.GetComponent<BroceduralGen>().BroBroBro();
                        planet.name = "Planet_" + Mathf.Abs(x) + "" + Mathf.Abs(y);
                        gameobjects.Add(planet);
                        map.sprite.texture.SetPixel(i, j, Color.blue);
                    }
                    else if (chance > 0.8)
                    {
                        x -= Random.Range(0, 100);
                        y -= Random.Range(0, 100);
                        GameObject newenemy = Instantiate(enemy, new Vector2(x, y), Quaternion.identity) as GameObject;
                        gameobjects.Add(newenemy);
                        map.sprite.texture.SetPixel(i, j, Color.red);
                    }
                    else
                    {
                        map.sprite.texture.SetPixel(i, j, Color.black);
                    }
                }
                
            }
        }

        map.sprite.texture.Apply();
        mapP = map.sprite.texture.GetPixels();

        player = Instantiate(Resources.Load("Player"), Vector3.zero, Quaternion.identity) as GameObject;
        GetComponent<CameraControls>().player = player;
        oldplayerpos = player.transform.position;
        map.sprite.texture.SetPixel(5, 5, Color.green);
        map.sprite.texture.Apply();


	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.enabled = !PauseMenu.enabled;
        }


        if (!oldplayerpos.Equals(player.transform.position))
        {
            foreach (GameObject g in gameobjects)
            {
                if (Vector2.Distance(g.transform.position, player.transform.position) > 150)
                {
                    g.SetActive(false);
                }
                else
                {
                    g.SetActive(true);
                }
            }
            setMapP(player.transform.position, Color.green);
            map.sprite.texture.Apply();
        }

        

        oldplayerpos = player.transform.position;
	}

    public void destroyed(GameObject o)
    {
        if (gameobjects.Contains(o))
        {
            gameobjects.Remove(o);
        }
    }

    private void setMapP(Vector2 p, Color c)
    {
        map.sprite.texture.SetPixels(mapP);
        map.sprite.texture.SetPixel((int)(Mathf.Round(p.x + 500) / 100), (int)(Mathf.Round(p.y + 500) / 100), c);
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
