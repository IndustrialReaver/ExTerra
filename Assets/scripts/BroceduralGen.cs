using UnityEngine;
using System.Collections;

public class BroceduralGen : MonoBehaviour {

    public float radius = 1f;

    byte[,] blocks;
    GameObject Dirt;
    GameObject Stone;
    GameObject Water;
    GameObject Grass;

    // Use this for initialization
    void Start () {
        GeneratePlanet();
    }
	
    public void GeneratePlanet()
    {
        Dirt = Resources.Load<GameObject>("dirt_block");
        Stone = Resources.Load<GameObject>("stone_block");
        Water = Resources.Load<GameObject>("water_block");
        Grass = Resources.Load<GameObject>("grass_block");

        GenTerrain();

        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                GameObject newblock;
                Vector2 newpos = new Vector2(transform.position.x + (blocks.GetLength(0) / 2) - i, transform.position.y + (blocks.GetLength(0) / 2) - j);
                Vector2 center = new Vector2(transform.position.x, transform.position.y);
                if (Vector2.Distance(center, newpos) <= radius)
                {
                    if (blocks[i, j] == 0)
                    {
                        newblock = Instantiate(Water, newpos, Quaternion.identity) as GameObject;
                        newblock.name = "water_block";
                        newblock.transform.parent = transform;
                    }
                    else if (blocks[i, j] == 1)
                    {
                        newblock = Instantiate(Stone, newpos, Quaternion.identity) as GameObject;
                        newblock.name = "stone_block";
                        newblock.transform.parent = transform;
                    }
                    else if (blocks[i, j] == 2)
                    {
                        newblock = Instantiate(Dirt, newpos, Quaternion.identity) as GameObject;
                        newblock.name = "dirt_block";
                        newblock.transform.parent = transform;
                    }
                    else if (blocks[i, j] == 3)
                    {
                        newblock = Instantiate(Grass, newpos, Quaternion.identity) as GameObject;
                        newblock.name = "grass_block";
                        newblock.transform.parent = transform;
                    }
                }
            }
        }
    }

    int Noise(int x, int y, float scale, float mag, float exp)
    {

        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));

    }

    void GenTerrain()
    {
        blocks = new byte[128, 128];

        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            int stone = Noise(px, 0, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 75;

            print(stone);

            int dirt = Noise(px, 0, 100f, 35, 1);
            dirt += Noise(px, 100, 50, 30, 1);
            dirt += 75;


            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py < stone)
                {
                    blocks[px, py] = 0;

                    //The next three lines make dirt spots in random places
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 3;

                    }

                    //The next three lines remove dirt and rock to make caves in certain places
                    if (Noise(px, py * 2, 16, 14, 1) > 10)
                    { //Caves
                        blocks[px, py] = 1;

                    }


                }
                else if (py < dirt)
                {
                    blocks[px, py] = 2;
                }


            }
        }
    }

}
