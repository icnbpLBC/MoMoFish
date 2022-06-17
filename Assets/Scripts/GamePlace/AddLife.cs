using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddLife : MonoBehaviour
{
    public float displayTime = 4.0f;//显示时间

    public GameObject dialogBox;//画布

    float timerDisplay;//对话时长

    public GameObject text;

    public int create;// 判断对话状态

    public GameObject addLife;//药剂补充时间
    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Talk();
    }

    void Talk()
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
        //判断要不要生成药剂
        if (create == 0)
        {
            text.GetComponent<TextMeshProUGUI>().text = "去右边角落寻找金币,我可以帮你增加存活时间";
            create = 1;
            NewCreate();
        }
        else if (create == 1)
        {
            text.GetComponent<TextMeshProUGUI>().text = "再找找,就在右边了";
        }else if(create == 2)
        {
            text.GetComponent<TextMeshProUGUI>().text = "好的,我已通过药剂为您增加了时间";
            Component.FindObjectOfType<GamingUIControl>().remaingTime += 25;
            create = 0;
        }
    }

    void NewCreate()
    {
        addLife.SetActive(true);
    }
}
