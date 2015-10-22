using UnityEngine;
using System.Collections;

public class InvDisp : MonoBehaviour {

    //is initialized?
    private bool init;

    //total size
    private Vector2 size;

    //UI bullshit
    public UnityEngine.UI.Text[,] invDispText;
    public UnityEngine.UI.Image[,] invDispImg;
    public UnityEngine.UI.Image[,] invSelectImg;

    
    //color of selector
    Color bkgc;


	
	public void Init (int x, int y) {
        size = new Vector2(x, y);
        invDispText = new UnityEngine.UI.Text[x,y];
        invDispImg = new UnityEngine.UI.Image[x,y];
        invSelectImg = new UnityEngine.UI.Image[x,y];

        //UnityEngine.UI.Text tempTxt;
        //UnityEngine.UI.Image tempImg;
        //UnityEngine.UI.Image tempSlt;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                //tempTxt = Instantiate<UnityEngine.UI.Text>();
                

                invDispImg[i, j] = null;
                invDispText[i, j] = null;
                invSelectImg[i, j] = null;
            }
        }

        init = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (init)
        {







        }
	}
}
