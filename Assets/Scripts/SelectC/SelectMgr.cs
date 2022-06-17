using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMgr
{
    GameObject seleCharPre;
    GameObject itemPre;
    GameObject seleChar;
    // ���صĸ�����λ��
    Transform rootCanTrs;
    // ����
    private static SelectMgr _instance;
    // ѡ��Ľ�ɫid
    public int userRoleId = -1;
    // ��ɫid
    public List<int> userRoles = new List<int>();
    // ��ɫitem
    public List<GameObject> roles = new List<GameObject>();

    // ��ɫ��������
    public List<string> skillInfos = new List<string>();
    public static SelectMgr Instance // ���Ի�ȡ
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SelectMgr();
            }
            return _instance;
        }
    }

    public void Init()
    // ���ض�Ӧ��Դ�����п�¡ �����¼��س���ʱ��Ϸ�������м��ص����嶼��ɾ�������Ǳ���ǣ�Object.DontDestroyOnLoad(Object)��ӱ�ǣ�Ϊ�������¼��غ���Ϸ��������޷�ʹ�����if�жϡ�
    {
        if (rootCanTrs == null)
        {
            rootCanTrs = GameObject.Find("UIRoot/Canvas").transform;
            
        }
        if(seleCharPre == null)
        {
            seleCharPre = Resources.Load("Select/SeleCharaPanel", typeof(GameObject)) as GameObject;
        }
        
        if(seleChar == null)
        {
            // ��� ���������¼��س�����roles��Ԫ��Ϊnull��
            userRoles.Clear();
            roles.Clear();
            seleChar = GameObject.Instantiate(seleCharPre, rootCanTrs);
            seleChar.transform.Find("btn_start").GetComponent<Button>().onClick.AddListener(StartGame);
            seleChar.transform.Find("btn_return").GetComponent<Button>().onClick.AddListener(ReturnClick);
            seleChar.transform.Find("TipsPanel/CloseBtn").GetComponent<Button>().onClick.AddListener(CloseTipsPanel);
        }

        if (userRoles == null || userRoles.Count == 0)
        {   itemPre = Resources.Load("Select/Item", typeof(GameObject)) as GameObject;
            // ��ȡ�û��Ľ�ɫid
            userRoles.Add(0);
            userRoles.Add(1);
            userRoles.Add(2);
            // ��ɫ����
            for (int i = 0; i < userRoles.Count; i++)
            {
                GameObject o = GameObject.Instantiate(itemPre, seleChar.transform.Find("BackGround"));
                Sprite spr = Resources.Load("Select/"+ userRoles[i].ToString(), typeof (Sprite)) as Sprite;
                o.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = spr;
                roles.Add(o);
                int roleId = userRoles[i];
                // ÿ��item����ӵ���¼����� �����ݶ�Ӧ��ɫid��item�������
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.SelectWindow(roleId, o); });
            }
        }

        if(skillInfos.Count == 0)
        {   
            // ��������
            skillInfos.Add("ʱ��ֹͣ3�룬�ڼ䵹��ʱֹͣ����˾�ͻ����˲����ƶ��������ճ���������ȴ20�롣");
            skillInfos.Add("��һ���ͷŰڷ����ӵ���ǰλ�ã��ڶ����ͷ����ﴫ�͵����Ӵ���������ȴ20�롣");
            skillInfos.Add("�������ƶ��ٶ�3�룬������ȴ20�롣");
        }
    }

    public void ChangeSelectCharActive(bool active)
    {
        seleChar.SetActive(active);
    }

    public void SelectWindow(int roleId, GameObject go)
    {
       
        
        GameObject selecting = go.transform.Find("selecting").gameObject;
        if (selecting.activeSelf)
        {   
            // ���������ر�
            SkillTipsChange(roleId, false);
            // item��ѡ���ر�
            selecting.SetActive(false);
            userRoleId = -1;
        }
        else
        {    // ������ɫѡ���ر�
            CleanSelect();
            // ѡ��ǰ��ɫ
            selecting.SetActive(true);
            // ��ʾδѡ���ر�
            ChangeTipsPanelActive(false);
            SkillTipsChange(roleId, true);
            userRoleId = roleId;
        }
    }

    public void SkillTipsChange(int roleId, bool active)
    {
        seleChar.transform.Find("CharaTipsPanel").gameObject.SetActive(active);
        // ���¼�������
        seleChar.transform.Find("CharaTipsPanel/SkillInfo").gameObject.GetComponent<Text>().text = skillInfos[roleId];
    }


    // ѡ���ر�
    public void CleanSelect()
    {
        if(roles != null)
        {
            for(int i = 0; i < roles.Count; i++)
            {
                roles[i].transform.Find("selecting").gameObject.SetActive(false);
            }
        }
    }

    // ��ʼ��Ϸ��ť
    public void StartGame()
    {   
        // ���ݽ�ɫid�Ƿ�Ϊ-1���ж��Ƿ�ʼ��Ϸ
        if (userRoleId != -1)
        {
            ChangeSelectCharActive(false);
            // ʱ�����Ÿ�Ϊ1
            Time.timeScale = 1;
            // ��Ϸ�н�����ƽű�����
            GameObject.Find("UIRoot/Canvas").GetComponent<GamingUIControl>().enabled = true;
            // ���ݽ�ɫid������Ӧ��ʼ��
            GameObject.Find("UIRoot/Canvas").GetComponent<GamingUIControl>().Init(userRoleId);
        }
        else
        {
            // ��ʾδѡ���ɫ
            ChangeTipsPanelActive(true);
        }
        
    }

    public void ReturnClick()
    {
        ChangeSelectCharActive(false);
        Component.FindObjectOfType<StartPanelMgr>().ChangeStartPanelActive(true);
    }

    public void ChangeTipsPanelActive(bool active)
    {
        seleChar.transform.Find("TipsPanel").gameObject.SetActive(active);
    }

    public void CloseTipsPanel()
    {
        ChangeTipsPanelActive(false);
    }
}
