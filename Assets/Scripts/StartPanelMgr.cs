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
        // ʱ�������ٶ� Ϊ0ʱ�����л���֡�ʵĹ��ܶ�������ͣ�� Time.deltaTime = 0
        Time.timeScale = 0;
        // ��ȡ��Դ�ļ���
        startPanelPre = Resources.Load("Home/StartPanel", typeof(GameObject)) as GameObject;
        // ��¡����original��ӵ�и�����
        startPanel = Instantiate(startPanelPre, this.transform);
        // ��ӵ���¼�
        startPanel.transform.Find("StartBtn").gameObject.GetComponent<Button>().onClick.AddListener(StartGame);
        startPanel.transform.Find("HelpBtn").gameObject.GetComponent<Button>().onClick.AddListener(HelpTips);
        // ���а�
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
        // ���ض�Ӧ�����ĳ���
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        // ����/ͣ�� GameObject
        startPanel.gameObject.SetActive(false);
        // ѡ������ʼ��
        SelectMgr.Instance.Init();
        // ����ѡ�����
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

    // �̵갴ť����¼�
    public void ShopBtnClick()
    {
        ShopController.Instance.Init();
        ShopController.Instance.OpenShop();
    }
}
