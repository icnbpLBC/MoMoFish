using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    private Animator enemyAnimaor;
    private GameObject player;

    
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimaor = this.GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        RotateTo();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && !enemyAnimaor.GetBool("isDying")) // �������
        {
            // �����������
            Component.FindObjectOfType<Player2>().UpdateAudioClip(1, false);
            Time.timeScale = 0;
            Component.FindObjectOfType<BuildEnemy>().StopBuildEnemy();
            // gameOver
            GameObject.Find("Canvas").GetComponent<PanelMgr>().GameOverActive();
            return;
        }
        else if (collision.gameObject.tag == "Attack" && !enemyAnimaor.GetBool("isDying")) // ���ӵ����𣬲��Ŷ���ֹͣ�ƶ�
        {
            Component.FindObjectOfType<PanelMgr>().killNum += 1;
            enemyAnimaor.SetBool("isDying", true);
            this.GetComponent<AIPath>().maxSpeed = 0;
        }
    }

    // �������
    void RotateTo()
    {
        if (this.transform.position.x <= player.transform.position.x) // ����
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 360.0f, 0.0f);
        }
        else // ����
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    public void DieAnimaClose()
    {
        Destroy(this.gameObject);
    }
}
