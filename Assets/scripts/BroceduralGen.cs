using UnityEngine;
using System;
using System.Collections.Generic;

public class BroceduralGen : MonoBehaviour {

    public float radius = 1f;

    byte[,] littleBros;
    GameObject[] bros;

    int offset = -1;

    private System.Random broseph = new System.Random();

    public void BroBroBro()
    {
        bros = Camera.main.GetComponent<GameManager>().blocks;
        SwapBros(bros);
        
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

    public void BroBroBro(int off)
    {
        offset = off;
        BroBroBro();
    }

    private int BRO(int x, int y, float scale, float mag, float exp)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
    }

    private void FISTBUMP()
    {
        littleBros = new byte[128, 128];
        if (offset < 0)
        {
            offset = Mathf.RoundToInt(UnityEngine.Random.value * 200);
        }

        for (int px = 0; px < littleBros.GetLength(0); px++)
        {
            int brostone = BRO(px + offset, 0 + offset, 80, 15, 1);
            brostone += BRO(px + offset, 0 + offset, 50, 30, 1);
            brostone += BRO(px + offset, 0 + offset, 10, 10, 1);
            brostone += 75;

            //print(stone);

            int brodirt = BRO(px + offset, 0 + offset, 100f, 35, 1);
            brodirt += BRO(px + offset, 100 + offset, 50, 30, 1);
            brodirt += 75;


            for (int py = 0; py < littleBros.GetLength(1); py++)
            {
                if (py < brostone)
                {
                    littleBros[px, py] = 0;

                    if (BRO(px + offset, py + offset, 12, 16, 1) > 7)
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
                else if (py < brodirt)
                {
                    littleBros[px, py] = 2;
                }


            }
        }
    }

    public void SwapBros<T>(this IList<T> broski)
    {
        int n = broski.Count;
        while (n > 1)
        {
            n--;
            int k = broseph.Next(n + 1);
            T value = broski[k];
            broski[k] = broski[n];
            broski[n] = value;
        }
    }

}
