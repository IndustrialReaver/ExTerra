using UnityEngine;
using System.Collections.Generic;

public class Starfield : MonoBehaviour
{

    public float starRadius = 10.0f;
    public int starDensity = 100;

    private GameObject starParent;
    private List<GameObject> stars;

    private Texture2D[] starTexture = new Texture2D[5];
    private Sprite[] starSprite = new Sprite[5];

    private Texture2D[] nebulaTexture = new Texture2D[1];
    private Sprite[] nebulaSprite = new Sprite[1];

    private int starWidth = 16;
    private int starHeight = 16;

    public float starScalar = 1.0f;
    public Color baseStarColor;

    private Vector3 lastCamLocation = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        starParent = new GameObject("Star Field");

        stars = new List<GameObject>();

        // Generate our star textures
        buildStars();

        SpawnStar(200, true);

        // Generate our nebula
        //buildNebula();

        //SpawnNebula(3, true);
    }

    void buildNebula()
    {
        int nebulaWidth = 256;
        int nebulaHeight = 256;

        nebulaTexture[0] = new Texture2D(nebulaWidth, nebulaHeight);
        Color[] pixelArray = new Color[nebulaWidth * nebulaHeight];

        for (int i = 0; i < pixelArray.Length; i++)
        {
            pixelArray[i] = Color.red;
        }

        CalcNoise(nebulaWidth, nebulaHeight);

        pixelArray = pix;

        nebulaTexture[0].SetPixels(pixelArray);
        nebulaTexture[0].anisoLevel = 0;
        nebulaTexture[0].filterMode = FilterMode.Point;
        nebulaTexture[0].Apply();

        nebulaSprite[0] = Sprite.Create(nebulaTexture[0], new Rect(0, 0, nebulaWidth, nebulaHeight), new Vector2(0, 0));
    }

    void buildStars()
    {
        for (int z = 0; z < 5; z++)
        {
            // Produce a star texture
            int sW = starWidth;
            int sH = starHeight;
            starTexture[z] = new Texture2D(sW, sH);
            Color[] pixelArray = new Color[sW * sH];

            for (int i = 0; i < pixelArray.Length; i++)
            {
                int y = i % starWidth;
                int x = i / starHeight;

                float fromCenter = Vector2.Distance(new Vector2(starWidth / 2, starHeight / 2), new Vector2(x, y));

                float lerpLevel = fromCenter / 12.0f;

                lerpLevel *= z;

                pixelArray[i] = Color.Lerp(Color.white, Color.clear, lerpLevel);
                pixelArray[i] = baseStarColor;
            }

            starTexture[z].SetPixels(pixelArray);

            starTexture[z].anisoLevel = 0;
            starTexture[z].filterMode = FilterMode.Point;

            starTexture[z].Apply();

            // Blur each one more as we go

            starSprite[z] = Sprite.Create(starTexture[z], new Rect(0, 0, sW, sH), new Vector2(0, 0));
        }
    }

    private Texture2D noiseTex;
    public float xOrg;
    public float yOrg;
    public float perlinScale = 1.0f;
    private Color[] pix;
    void CalcNoise(float pixWidth, float pixHeight)
    {
        noiseTex = new Texture2D((int)pixWidth, (int)pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        float y = 0.0F;
        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * perlinScale;
                float yCoord = yOrg + y / noiseTex.height * perlinScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)(y * noiseTex.width + x)] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    private Vector3 vel = new Vector3(-1, -1, -1);

    public void RecieveVelocity(Vector3 v)
    {
        vel = v;
    }

    // Update is called once per frame
    void Update()
    {
        if (stars.Count < starDensity)
        {
            SpawnStar(starDensity - stars.Count, false);
        }

        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i])
            {

                Vector3 offset = camVelocity;

                if (vel != new Vector3(-1, -1, -1))
                {
                    offset = vel;
                }

                if (offset != new Vector3(0, 0, 0))
                {

                    float z = stars[i].transform.position.z;

                    if (z == 0) { z = 1; }

                    float offsetScalar = (z / 10.0f);

                    if (offsetScalar < 0.3f)
                    {
                        offsetScalar = 0.3f;
                    }
                    if (offsetScalar >= 1.0f)
                    {
                        offsetScalar = 0.9f;
                    }

                    offset *= offsetScalar;
                    stars[i].transform.position += new Vector3(offset.x, offset.y, 0.0f);

                    Vector3 temp = stars[i].transform.position;
                    temp.z = transform.position.z;

                    if (Vector3.Distance(transform.position, temp) > (starRadius * 1.30f))
                    {
                        GameObject.Destroy(stars[i]);
                    }
                }
            }
        }



        flushList();
    }

    private Vector3 camVelocity = Vector3.zero;

    void LateUpdate()
    {
        camVelocity = transform.position - (lastCamLocation);
        lastCamLocation = transform.position;
    }


    void flushList()
    {
        // Try to only run this once, but it should be done each frame to cleanup the star list.
        List<GameObject> clean = new List<GameObject>();

        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i])
            {
                clean.Add(stars[i]);
            }
        }

        stars = clean;
    }

    Vector3 randomClamp(float maxRadius, float minRadius)
    {
        // Finds a random point within the max radius, but outside the minimum radius\
        float mod = Random.Range(minRadius, maxRadius);
        float angle = Random.Range(0, 360);

        angle *= Mathf.Deg2Rad;

        return new Vector3(Mathf.Sin(angle) * mod, Mathf.Cos(angle) * mod, Random.Range(1, 10));
    }

    void SpawnStar(int q, bool bake = false)
    {
        for (int i = 0; i < q; i++)
        {
            // First determine the star point, scale, color, and lamb.

            Vector3 point = Vector3.zero;
            Vector3 scale = Vector3.zero;
            Color color = Color.white;
            float scaleScalar = starScalar;

            // Calculate some of the stuff
            Vector3 root = Random.insideUnitSphere;
            scale = new Vector3(root.z, root.z, 1.0f);
            if (bake)
            {
                point = transform.position + randomClamp(starRadius, 0.0f);
            }
            else
            {
                point = transform.position + randomClamp(starRadius, starRadius - (4));
            }

            color = new Color(
                Random.Range(0.8f, 1.0f),
                1.0f,
                Random.Range(0.8f, 1.0f));

            // Generate the objects, and push there variables
            GameObject newStar = new GameObject("Star");
            SpriteRenderer s = newStar.AddComponent<SpriteRenderer>();
            s.sprite = starSprite[Random.Range(0, starSprite.Length)];
            s.sortingOrder = -5;
            //s.color = color;
            newStar.transform.parent = starParent.transform;
            point.z = Random.Range(1, 11);
            point.z -= 10;
            newStar.transform.position = point;
            newStar.transform.localScale = new Vector3(scale.x * scaleScalar, scale.y * scaleScalar, 1.0f);

            stars.Add(newStar);
        }
    }

    void SpawnNebula(int q, bool bake = false)
    {
        for (int i = 0; i < q; i++)
        {
            // First determine the star point, scale, color, and lamb.

            Vector3 point = Vector3.zero;
            Vector3 scale = Vector3.zero;
            Color color = Color.white;
            float scaleScalar = starScalar;

            // Calculate some of the stuff
            Vector3 root = Random.insideUnitSphere;
            scale = new Vector3(3.0f, 3.0f, 1.0f);
            if (bake)
            {
                point = transform.position + randomClamp(starRadius, 0.0f);
            }
            else
            {
                point = transform.position + randomClamp(starRadius, starRadius - (starRadius * 0.30f));
            }


            color = new Color(
                Random.Range(0.8f, 1.0f),
                1.0f,
                Random.Range(0.8f, 1.0f));

            // Generate the objects, and push there variables
            GameObject newStar = new GameObject("Nebula");
            SpriteRenderer s = newStar.AddComponent<SpriteRenderer>();
            s.sprite = nebulaSprite[0];
            //s.color = color;
            newStar.transform.parent = starParent.transform;
            point.z = Random.Range(0, 11);
            newStar.transform.position = point;
            newStar.transform.localScale = new Vector3(scale.x * scaleScalar, scale.y * scaleScalar, scale.z);

            stars.Add(newStar);
        }
    }
}