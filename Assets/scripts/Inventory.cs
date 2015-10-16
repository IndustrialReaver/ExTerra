using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    //Game Manager
    GameManager gm;

    //UI bullshit
    public UnityEngine.UI.Text[] invDispText = new UnityEngine.UI.Text[5];
    public UnityEngine.UI.Image[] invDispImg = new UnityEngine.UI.Image[5];
    public UnityEngine.UI.Image[] invSelectImg = new UnityEngine.UI.Image[5];

    //dictonary of block counts
    public Dictionary<string, int> invAct;

    //2d array of inventory grid
    public GameObject[,] inventory = new GameObject[5, 10];

    //boolean value of if the inventory is full
    bool full = false;

    //pointer to currently selected inventory space
    private Vector2 pointer = new Vector2(0,0);

    //color of selector
    Color bkgc;


	// Use this for initialization
	void Start () {
        invAct = new Dictionary<string, int>();
        //bkgc = invSelectImg[0].color;
        //updateSel();
	}
	
	// Update is called once per frame
	void Update () {
        //updateInv();
        if (Input.GetKeyDown(KeyCode.Alpha1) && invAct.Keys.Count > 0)
        {
            updateSel(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && invAct.Keys.Count > 1)
        {
            updateSel(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && invAct.Keys.Count > 2)
        {
            updateSel(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && invAct.Keys.Count > 3)
        {
            updateSel(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && invAct.Keys.Count > 4)
        {
            updateSel(4);
        }
	}


    /// <summary>
    /// Initilizes a clean inventory, and syncs its list of blocks with the GM's list
    /// </summary>
    public void Init()
    {
        invAct = new Dictionary<string, int>();
        gm = Camera.main.GetComponent<GameManager>();
        GameObject[] temp = gm.blocks;
        for (int i = 0; i < temp.Length; i++)
        {
            invAct.Add(temp[i].name, 0);
        }
    }
    
    /// <summary>
    /// Adds The GameObject to the inventory at the next open space
    /// </summary>
    /// <param name="b">The GameObject to add</param>
    /// <returns>returns true if added, false if inventory is full</returns>
    public bool Add(GameObject b)
    {
        if (!full)
        {
            for (int i = 0; i < inventory.GetLength(0); i++)
            {
                for (int j = 0; j < inventory.GetLength(1); j++)
                {
                    if (inventory[i, j] == null)
                    {
                        inventory[i, j] = b;
                        invAct[b.name] += 1;
                        return true;
                    }
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns the Game object that is currently selected in the inventory, without removing it.
    /// </summary>
    /// <returns>Currently selected inventory space</returns>
    public GameObject GetSelected()
    {
        return inventory[(int)pointer.x, (int)pointer.y];
    }

    /// <summary>
    /// Returns the GameObject in the currently selected inventory space, and removes one.
    /// </summary>
    /// <returns>Currently selected GameObject</returns>
    public GameObject PlaceSelected()
    {
        GameObject toplace = GetSelected();
        invAct[toplace.name] -= 1;
        if (invAct[toplace.name] == 0)
        {
            inventory[(int)pointer.x, (int)pointer.y] = null;
        }
        return toplace;
    }





    private void updateSel()
    {
        foreach (UnityEngine.UI.Image i in invSelectImg)
        {
            i.color = bkgc;
        }
    }

    private void updateSel(int n)
    {
        updateSel();
        invSelectImg[n].color = Color.cyan;
        //selectedBlock = invDispText[n].text.Substring(0, invDispText[n].text.IndexOf(':'));
    }

    private void updateInv()
    {

        string[] invItems = new string[invAct.Keys.Count];
        invAct.Keys.CopyTo(invItems, 0);

        for (int i = 0; i < 5; i++)
        {
            if (invItems.Length > i && invItems[i] != null && invAct[invItems[i]] > 0)
            {
                invDispText[i].text = invItems[i] + ": " + invAct[invItems[i]];
                invDispImg[i].sprite = Resources.Load<Sprite>("images/" + invItems[i]);
            }
            else
            {
                invDispText[i].text = "";
                invDispImg[i].sprite = Resources.Load<Sprite>("images/blank_block");
            }
        }
    }

}
