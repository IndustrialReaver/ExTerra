using UnityEngine;
using System.Collections;

public class blastFade : MonoBehaviour {
    
    public int fadeTicks;
    public float totalScale;
    public int startOffset;
    private int count = 0;
    public GameObject shard;

	// Use this for initialization
	void Start () {
        count = startOffset;
        int countar = Random.Range(20, 40);
        for (int i = 0; i < countar; i++){
            GameObject sh = (GameObject)Instantiate(shard, this.transform.position, this.transform.rotation);
            sh.transform.Rotate(90, 0, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        count++;
        if (count >= fadeTicks) Destroy(gameObject);
        float scalar = 1 / ((float)fadeTicks) * count;
        this.transform.localScale = new Vector3(scalar*totalScale, scalar*totalScale, scalar*totalScale);
        float opacity = 1 - scalar;
        this.transform.GetComponent<Renderer>().material.SetColor("_TintColor", new Color32((byte)255, (byte)255, (byte)255, (byte)(opacity * 128f)));
	}
}
