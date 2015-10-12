using UnityEngine;
using System.Collections;

public class BroceduralGen : MonoBehaviour {

    byte[,] blocks;

    // Use this for initialization
    void Start () {
        GenTerrain();

    }
	
	// Update is called once per frame
	void Update () {
	
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
                    blocks[px, py] = 1;

                    //The next three lines make dirt spots in random places
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 2;

                    }

                    //The next three lines remove dirt and rock to make caves in certain places
                    if (Noise(px, py * 2, 16, 14, 1) > 10)
                    { //Caves
                        blocks[px, py] = 0;

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
