﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour {

    //Game Manager
    GameManager gm;

    //size of inventory
    private Vector2 size;

    //dictonary of block counts
    public Dictionary<string, int> invAct;

    //2d array of inventory grid
    public string[,] inventory;

    //boolean value of if the inventory is full
    bool full = false;

    //pointer to currently selected inventory space
    private Vector2 pointer = new Vector2(0,0);


	// Update is called once per frame
	void Update () {
        //updateInv();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            updateSel(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            updateSel(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            updateSel(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            updateSel(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            updateSel(4);
        }
	}
    
    /// <summary>
    /// Initilizes a clean inventory, and syncs its list of blocks with the GM's list
    /// </summary>
    /// <param name="x">the number of rows to make</param>
    /// <param name="y">the number of columns to make</param>
    public void Init(int x, int y)
    {
        size = new Vector2(x, y);
        invAct = new Dictionary<string, int>();
        inventory = new string[x, y];
        gm = Camera.main.GetComponent<GameManager>();
        GameObject[] temp = gm.blocks;
        for (int i = 0; i < temp.Length; i++)
        {
            invAct.Add(temp[i].name, 0);
        }
        
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                inventory[i, j] = null;
            }
        }

        updateInv();
        updateSel(0,0);
    }
    
    /// <summary>
    /// Adds The GameObject to the inventory at the next open space
    /// </summary>
    /// <param name="b">The GameObject to add</param>
    /// <returns>returns true if added, false if inventory is full</returns>
    public bool Add(string b)
    {
        if (!full)
        {
            for (int i = 0; i < inventory.GetLength(0); i++)
            {
                for (int j = 0; j < inventory.GetLength(1); j++)
                {
                    if (inventory[i, j] == b)
                    {
                        invAct[b] += 1;
                        Debug.Log("Inventory::Add -- added: " + b + " to [" + i + ", " + j + " ]");
                        return true;
                    }
                    else if (inventory[i, j] == null)
                    {
                        inventory[i, j] = b;
                        if (invAct.ContainsKey(b))
                        {
                            invAct[b] += 1;
                        }
                        else
                        {
                            invAct.Add(b, 1);
                        }
                        Debug.Log("Inventory::Add -- added: " + b + " to [" + i + ", " + j + " ]");
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
    /// Adds The GameObject to the inventory at the specified x and y
    /// </summary>
    /// <param name="b">The GameObject to add</param>
    /// <param name="x">The x position in the inventory</param>
    /// <param name="y">The y position in the inventory</param>
    /// <returns>returns true if added, false if inventory is full</returns>
    public bool Add(string b, int x, int y)
    {
        if (!full)
        {
            if (inventory[x, y] == b)
            {
                invAct[b] += 1;
                Debug.Log("Inventory::Add -- added: " + b + " to [" + x + ", " + y + " ]");
                return true;
            }
            else if (inventory[x, y] == null)
            {
                inventory[x, y] = b;
                if (invAct.ContainsKey(b))
                {
                    invAct[b] += 1;
                }
                else
                {
                    invAct.Add(b, 1);
                }
                Debug.Log("Inventory::Add -- added: " + b + " to [" + x + ", " + y + " ]");
                updateInv();
                return true;
            }
            Debug.Log("Inventory::Add -- failed");
            return false;
        }
        else
        {
            Debug.Log("Inventory::Add -- cannot add: Inventory full");
            return false;
        }
    }

    /// <summary>
    /// Returns the Game object that is currently selected in the inventory, without removing it.
    /// </summary>
    /// <returns>Currently selected inventory space</returns>
    public GameObject GetSelected()
    {
        return gm.blockmaps[inventory[(int)pointer.x, (int)pointer.y]];
    }

    /// <summary>
    /// Returns the GameObject in the currently selected inventory space, and removes one.
    /// </summary>
    /// <returns>Currently selected GameObject</returns>
    public GameObject PlaceSelected()
    {
        GameObject toplace = GetSelected();
        invAct[toplace.name] -= 1;
        if (invAct[toplace.name] <= 0)
        {
            inventory[(int)pointer.x, (int)pointer.y] = null;
        }
        updateInv();
        return toplace;
    }

    /// <summary>
    /// removes the specified amount of the given item from the inventory if it can, wont remove if there are not enough
    /// </summary>
    /// <param name="key">the item</param>
    /// <param name="amount">the amount to remove</param>
    /// <returns>true if there was enough to remove, false if not</returns>
    public bool Remove(string key, int amount)
    {
        if (invAct.ContainsKey(key) && invAct[key] >= amount)
        {
            invAct[key] -= amount;
            if (invAct[key] <= 0)
            {
                Vector2 pos = GetIndexAt(key);
                inventory[(int)pos.x, (int)pos.y] = null;
            }
            updateInv();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// moves the pointer in the inventory to the first row in the specified collumn
    /// </summary>
    /// <param name="y">to column to move the pointer to</param>
    public void updateSel(int y)
    {
        pointer = new Vector2(0,y);
    }

    /// <summary>
    /// moves the pointer in the inventory to the specified position
    /// </summary>
    /// <param name="x">the row to move the pointer to</param>
    /// <param name="y">the column to move the pointer to</param>
    public void updateSel(int x, int y)
    {
        pointer = new Vector2(x, y);
    }

    /// <summary>
    /// gets the current pointer location
    /// </summary>
    /// <returns>vector2 representation of the pointer in the inventory</returns>
    public Vector2 getPointer()
    {
        return pointer;
    }

    /// <summary>
    /// updates the inventorys full status
    /// </summary>
    private void updateInv()
    {
        bool tfull = true;
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i, j] == null)
                {
                    tfull = false;
                }
            }
        }
        full = tfull;
    }

    /// <summary>
    /// Returns the name of the Game object at the given position in the inventory, without removing it.
    /// </summary>
    /// <returns>selected inventory space</returns>
    public string GetNameAt(int x, int y)
    {
        return inventory[x, y];
    }

    /// <summary>
    /// Returns the number of Game objects held at the given position in the inventory, without removing them.
    /// </summary>
    /// <returns>number of items at selected inventory space</returns>
    public int GetNumberAt(int x, int y)
    {
        return invAct[inventory[x, y]];
    }

    /// <summary>
    /// Gets the index of an item in the inventory, returns -1,-1 if the object is not in the inventory
    /// </summary>
    /// <param name="key">the name of the object</param>
    /// <returns>Vector2 x and y coordinates of the object</returns>
    public Vector2 GetIndexAt(string key)
    {
        Vector2 position = new Vector2(-1, -1);
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i, j] == key)
                {
                    position = new Vector2(i, j);
                }
            }
        }
        return position;
    }

    /// <summary>
    /// saves the inventory to a string
    /// </summary>
    /// <returns>a string representation of the current inventory</returns>
    public string save()
    {
        string SaveData = "";
        string[] keys = new string[invAct.Count];
        invAct.Keys.CopyTo(keys,0);
        foreach(string s in keys)
        {
            SaveData += s + "-" + invAct[s] + ",";
        }
        return SaveData;
    }

    /// <summary>
    /// loads an inventory from a string
    /// </summary>
    /// <param name="s">the inventory to load</param>
    public void load(string s)
    {
        string[] entries = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        int position = 0;
        foreach (string t in entries)
        {
            string[] temp = t.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            invAct[temp[0]] = int.Parse(temp[1]);
            if (int.Parse(temp[1]) > 0)
            {
                inventory[0, position] = temp[0];
                position++;
            }
        }
    }
}
