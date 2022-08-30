using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float displayTime = 4.0f;//��ʾʱ��

    public GameObject dialogBox;//����

    float timerDisplay;//�Ի�ʱ��

    public bool createIce;// �ж��Ƿ���Ҫ�����µ�ѩ��

    public GameObject text;

    public GameObject parent;//�ɵ�����

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

    //�Ƿ���жԻ�
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
        //�ж�Ҫ��Ҫ�����µ�ѩ��
        if(createIce == true)
        {
            text.GetComponent<TextMeshProUGUI>().text = "�Ѿ������µĵ�ҩ�����·�";
            createIce = false;
            NewIceCreate();
        }else if(createIce == false)
        {
            text.GetComponent<TextMeshProUGUI>().text = "�ȼ�����������Ҫ��";
        }
    }

    /// <summary>
    //�����µĵ�ҩ
    /// </summary>
    void NewIceCreate()
    {
        GameObject ice = Instantiate(clone);
       
        int n = Random.Range(20, 40);
        int m = Random.Range(-15, -10);
        ice.transform.position = new Vector2(n, m);
    }
}
