using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRedTest : MonoBehaviour
{
    Rigidbody2D playerRed;

    // 0：可以放技能，1：第一次释放，2：第二次释放
    int flag;

    public GameObject qizhi;
    // 技能冷却时间
    private float skillCoolTime;
    // 技能冷却计时器
    private float skillCoolTimer;
    // Start is called before the first frame update
    void Start()
    {
        playerRed = GetComponent<Rigidbody2D>();
        flag = 0;
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
        if (Input.GetKeyDown(KeyCode.K) && flag == 1)
        {
            playerRed.transform.position = qizhi.transform.position;
            qizhi.transform.position = new Vector2(-100, -200);
            flag = 2;
            skillCoolTimer = 0;
        }
        else if(Input.GetKeyDown(KeyCode.K) && flag == 0)
        {
            flag = 1;
            qizhi.transform.position = playerRed.transform.position;
        }
    }

    public void UpdateSkillImgFill()
    {
        if(flag == 2)
        {
            skillCoolTimer += Time.deltaTime;
            float fillAmount = (skillCoolTime - skillCoolTimer) / skillCoolTime;
            Component.FindObjectOfType<GamingUIControl>().UpdateSkillMaskFill(fillAmount);
            if(fillAmount <= 0) // 冷却时间已过
            {
                flag = 0;
            }
        }
    }

    IEnumerator Demo(int i)
    {
        //代码块
        yield return new WaitForSeconds(20.0f);
        //代码块
        flag = 0;
    }
}
