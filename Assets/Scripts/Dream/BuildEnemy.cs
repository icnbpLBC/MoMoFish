using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildEnemy : MonoBehaviour
{
    public Transform topBorder;
    public Transform bottomBorder;
    public Transform leftBorder;
    public Transform rightBorder;

    private GameObject enemyPre;
    private Transform playerTransform;
    private bool canBuild;
    private float liveTime;
    private float buildTime;
    // Start is called before the first frame update
    void Start()
    {
        canBuild = true;
        liveTime = 0;
        buildTime = 2.0f;
        playerTransform = GameObject.Find("Player").transform;
        enemyPre = Resources.Load("Dream/Enemy", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        BuildEnemyAtRadio();
    }

    public void UpdateTime()
    {
        liveTime += Time.deltaTime;
        // 生存时间超过9秒则每0.5秒生成一个怪
        if (liveTime > 9.0f)
        {
            buildTime = 0.5f;
        }
        else
        {
            buildTime = 2.0f - (liveTime / 10);
        }
    }

    public void BuildEnemyAtRadio()
    {
        if (canBuild)
        {   
            canBuild = false; // 确保敌人生成才能生成下一个
            Vector3 temp = BuildEnemyPostion();
            // 不能用成员变量。。
            float tempTime = buildTime;
            StartCoroutine(BuildEnemyAfterTime(temp, tempTime));
        }
        
    }

    public Vector3 BuildEnemyPostion()
    {   
        // 确保找到合法的位置
        bool flag = false;
        float x = 0;
        float y = 0;
        while (!flag)
        {
            x = Random.RandomRange(leftBorder.position.x, rightBorder.position.x);
            y = Random.RandomRange(topBorder.position.y, bottomBorder.position.y);
            flag = CheckPostionValid(new Vector2(x, y));
        }
        return new Vector3(x, y, 0);
    }

    // 确保生成的敌人不能靠玩家太近
    public bool CheckPostionValid(Vector2 postion)
    {
        if(postion.y > playerTransform.position.y + 6.5)
        {
            return true;
        }
        if (postion.y < playerTransform.position.y - 4.5)
        {
            return true;
        }
        if(postion.x > playerTransform.position.x + 5.5)
        {
            return true;
        }
        if (postion.x < playerTransform.position.x - 5.5)
        {
            return true;
        }
        return false;
    }

    IEnumerator BuildEnemyAfterTime(Vector3 position, float tempTime)
    {
        
        yield return new WaitForSeconds(tempTime);
        GameObject o = GameObject.Instantiate(enemyPre, this.transform);
        o.GetComponent<AIDestinationSetter>().target = playerTransform;
        // 设置世界坐标
        o.transform.position = position;
        canBuild = true;
    }

    public void StopBuildEnemy()
    {
        this.canBuild = false;
    }
}
