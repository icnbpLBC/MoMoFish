using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PanelMgr : MonoBehaviour
{
    GameObject gameOverPanelPre;
    GameObject gamingPanelPre;
    GameObject gameOverPanel;
    GameObject gamingPanel;
    public int killNum;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        killNum = 0;
        gameOverPanelPre = Resources.Load("Dream/GameOverPanel", typeof(GameObject)) as GameObject;
        gamingPanelPre = Resources.Load("Dream/GamingPanel", typeof(GameObject)) as GameObject;
        gameOverPanel = GameObject.Instantiate(gameOverPanelPre, this.transform);
        gameOverPanel.transform.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(RestartBtnClick);
        gameOverPanel.transform.Find("ReturnBtn").GetComponent<Button>().onClick.AddListener(ReturnBtnClick);
        gameOverPanel.SetActive(false);
        gamingPanel = GameObject.Instantiate(gamingPanelPre, this.transform);
        gamingPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKillNum();
    }

    public void UpdateKillNum()
    {
        gamingPanel.transform.Find("KillNum").gameObject.GetComponent<Text>().text = "��ɱ����" + killNum.ToString();
    }

    public void RestartBtnClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnBtnClick()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOverActive()
    {
        // ���⵲ס��ť
        gamingPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.Find("EarnMoney").gameObject.GetComponent<Text>().text = "������ս׬ȡ��ң�" + (killNum*15).ToString() + "ö";
        // ��������
        DataController.SaveUserInfo(DataController.GetUserMoney() + killNum * 15, DataController.GetDiamond());
    }
}
