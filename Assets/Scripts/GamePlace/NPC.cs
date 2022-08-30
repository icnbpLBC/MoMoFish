using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float displayTime = 4.0f;//显示时间

    public GameObject dialogBox;//画布

    float timerDisplay;//对话时长

    public bool createIce;// 判断是否需要生成新的雪球；

    public GameObject text;

    public GameObject parent;//旧的物体

    public GameObject obj;

    private GameObject clone;
    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        createIce = false;
        clone = Resources.Load("Gaming/ice", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Talk();
    }

    //是否进行对话
    public void Talk() 
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        //判断要不要生成新的雪球
        if(createIce == true)
        {
            text.GetComponent<TextMeshProUGUI>().text = "已经生成新的弹药在右下方";
            createIce = false;
            NewIceCreate();
        }else if(createIce == false)
        {
            text.GetComponent<TextMeshProUGUI>().text = "先捡了再来找我要吧";
        }
    }

    /// <summary>
    //增设新的弹药
    /// </summary>
    void NewIceCreate()
    {
        GameObject ice = Instantiate(clone);
       
        int n = Random.Range(20, 40);
        int m = Random.Range(-15, -10);
        ice.transform.position = new Vector2(n, m);
    }
}
