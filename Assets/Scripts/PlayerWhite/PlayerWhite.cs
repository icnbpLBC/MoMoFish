using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWhite : MonoBehaviour
{
    public GameObject boss;
    // ������ȴʱ��
    private float skillCoolTime;
    // ������ȴ��ʱ��
    private float skillCoolTimer;
    // �ܷ�ż���
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
        // �ͷż���
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

    // ����Mask����ʾ
    public void UpdateSkillImgFill()
    {
        if (!flag)
        {
            skillCoolTimer += Time.deltaTime;
            float fillAmount = (skillCoolTime - skillCoolTimer) / skillCoolTime;
            Component.FindObjectOfType<GamingUIControl>().UpdateSkillMaskFill(fillAmount);
            if (fillAmount <= 0) // ��ȴʱ���ѹ�
            {
                flag = true;
            }
        }
    }

    IEnumerator BossWalk(int i)
    {
        //�����
        yield return new WaitForSeconds(3.0f);
        //�����
       boss.GetComponent<AILerp>().enabled = true;
        boss.GetComponent<AiRoot>().enabled = true;
        Component.FindObjectOfType<GamingUIControl>().isTimeStop = false;
        Component.FindObjectOfType<aiRobot>().ChangeActiveAnimaotr(true);
    }
}
