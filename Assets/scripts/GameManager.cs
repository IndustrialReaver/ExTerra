using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    GameObject dirt;
    GameObject stone;
    GameObject water;
    GameObject grass;

	// Use this for initialization
	void Start () {

        dirt = Resources.Load<GameObject>("dirt_block");
        stone = Resources.Load<GameObject>("stone_block");
        water = Resources.Load<GameObject>("water_block");
        grass = Resources.Load<GameObject>("grass_block");

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
