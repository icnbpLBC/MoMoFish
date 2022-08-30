using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamingUIControl : MonoBehaviour
{
    GameObject gamingPanelPrefab;
    GameObject gamingPanel;
    GameObject settlementPrefab;
    GameObject settlement;
    // ��ʾ�ı�
    GameObject tips;
    // ��ʾ�ı��ı������
    GameObject tipsBg;
    float timePass;

    // ����ҩ����ʹ��
    // ��������
    public int lazyTimeIncre;
    // ��������
    public int busyTimeIncre;
    public float timeLazy;//����ʱ��
    public float timeBusy;//����ʱ��
    public int remaingTime;
    public int lifeTime;//����ʱ��

    public bool isTimeStop; // �Ƿ񵹼�ʱֹͣ�����ܷ���������
    // ��ʱ��
    public int lazyTime;
    int busyTime;
    bool save; // ��������ʱ���Ƿ��Ѵ洢��Ǯ����ʯ
    // �����صĶ��������ں���Ϊ��ͬ��ɫ���
    List<RuntimeAnimatorController> seletCache;
    // Start is called before the first frame update
    void Start()
    {
       
    }


    // ���ݽ�ɫid������Ӧ��ʼ��
    public void Init(int userRoleId)
    {
        if (gamingPanelPrefab == null)
        {
            gamingPanelPrefab = Resources.Load("Gaming/GamingPanel", typeof(GameObject)) as GameObject;
            gamingPanel = Instantiate(gamingPanelPrefab, this.transform);
            tips = gamingPanel.transform.Find("Tips").gameObject;
            tipsBg = gamingPanel.transform.Find("TipsBg").gameObject;
        }
        if (settlementPrefab == null)
        {
            settlementPrefab = Resources.Load("Settlement/Settlement", typeof(GameObject)) as GameObject;
        }
        if (seletCache == null || seletCache.Count == 0)
        {
            seletCache = new List<RuntimeAnimatorController>();
            seletCache.Add(Resources.Load<RuntimeAnimatorController>("Select/palyer1") as RuntimeAnimatorController);
            seletCache.Add(Resources.Load<RuntimeAnimatorController>("Select/PlayerRed") as RuntimeAnimatorController);
            seletCache.Add(Resources.Load<RuntimeAnimatorController>("Select/YeBi") as RuntimeAnimatorController);
        }
        if (userRoleId == 1)  // ������ѡ��ɫid ������Ӧ����ͼƬ�����ö�Ӧ��ɫ�ű�
        {
            GameObject.Find("player").GetComponent<PlayerRedTest>().enabled = true;
            gamingPanel.transform.Find("Inventory/SkillPanel/SkillImage").gameObject.GetComponent<Image>().sprite = Resources.Load("Select/qizhi", typeof(Sprite)) as Sprite;
            gamingPanel.transform.Find("Inventory/SkillPanel/SkillMask").gameObject.GetComponent<Image>().sprite = Resources.Load("Select/qizhi", typeof(Sprite)) as Sprite;
        }
        else if (userRoleId == 0)
        {
            GameObject.Find("player").GetComponent<PlayerWhite>().enabled = true;
            gamingPanel.transform.Find("Inventory/SkillPanel/SkillImage").gameObject.GetComponent<Image>().sprite = Resources.Load("Select/Timer", typeof(Sprite)) as Sprite;
            gamingPanel.transform.Find("Inventory/SkillPanel/SkillMask").gameObject.GetComponent<Image>().sprite = Resources.Load("Select/Timer", typeof(Sprite)) as Sprite;
        }
        else if (userRoleId == 2)
        {
            gamingPanel.transform.Find("Inventory/SkillPanel/SkillImage").gameObject.GetComponent<Image>().sprite = Resources.Load("Select/SpeedShoe", typeof(Sprite)) as Sprite;
            gamingPanel.transform.Find("Inventory/SkillPanel/SkillMask").gameObject.GetComponent<Image>().sprite = Resources.Load("Select/SpeedShoe", typeof(Sprite)) as Sprite;
            GameObject.Find("player").GetComponent<PlayerYeBi>().enabled = true;
        }
        // ʹ�ö�Ӧ�Ľ�ɫ����ͼ��
        Component.FindObjectOfType<Player>().GetComponent<Animator>().runtimeAnimatorController = seletCache[userRoleId];
        // �������ֲ���
        Component.FindObjectOfType<Player>().ChangeAudioClip(0, true);

        // ������ֵ��ʼ��
        remaingTime = 150;
        lifeTime = 0;
        lazyTime = 0;
        busyTime = 0;
        timeLazy = 0;
        timeBusy = 0;
        lazyTimeIncre = 1;
        busyTimeIncre = 1;
        save = false;
        isTimeStop = false;
        UpdateUI();
    }

    // ����fill Amount����ʾ��ͼ������ 0--1  ������ͼƬ��һ�ż���ԭͼ��һ�ż���ͼƬΪ��ɫ������ĵ��Ǻ�ɫͼƬ�Ĳ��������ﵽ������ȴ��Ч����
    public void UpdateSkillMaskFill(float val)
    {
        gamingPanel.transform.Find("Inventory/SkillPanel/SkillMask").gameObject.GetComponent<Image>().fillAmount = val;
    }


    // �޸���ʾ�ı�
    public void ChangeTips(string str)
    {
        tips.GetComponent<Text>().text = str;
    }

    public void ChangeTipsBg(bool active)
    {
        tipsBg.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        UpdateTheRemaingTime();
        AddLazyTime();
        AddBusyTime();
        
    }

    // ÿ��һ�� ����ʱ��1min
    public void UpdateTheRemaingTime()
    {
        timePass += Time.deltaTime;
        if(timePass > 1.0f)
        {   
            // ʱ��ֹͣ����ʱ����
            if (!isTimeStop)
            {
                remaingTime--;
            }
            
            lifeTime++;
            timePass = 0;
        }
        if(remaingTime <= 0)
        {
            LoadSettlement();
        }
    }

    


    // ���ؽ���ҳ��
    public void LoadSettlement()
    {
        // �������ֲ���
        Component.FindObjectOfType<Player>().ChangeAudioClip(3, false);
        // ʱ�����Ÿ�Ϊ0
        Time.timeScale = 0;
        if(settlement == null)
        {
            settlement = Instantiate(settlementPrefab, this.transform);
            settlement.transform.Find("back_to_main").gameObject.GetComponent<Button>().onClick.AddListener(BactToMainClick);
        }
        else
        {
            ChangeSetmentActive(true);
        }

        int moneyIncre = lazyTime * 2 + lifeTime*3;
        int score = lifeTime * 2 + lazyTime * 3;
        int diamondIncre = lazyTime;
        if (!save) // δ����
        {
            // �����û���Ǯ
            Component.FindObjectOfType<Player>().money += moneyIncre;
            Component.FindObjectOfType<Player>().diamond += diamondIncre;
            DataController.SaveUserInfo(Component.FindObjectOfType<Player>().money, Component.FindObjectOfType<Player>().diamond);
            DataController.SaveRankList(score);
            save = true;
        }
        settlement.transform.Find("text_worktime").gameObject.GetComponent<Text>().text = "����:  " + score.ToString() ;
        settlement.transform.Find("text_playtime").gameObject.GetComponent<Text>().text = "����ʱ��: " + lazyTime.ToString() + "min";
        settlement.transform.Find("text_money").gameObject.GetComponent<Text>().text = "��ý��: " + (moneyIncre).ToString();
        settlement.transform.Find("text_diamond").gameObject.GetComponent<Text>().text = "�����ʯ: " + (diamondIncre).ToString();
    }

    // �������ķ���������ĵ���¼�
    public void BactToMainClick()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void ChangeGamingUIActive(bool active)
    {
        gamingPanel.SetActive(active);
    }

    public void ChangeSetmentActive(bool active)
    {
        settlement.SetActive(active);
    }

    // ÿ֡�������ı��ĸ���
    public void UpdateUI()
    {
        // �ҵ���Ӧ�Ӷ��������ı�
        gamingPanel.transform.Find("TimeBG/TheRemaingTime/TheRemaingTimeText").gameObject.GetComponent<Text>().text = "����ʱ��" + remaingTime.ToString() + "min";
        gamingPanel.transform.Find("TimeBG/TheWorkingTime/TheWorkingTimeText").gameObject.GetComponent<Text>().text = "����ʱ����" + busyTime.ToString() + "min";
        gamingPanel.transform.Find("TimeBG/TheLazyTime/TheLazyTimeText").gameObject.GetComponent<Text>().text = "����ʱ����" + lazyTime.ToString() + "min";
        gamingPanel.transform.Find("TimeBG/TheLifeTime/TheLifeTimeText").gameObject.GetComponent<Text>().text = "����ʱ����" + lifeTime.ToString() + "min";
        // ������Ʒ��
        LoadInventory(Component.FindObjectOfType<Player>().userShopItems);
    }

    // ���ļ������û���Ʒ����Ӧ����Ʒ
    public void LoadInventory(Dictionary<int, int> userShopItems)
    {
        Transform invent = gamingPanel.transform.Find("Inventory");
        for(int i = 0; i < 7; i++)
        {
            Transform child = invent.GetChild(i);
            child.Find("ItemNum").GetComponent<Text>().text = "��"+ userShopItems[i + 1].ToString(); ;
        }
    }


    // ��������״̬����������ʱ�� ÿ��1s����1min
    public void AddLazyTime()
    {
        if (Component.FindObjectOfType<Player>().curState == Player.State.Dawdling)
        {   
            timeLazy += Time.deltaTime;
            if (timeLazy > 1.0f)
            {
                lazyTime += lazyTimeIncre;
                timeLazy = 0;
            }
        }
    }

    // ��������״̬�����ӹ���ʱ��
    public void AddBusyTime()
    {
        if (Component.FindObjectOfType<Player>().curState == Player.State.Working)
        {
            timeBusy += Time.deltaTime;
            if (timeBusy > 1.0f)
            {
                busyTime += busyTimeIncre;
                timeBusy = 0;
            }
        }
    }


    //�ı�ʱ��
    public void ChangeTimePass()
    {
        timePass -= 3f;
    }


 
}
