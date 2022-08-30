using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public enum State // 状态 
    {
        Working, // 工作
        Dawdling, // 摸鱼
        Undefined // 未定义的状态 【除工作和摸鱼外的状态】
    }

    public State curState;
    private Animator animator;

    private Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    // 用于更新上司的target位置
    public Transform  changefollow;

    // 确保只按一次即可一直摸鱼或工作
    private bool candoit;

    // 标志是否可以摸鱼【到摸鱼点即可】
    bool flag = false;
    // 标志是否可以工作
    bool work = false;

    public float speed = 3.0f;

    public Vector2 lookDirection = new Vector2(0, -1);

    // key 为物品id，value为个数
    public Dictionary<int, int> userShopItems;
    //导弹设置区域
    // 导弹个数
    public int frozenAmmunition;

    // 子弹实例化的模板
    private GameObject iceAmmunitionPre;

    float BossWorkSpeedAdd;

    // 人物的金钱数量
    public int money;
    // 人物的钻石数量
    public int diamond;
    public AudioClip[] audios;

    private float BossSpeedAdd;//Boss速度增长

    // Start is called before the first frame update
    void Start()
    {
        frozenAmmunition = 1;
        iceAmmunitionPre = Resources.Load("Gaming/FrozenBullet", typeof(GameObject)) as GameObject;
        rigidbody2d  = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        curState = State.Undefined;
        candoit = false;
        InitUserData();
        BossWorkSpeedAdd = 0f; //老板工作增速
    }

    // 加载用户的物品、金钱
    public void InitUserData()
    {
        money = DataController.GetUserMoney();
        diamond = DataController.GetDiamond();
        userShopItems = DataController.GetShopItemInfo();
    }

    // Update is called once per frame
    // Update的调用不受Time.timeScale影响，它的调用间隔和真实的上一帧调用时间有关
    void Update()
    {
        Movement();
        DoFish();
        DoWork();
        LookDirectionChange();
        TalkToNPC();//与NPC交流
        ReadInputForInventroy();
        UpdateUserData();// 每帧更新用户数据
        BossSpeedChange();//速度增长
        PlayerSpeedMax();//角色限速
    }

    public void UpdateUserData()
    {
        DataController.SaveShopItemInfo(userShopItems);
        DataController.SaveUserInfo(money, diamond);
    }

    // 读取对应物品栏按键的输入
    public void ReadInputForInventroy()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (userShopItems[1] >= 1)  // 小幅提高工作老板移速
            {
                userShopItems[1]--;
                Component.FindObjectOfType<GamingUIControl>().busyTimeIncre += 1;
                BossWorkSpeedAdd += 1.0f;
                // 10秒后回调回复
                IEnumerator enumerator = this.ReturnBusyTimeIncre(1);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // 小幅提高摸鱼效率
        {
            if (userShopItems[2] >= 1)
            {
                userShopItems[2]--;
                Component.FindObjectOfType<GamingUIControl>().lazyTimeIncre += 1;
                // 10秒后回调回复
                IEnumerator enumerator = this.ReturnLazyTimeIncre(1);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // 小幅提高移动速度
        {
            if (userShopItems[3] >= 1)
            {
                userShopItems[3]--;
                speed += 1.0f;
                // 10秒后回调回复
                IEnumerator enumerator = this.ReturnSpeed(1.0f);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // 大幅提高工作老板移速
        {
            if (userShopItems[4] >= 1)
            {
                userShopItems[4]--;
                Component.FindObjectOfType<GamingUIControl>().busyTimeIncre += 2;
                BossWorkSpeedAdd += 2.0f;
                // 10秒后回调回复
                IEnumerator enumerator = this.ReturnBusyTimeIncre(2);
                StartCoroutine(enumerator);

            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // 大幅提高摸鱼效率
        {
            if (userShopItems[5] >= 1)
            {
                userShopItems[5]--;
                Component.FindObjectOfType<GamingUIControl>().lazyTimeIncre += 2;
                // 10秒后回调回复
                IEnumerator enumerator = this.ReturnLazyTimeIncre(2);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) // 大幅提高移动速度
        {
            if (userShopItems[6] >= 1)
            {
                userShopItems[6]--;
                speed += 2.0f;
                // 10秒后回调回复
                IEnumerator enumerator = this.ReturnSpeed(2.0f);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.J)) // 冷冻弹药
        {
            if (userShopItems[7] >= 1)
            {
                Launch();
            }

        }
    }

    // 恢复速度
    IEnumerator ReturnSpeed(float dercre)
    {
        yield return new WaitForSeconds(10.0f);
        speed -= dercre;
    }

    // 恢复摸鱼增量
    IEnumerator ReturnLazyTimeIncre(int dercre)
    {
        yield return new WaitForSeconds(10.0f);
        Component.FindObjectOfType<GamingUIControl>().lazyTimeIncre -= dercre;

    }

    // 恢复工作增量

    IEnumerator ReturnBusyTimeIncre(int dercre)
    {
        yield return new WaitForSeconds(10.0f);
        Component.FindObjectOfType<GamingUIControl>().busyTimeIncre -= dercre;
        BossWorkSpeedAdd -= dercre;
    }

    private void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        // Mathf.Approximately：比较两个浮点数是否相似
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {   
            // 不相似 表示改变方向
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        // 对应动画
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

    }
    private void LookDirectionChange()
    {
        if (curState == State.Working && candoit == true)
        {
            // GameObject.Find("ai").GetComponent<AIDestinationSetter>().target = changefollow;
            ChangeWorkingAI();//改变Ai寻找方向
            
            /*AIDestinationSetter.target = changefollow;*/
            lookDirection = new Vector2(0, -1);
        }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
        
    }


    public void ChangeState(State newState) // 更改状态
    {
        curState = newState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LazyPoint")
        {
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("按空格开始摸鱼");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
            flag = true;
        }
        if (collision.gameObject.tag == "Boss" && curState != State.Working) // 老板抓住去世
        {
            //Component.FindObjectOfType<GamingUIControl>().LoadSettlement();
            GameObject.Find("BossAi").GetComponent<AiRoot>().CatchPlayer();
            
        }
        if(collision.gameObject.tag == "Robot" && curState != State.Working) // 机器人抓住去世
        {
            Component.FindObjectOfType<GamingUIControl>().LoadSettlement();
        }
    }
    private void DoFish()
    {
        if (Input.GetKeyDown(KeyCode.Space) && flag)
        {
            ChangeState(State.Dawdling);
            candoit = true;
        }

        if (candoit == true && curState == State.Dawdling) // 保证摸鱼状态
        {
            ChangeAudioClip(1, true);
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("摸鱼中。。。。");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
            // 上司速度一直增加【上司速度有上限】
            ChangeFishAI();
        }
    }

    // 更改音乐片段并播放
    public void ChangeAudioClip(int index, bool loop)
    {   
        if(this.GetComponent<AudioSource>().clip != audios[index] || this.GetComponent<AudioSource>().loop != loop) // 有变化才更改
        {
            this.GetComponent<AudioSource>().clip = audios[index];
            this.GetComponent<AudioSource>().loop = loop;
            this.GetComponent<AudioSource>().Play();
        }
        
    }

    private void DoWork()
    {
        if (Input.GetKeyDown(KeyCode.Space) && work)
        {
            ChangeState(State.Working);
            candoit = true;
        }

        if (candoit == true && curState == State.Working)
        {
            ChangeAudioClip(2, true);
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("工作中。。。。");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LazyPoint") // 离开摸鱼点
        {
            ChangeState(State.Undefined);
            ChangeAudioClip(0, true);
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(false);
            ChangeUndefinedAI();
        }
        flag = false;
        candoit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "WorkPoint") //工作地点
        {
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("按空格开始工作");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
            work = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WorkPoint") // 离开工作点
        {
            ChangeState(State.Undefined);
            ChangeAudioClip(0, true);
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(false);
        }
        ChangeUndefinedAI();//脱离工作,AI改变
        work = false;
        candoit = false;
    }

    private void ChangeWorkingAI() //工作状态下AI动作
    {
        GameObject.Find("BossAi").GetComponent<AIDestinationSetter>().target = changefollow;
        GameObject.Find("BossAi").GetComponent<AILerp>().speed = 2.5f + BossSpeedAdd + BossWorkSpeedAdd;
    }

    private void ChangeUndefinedAI()  //无状态下AI
    {
        GameObject.Find("BossAi").GetComponent<AIDestinationSetter>().target = this.transform;
        GameObject.Find("BossAi").GetComponent<AILerp>().speed = 2.3f + BossSpeedAdd;
    }

    private void ChangeFishAI()  //摸鱼状态下AI
    {
        GameObject.Find("BossAi").GetComponent<AILerp>().speed = 3.1f + BossSpeedAdd;
    }

    public void ChangeAmmunitionNum(int amount) //改变子弹数量
    {
        userShopItems[7]++;
    }

    void Launch() //导弹发射
    {
        // Vector2.up (0,1) 子弹位置在玩家上方 对象与世界轴或父轴完全对齐即方向不变。
        GameObject ice = Instantiate(iceAmmunitionPre, rigidbody2d.position + Vector2.up * 0.25f, Quaternion.identity);

        // 子弹上挂载的脚本
        iceAttack ices = ice.GetComponent<iceAttack>();
        // 在对应方向加力
        ices.Launch(lookDirection, 300f);
        // 对应物品数-1
        userShopItems[7]--;
    }


    void TalkToNPC()//跟NPC对话
    {
        if (Input.GetKeyDown(KeyCode.E))
        {   
            // 朝对应方向发出射线
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();//老头1号
                if (character != null)
                {
                    character.DisplayDialog();
                }
                AddLife life = hit.collider.GetComponent<AddLife>();//老头2号
                if (life != null)
                {
                    life.DisplayDialog();
                }
            }
        }
    }

    // 死亡处理
    public void DieHandle()
    {   
        //人物死亡动画
        animator.SetBool("Catch", true);
        //该脚本关闭
        this.enabled = false;
    }

    //改变boos速度
    void BossSpeedChange()
    {
        int i = Component.FindObjectOfType<GamingUIControl>().lifeTime;
        int res = i / 30;
        BossSpeedAdd = res * 0.3f;
    }

    public void ChangePlayerSpeed(float s)
    {
        speed += s;
    }

    // 玩家速度有上限和下限
    void PlayerSpeedMax()
    {
        if(speed >= 10.0f)
        {
            speed = 10.0f;
        }else if(speed <= 3.0f)
        {
            speed = 3.0f;
        }
    }
}
