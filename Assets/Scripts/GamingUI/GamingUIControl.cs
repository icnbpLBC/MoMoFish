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
    // 提示文本
    GameObject tips;
    // 提示文本的背景面板
    GameObject tipsBg;
    float timePass;

    // 用于药剂的使用
    // 摸鱼增量
    public int lazyTimeIncre;
    // 工作增量
    public int busyTimeIncre;
    public float timeLazy;//摸鱼时间
    public float timeBusy;//工作时间
    public int remaingTime;
    public int lifeTime;//生存时间

    public bool isTimeStop; // 是否倒计时停止【技能发动触发】
    // 计时器
    public int lazyTime;
    int busyTime;
    bool save; // 人物死亡时，是否已存储金钱和钻石
    // 所加载的动画：用于后续为不同角色添加
    List<RuntimeAnimatorController> seletCache;
    // Start is called before the first frame update
    void Start()
    {
       
    }


    // 根据角色id进行相应初始化
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
        if (userRoleId == 1)  // 根据所选角色id 更换对应技能图片和启用对应角色脚本
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
        // 使用对应的角色技能图标
        Component.FindObjectOfType<Player>().GetComponent<Animator>().runtimeAnimatorController = seletCache[userRoleId];
        // 背景音乐播放
        Component.FindObjectOfType<Player>().ChangeAudioClip(0, true);

        // 各种数值初始化
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

    // 更新fill Amount即显示的图像数量 0--1  【两张图片，一张技能原图，一张技能图片为黑色，这里改的是黑色图片的参数，来达到技能冷却的效果】
    public void UpdateSkillMaskFill(float val)
    {
        gamingPanel.transform.Find("Inventory/SkillPanel/SkillMask").gameObject.GetComponent<Image>().fillAmount = val;
    }


    // 修改提示文本
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

    // 每过一秒 倒计时减1min
    public void UpdateTheRemaingTime()
    {
        timePass += Time.deltaTime;
        if(timePass > 1.0f)
        {   
            // 时间停止倒计时不捡
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

    


    // 加载结算页面
    public void LoadSettlement()
    {
        // 死亡音乐播放
        Component.FindObjectOfType<Player>().ChangeAudioClip(3, false);
        // 时间流逝改为0
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
        if (!save) // 未保存
        {
            // 更新用户金钱
            Component.FindObjectOfType<Player>().money += moneyIncre;
            Component.FindObjectOfType<Player>().diamond += diamondIncre;
            DataController.SaveUserInfo(Component.FindObjectOfType<Player>().money, Component.FindObjectOfType<Player>().diamond);
            DataController.SaveRankList(score);
            save = true;
        }
        settlement.transform.Find("text_worktime").gameObject.GetComponent<Text>().text = "分数:  " + score.ToString() ;
        settlement.transform.Find("text_playtime").gameObject.GetComponent<Text>().text = "摸鱼时间: " + lazyTime.ToString() + "min";
        settlement.transform.Find("text_money").gameObject.GetComponent<Text>().text = "获得金币: " + (moneyIncre).ToString();
        settlement.transform.Find("text_diamond").gameObject.GetComponent<Text>().text = "获得钻石: " + (diamondIncre).ToString();
    }

    // 结算界面的返回主界面的点击事件
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

    // 每帧都进行文本的更新
    public void UpdateUI()
    {
        // 找到对应子对象并设置文本
        gamingPanel.transform.Find("TimeBG/TheRemaingTime/TheRemaingTimeText").gameObject.GetComponent<Text>().text = "倒计时：" + remaingTime.ToString() + "min";
        gamingPanel.transform.Find("TimeBG/TheWorkingTime/TheWorkingTimeText").gameObject.GetComponent<Text>().text = "工作时长：" + busyTime.ToString() + "min";
        gamingPanel.transform.Find("TimeBG/TheLazyTime/TheLazyTimeText").gameObject.GetComponent<Text>().text = "摸鱼时长：" + lazyTime.ToString() + "min";
        gamingPanel.transform.Find("TimeBG/TheLifeTime/TheLifeTimeText").gameObject.GetComponent<Text>().text = "生存时长：" + lifeTime.ToString() + "min";
        // 加载物品栏
        LoadInventory(Component.FindObjectOfType<Player>().userShopItems);
    }

    // 从文件加载用户物品栏对应的物品
    public void LoadInventory(Dictionary<int, int> userShopItems)
    {
        Transform invent = gamingPanel.transform.Find("Inventory");
        for(int i = 0; i < 7; i++)
        {
            Transform child = invent.GetChild(i);
            child.Find("ItemNum").GetComponent<Text>().text = "×"+ userShopItems[i + 1].ToString(); ;
        }
    }


    // 根据人物状态来增加摸鱼时间 每过1s增加1min
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

    // 根据人物状态来增加工作时间
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


    //改变时间
    public void ChangeTimePass()
    {
        timePass -= 3f;
    }


 
}
