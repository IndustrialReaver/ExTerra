using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public byte[,] grid = new byte[10,10];
    public GameObject[] blocks;
    public Dictionary<string, GameObject> blockmaps;

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
                float x = (grid.GetLength(0) / 2) - i;
                float y = (grid.GetLength(1) / 2) - j;
                if (x != 0 && y != 0)
                {
                    float chance = Random.value;
                    x *= 100;
                    y *= 100;

                    if (chance > 0.95)
                    {
                        GameObject planet = Instantiate(Resources.Load("Planet"), new Vector2(x, y), Quaternion.identity) as GameObject;
                        planet.GetComponent<BroceduralGen>().GeneratePlanet();
                        planet.name = "Planet_" + Mathf.Abs(x) + "" + Mathf.Abs(y);
                        gameobjects.Add(planet);
                    }
                    else if (chance > 0.8)
                    {
                        x -= Random.Range(0, 100);
                        y -= Random.Range(0, 100);
                        GameObject newenemy = Instantiate(enemy, new Vector2(x, y), Quaternion.identity) as GameObject;
                        gameobjects.Add(newenemy);
                    }
                }
            }
        }
        
        player = Instantiate(Resources.Load("Player"), Vector3.zero, Quaternion.identity) as GameObject;
        GetComponent<CameraControls>().player = player;
        oldplayerpos = player.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
	    
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
}
