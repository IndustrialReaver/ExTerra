using UnityEngine;
using System.Collections;

public class BroceduralGen : MonoBehaviour {

    public float radius = 1f;

    byte[,] blocks;
    GameObject[] placeables;


    public void GeneratePlanet()
    {
        placeables = Camera.main.GetComponent<GameManager>().blocks;

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
                    newblock = Instantiate(placeables[blocks[i, j]], newpos, Quaternion.identity) as GameObject;
                    newblock.name = placeables[blocks[i, j]].name;
                    newblock.transform.parent = transform;
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

                    
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 3;
                    }

                    
                    if (Noise(px, py * 2, 16, 14, 1) > 10)
                    { 
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
