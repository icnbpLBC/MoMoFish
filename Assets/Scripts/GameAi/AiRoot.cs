using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRoot : MonoBehaviour
{
    public GameObject start;
    private Animator animator;

    float frozentime; //冰冻时间
    // Start is called before the first frame update
    void Start()
    {
        frozentime = 0;
        animator = GameObject.Find("duola").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FrozentimeDelete();
        
    }

    // 被子弹射中 移动和动画关闭
    public void Changemove()
    {
        GameObject.Find("BossAi").GetComponent<AILerp>().enabled = false;
        GameObject.Find("duola").GetComponent<Animator>().enabled = false;
        frozentime = 5;
    }

    // 更新冷冻时间 小于0时回复动画和移动
    void FrozentimeDelete()
    {
        frozentime -= Time.deltaTime;
        if(frozentime < 0)
        {
            GameObject.Find("BossAi").GetComponent<AILerp>().enabled = true;
            GameObject.Find("duola").GetComponent<Animator>().enabled = true;
        }
    }

    public void CatchPlayer()
    {   
        // 老板抓住玩家动画
        animator.SetBool("Catch", true);
        CallBackUtil.CatchPlayer(1, 1, ShowBg);
        
    }
    

    void ShowBg()
    {
        StartCoroutine(Show(1));
    }

    // 协程 1s后展示结算界面
    IEnumerator Show(int i)
    {
        Component.FindObjectOfType<GamingUIControl>().ChangeTimePass();
        // 禁用跟随脚本
        GameObject.Find("BossAi").GetComponent<AIDestinationSetter>().enabled = false;
        GameObject.Find("player").GetComponent<Player>().DieHandle();
        yield return new WaitForSeconds(3.0f);
        Component.FindObjectOfType<GamingUIControl>().LoadSettlement();
    }
}
