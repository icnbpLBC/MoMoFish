using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddLife : MonoBehaviour
{
    public float displayTime = 4.0f;//��ʾʱ��

    public GameObject dialogBox;//����

    float timerDisplay;//�Ի�ʱ��

    public GameObject text;

    public int create;// �ж϶Ի�״̬

    public GameObject addLife;//ҩ������ʱ��
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
        //�ж�Ҫ��Ҫ����ҩ��
        if (create == 0)
        {
            text.GetComponent<TextMeshProUGUI>().text = "ȥ�ұ߽���Ѱ�ҽ��,�ҿ��԰������Ӵ��ʱ��";
            create = 1;
            NewCreate();
        }
        else if (create == 1)
        {
            text.GetComponent<TextMeshProUGUI>().text = "������,�����ұ���";
        }else if(create == 2)
        {
            text.GetComponent<TextMeshProUGUI>().text = "�õ�,����ͨ��ҩ��Ϊ��������ʱ��";
            Component.FindObjectOfType<GamingUIControl>().remaingTime += 25;
            create = 0;
        }
    }

    void NewCreate()
    {
        addLife.SetActive(true);
    }
}
