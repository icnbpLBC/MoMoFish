using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRedTest : MonoBehaviour
{
    Rigidbody2D playerRed;

    // 0�����Էż��ܣ�1����һ���ͷţ�2���ڶ����ͷ�
    int flag;

    public GameObject qizhi;
    // ������ȴʱ��
    private float skillCoolTime;
    // ������ȴ��ʱ��
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
            if(fillAmount <= 0) // ��ȴʱ���ѹ�
            {
                flag = 0;
            }
        }
    }

    IEnumerator Demo(int i)
    {
        //�����
        yield return new WaitForSeconds(20.0f);
        //�����
        flag = 0;
    }
}
