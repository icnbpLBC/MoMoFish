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
        if(collision.gameObject.tag == "Player" && !enemyAnimaor.GetBool("isDying")) // 碰到玩家
        {
            // 玩家死亡音乐
            Component.FindObjectOfType<Player2>().UpdateAudioClip(1, false);
            Time.timeScale = 0;
            Component.FindObjectOfType<BuildEnemy>().StopBuildEnemy();
            // gameOver
            GameObject.Find("Canvas").GetComponent<PanelMgr>().GameOverActive();
            return;
        }
        else if (collision.gameObject.tag == "Attack" && !enemyAnimaor.GetBool("isDying")) // 被子弹消灭，播放动画停止移动
        {
            Component.FindObjectOfType<PanelMgr>().killNum += 1;
            enemyAnimaor.SetBool("isDying", true);
            this.GetComponent<AIPath>().maxSpeed = 0;
        }
    }

    // 朝向敌人
    void RotateTo()
    {
        if (this.transform.position.x <= player.transform.position.x) // 向右
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 360.0f, 0.0f);
        }
        else // 向左
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    public void DieAnimaClose()
    {
        Destroy(this.gameObject);
    }
}
