using UnityEngine;
using System.Collections;

public class BroceduralGen : MonoBehaviour {

    public float radius = 1f;

    byte[,] littleBros;
    GameObject[] bros;


    public void BroBroBro()
    {
        bros = Camera.main.GetComponent<GameManager>().blocks;
        bros.
        
        FISTBUMP();

        for (int i = 0; i < littleBros.GetLength(0); i++)
        {
            for (int j = 0; j < littleBros.GetLength(1); j++)
            {
                GameObject newBro;
                Vector2 newpos = new Vector2(transform.position.x + (littleBros.GetLength(0) / 2) - i, transform.position.y + (littleBros.GetLength(0) / 2) - j);
                Vector2 center = new Vector2(transform.position.x, transform.position.y);
                if (Vector2.Distance(center, newpos) <= radius)
                {
                    newBro = Instantiate(bros[littleBros[i, j]], newpos, Quaternion.identity) as GameObject;
                    newBro.name = bros[littleBros[i, j]].name;
                    newBro.transform.parent = transform;
                }
            }
        }
    }

    private int BRO(int x, int y, float scale, float mag, float exp)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
    }

    private void FISTBUMP()
    {
        littleBros = new byte[128, 128];
        int offset = Mathf.RoundToInt(Random.value * 200);

        for (int px = 0; px < littleBros.GetLength(0); px++)
        {
            int stone = BRO(px + offset, 0 + offset, 80, 15, 1);
            stone += BRO(px + offset, 0 + offset, 50, 30, 1);
            stone += BRO(px + offset, 0 + offset, 10, 10, 1);
            stone += 75;

            print(stone);

            int dirt = BRO(px + offset, 0 + offset, 100f, 35, 1);
            dirt += BRO(px + offset, 100 + offset, 50, 30, 1);
            dirt += 75;


            for (int py = 0; py < littleBros.GetLength(1); py++)
            {
                if (py < stone)
                {
                    littleBros[px, py] = 0;

                    if (BRO(px + offset, py + offset, 12, 16, 1) > 5)
                    {
                        littleBros[px, py] = 2;
                    }

                    if (BRO(px + offset, py + offset, 12, 16, 1) > 10)
                    {
                        littleBros[px, py] = 3;
                    }

                    
                    if (BRO(px + offset, py * 2 + offset, 16, 14, 1) > 10)
                    { 
                        littleBros[px, py] = 1;

                    }


                }
                else if (py < dirt)
                {
                    littleBros[px, py] = 2;
                }


            }
        }
    }

}
