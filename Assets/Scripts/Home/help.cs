using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class help : MonoBehaviour
{
    public GameObject helpObj;

    public Text Ranking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void HelpButtonPress()
    {
            helpObj.SetActive(true);
        reload();
        print("�򿪰������"); 

    }
    public void exit()
    {
        helpObj.SetActive(false);
        print("�رհ������");
    }

    public void reload()
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        dict = DataController.GetRankList();
        string res = " ";
        for(int i = 1; i < dict.Count; i++)
        {
            if (i == 1)
            {
                string v = i.ToString() + ".  " + dict[i].ToString() + "\r\n";
                res = v;
            }
            else
            {
                res = res + i.ToString() + ".  " + dict[i].ToString() + "\r\n";
            }
            
        }
        Ranking.GetComponent<Text>().text = res ;
    }
}
