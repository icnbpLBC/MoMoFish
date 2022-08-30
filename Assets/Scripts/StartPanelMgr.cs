using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartPanelMgr : MonoBehaviour
{
    GameObject startPanelPre;
    GameObject startPanel;
    GameObject gameHelp;
    GameObject gameScore;
    // Start is called before the first frame update
    void Start()
    {
        // 时间流逝速度 为0时，所有基于帧率的功能都将被暂停。 Time.deltaTime = 0
        Time.timeScale = 0;
        // 读取资源文件。
        startPanelPre = Resources.Load("Home/StartPanel", typeof(GameObject)) as GameObject;
        // 克隆物体original，拥有父物体
        startPanel = Instantiate(startPanelPre, this.transform);
        // 添加点击事件
        startPanel.transform.Find("StartBtn").gameObject.GetComponent<Button>().onClick.AddListener(StartGame);
        startPanel.transform.Find("HelpBtn").gameObject.GetComponent<Button>().onClick.AddListener(HelpTips);
        // 排行版
        startPanel.transform.Find("ScoreBtn").gameObject.GetComponent<Button>().onClick.AddListener(ScoreTips);
        startPanel.transform.Find("ShopBtn").gameObject.GetComponent<Button>().onClick.AddListener(ShopBtnClick);
        startPanel.transform.Find("ChallengeBtn").gameObject.GetComponent<Button>().onClick.AddListener(ChallengeBtnClick);
        gameHelp = startPanel.transform.Find("GameHelp").gameObject;
        gameHelp.transform.Find("Back").gameObject.GetComponent<Button>().onClick.AddListener(ClosHelpTis);
        gameScore = startPanel.transform.Find("GameScore").gameObject;
        gameScore.transform.Find("Back").gameObject.GetComponent<Button>().onClick.AddListener(CloseScoreTis);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChallengeBtnClick()
    {   
        // 加载对应索引的场景
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        // 激活/停用 GameObject
        startPanel.gameObject.SetActive(false);
        // 选择界面初始化
        SelectMgr.Instance.Init();
        // 进入选择界面
        SelectMgr.Instance.ChangeSelectCharActive(true);
    }

    public void ChangeStartPanelActive(bool active)
    {
        startPanel.gameObject.SetActive(active);
    }

    public void HelpTips()
    {
        gameHelp.SetActive(true);
        gameScore.SetActive(false);
    }

    public void ClosHelpTis()
    {
        gameHelp.SetActive(false);
    }

    public void ScoreTips()
    {
        gameHelp.SetActive(false);
        gameScore.SetActive(true);
    }


    public void CloseScoreTis()
    {
        gameScore.SetActive(false);
    }

    // 商店按钮点击事件
    public void ShopBtnClick()
    {
        ShopController.Instance.Init();
        ShopController.Instance.OpenShop();
    }
}
