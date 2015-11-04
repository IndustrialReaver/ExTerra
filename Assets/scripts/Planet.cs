using UnityEngine;

public class Planet : MonoBehaviour, SaveData {

    public int offset = -1;
    
    public void init()
    {
        offset = Mathf.RoundToInt(Random.value * 200);
        GetComponent<BroceduralGen>().BroBroBro(offset);
    }

    public string save()
    {
        //save data
        string SaveData = "";
        //name
        SaveData += gameObject.name + ":";
        //location
        SaveData += transform.position.x + ":" + transform.position.y + ":";
        //blocks
        foreach(Component c in gameObject.GetComponentsInChildren<SaveData>(true))
        {
            if (c.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                SaveData += c.gameObject.name + "%" + ((SaveData)c).save() + ":";
            }
        }

        return SaveData;
    }

    public void load(string s)
    {
        Debug.Log("Planet::load -- " + s);
        string[] values = s.Split(new char[] { ':' }, System.StringSplitOptions.RemoveEmptyEntries);
        gameObject.name = values[0];
        transform.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
        for(int i = 3; i < values.Length; i++)
        {
            string[] temp = values[i].Split(new char[] { '%' }, System.StringSplitOptions.RemoveEmptyEntries);
            GameObject nblk = Instantiate(Resources.Load(temp[0])) as GameObject;
            nblk.name = temp[0];
            nblk.transform.parent = transform;
            nblk.GetComponent<SaveData>().load(temp[1]);
        }
        
    }
}
