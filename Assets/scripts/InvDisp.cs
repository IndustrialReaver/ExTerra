using UnityEngine;
using UnityEngine.UI;

public class InvDisp : MonoBehaviour {

    //is initialized?
    private bool init;

    //inventory
    Inventory inventory;

    //total size
    private Vector2 size;

    //curent selections
    private Vector2 curselect;

    //UI bullshit
    public Text[,] invDispText;
    public Image[,] invDispImg;
    public Image[,] invSelectImg;

    public Text[] invDText = new Text[5];
    public Image[] invDImg = new Image[5];
    public Image[] invSImg = new Image[5];
    
    //color of selector
    public Color bkgc;


	
	public void Init (int x, int y) {
        size = new Vector2(x, y);
        inventory = transform.GetComponent<GameManager>().getPlayer().GetComponent<Inventory>();
        invDispText = new Text[x,y];
        invDispImg = new Image[x,y];
        invSelectImg = new Image[x,y];

        //UnityEngine.UI.Text tempTxt;
        //UnityEngine.UI.Image tempImg;
        //UnityEngine.UI.Image tempSlt;

        for (int i = 0; i < 5; i++)
        {
            invDispImg[0, i] = invDImg[i];
            invDispText[0, i] = invDText[i];
            invSelectImg[0, i] = invSImg[i];
        }

        for (int i = 1; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                //tempTxt = Instantiate<UnityEngine.UI.Text>();


                invDispImg[i, j] = null;
                invDispText[i, j] = null;
                invSelectImg[i, j] = null;
            }
        }

        bkgc = invDispImg[0, 1].color;
        curselect = inventory.getPointer();
        updateSelect();
        init = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (init)
        {
            if (curselect != inventory.getPointer())
            {
                curselect = inventory.getPointer();
                updateSelect();
            }
            string temp;
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    temp = inventory.GetNameAt(i, j);
                    if (temp != null)
                    {
                        invDispImg[i, j].sprite = Resources.Load<Sprite>("images/"+temp);
                        invDispText[i, j].text = ""+inventory.GetNumberAt(i,j);
                    }
                    else if (invDispImg[i, j] != null)
                    {
                        invDispImg[i, j].sprite = Resources.Load<Sprite>("images/blank_block");
                        invDispText[i, j].text = "";
                    }
                }
            }

        }
	}


    private void updateSelect()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (invSelectImg[i, j] != null)
                {
                    invSelectImg[i, j].color = bkgc;
                }
            }
        }
        invSelectImg[(int)curselect.x, (int)curselect.y].color = Color.cyan;
    }
}
