using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYeBi : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.K) && flag == true)
        {
            Debug.Log("-11");
            GameObject.Find("player").GetComponent<Player>().ChangePlayerSpeed(3.0f);
            flag = false;
            // 3秒后速度复原
            StartCoroutine("SpeedRetrun", 3);
            skillCoolTimer = 0.0f;
        }
    }

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

    IEnumerator SpeedRetrun()
    {
        yield return new WaitForSeconds(3.0f);
        GameObject.Find("player").GetComponent<Player>().ChangePlayerSpeed(-3.0f);
    }

   
    
}
