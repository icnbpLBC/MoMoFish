using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWhite : MonoBehaviour
{
    public GameObject boss;
    // 技能冷却时间
    private float skillCoolTime;
    // 技能冷却计时器
    private float skillCoolTimer;
    // 能否放技能
    bool flag;
    // Start is called before the first frame update
    void Start()
    {
        flag = true;
        skillCoolTime = 20.0f;
        skillCoolTimer = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        UseBig();
        UpdateSkillImgFill();
    }

    private void UseBig()
    {   
        // 释放技能
        if (Input.GetKeyDown(KeyCode.K) && flag)
        {
            boss.GetComponent<AILerp>().enabled = false;
            boss.GetComponent<AiRoot>().enabled = false;
            GameObject.Find("duola").GetComponent<Animator>().enabled = false;
            StartCoroutine("BossWalk", 3);
            flag = false;
            Component.FindObjectOfType<GamingUIControl>().isTimeStop = true;
            Component.FindObjectOfType<aiRobot>().ChangeActiveAnimaotr(false);
            skillCoolTimer = 0.0f;
        }
    }

    // 更改Mask的显示
    public void UpdateSkillImgFill()
    {
        if (!flag)
        {
            skillCoolTimer += Time.deltaTime;
            float fillAmount = (skillCoolTime - skillCoolTimer) / skillCoolTime;
            Component.FindObjectOfType<GamingUIControl>().UpdateSkillMaskFill(fillAmount);
            if (fillAmount <= 0) // 冷却时间已过
            {
                flag = true;
            }
        }
    }

    IEnumerator BossWalk(int i)
    {
        //代码块
        yield return new WaitForSeconds(3.0f);
        //代码块
       boss.GetComponent<AILerp>().enabled = true;
        boss.GetComponent<AiRoot>().enabled = true;
        Component.FindObjectOfType<GamingUIControl>().isTimeStop = false;
        Component.FindObjectOfType<aiRobot>().ChangeActiveAnimaotr(true);
    }
}
