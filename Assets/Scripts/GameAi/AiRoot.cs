using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRoot : MonoBehaviour
{
    public GameObject start;
    private Animator animator;

    float frozentime; //����ʱ��
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

    // ���ӵ����� �ƶ��Ͷ����ر�
    public void Changemove()
    {
        GameObject.Find("BossAi").GetComponent<AILerp>().enabled = false;
        GameObject.Find("duola").GetComponent<Animator>().enabled = false;
        frozentime = 5;
    }

    // �����䶳ʱ�� С��0ʱ�ظ��������ƶ�
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
        // �ϰ�ץס��Ҷ���
        animator.SetBool("Catch", true);
        CallBackUtil.CatchPlayer(1, 1, ShowBg);
        
    }
    

    void ShowBg()
    {
        StartCoroutine(Show(1));
    }

    // Э�� 1s��չʾ�������
    IEnumerator Show(int i)
    {
        Component.FindObjectOfType<GamingUIControl>().ChangeTimePass();
        // ���ø���ű�
        GameObject.Find("BossAi").GetComponent<AIDestinationSetter>().enabled = false;
        GameObject.Find("player").GetComponent<Player>().DieHandle();
        yield return new WaitForSeconds(3.0f);
        Component.FindObjectOfType<GamingUIControl>().LoadSettlement();
    }
}
