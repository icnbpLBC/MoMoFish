using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public enum State // ״̬ 
    {
        Working, // ����
        Dawdling, // ����
        Undefined // δ�����״̬ �����������������״̬��
    }

    public State curState;
    private Animator animator;

    private Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    // ���ڸ�����˾��targetλ��
    public Transform  changefollow;

    // ȷ��ֻ��һ�μ���һֱ�������
    private bool candoit;

    // ��־�Ƿ�������㡾������㼴�ɡ�
    bool flag = false;
    // ��־�Ƿ���Թ���
    bool work = false;

    public float speed = 3.0f;

    public Vector2 lookDirection = new Vector2(0, -1);

    // key Ϊ��Ʒid��valueΪ����
    public Dictionary<int, int> userShopItems;
    //������������
    // ��������
    public int frozenAmmunition;

    // �ӵ�ʵ������ģ��
    private GameObject iceAmmunitionPre;

    float BossWorkSpeedAdd;

    // ����Ľ�Ǯ����
    public int money;
    // �������ʯ����
    public int diamond;
    public AudioClip[] audios;

    private float BossSpeedAdd;//Boss�ٶ�����

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
        BossWorkSpeedAdd = 0f; //�ϰ幤������
    }

    // �����û�����Ʒ����Ǯ
    public void InitUserData()
    {
        money = DataController.GetUserMoney();
        diamond = DataController.GetDiamond();
        userShopItems = DataController.GetShopItemInfo();
    }

    // Update is called once per frame
    // Update�ĵ��ò���Time.timeScaleӰ�죬���ĵ��ü������ʵ����һ֡����ʱ���й�
    void Update()
    {
        Movement();
        DoFish();
        DoWork();
        LookDirectionChange();
        TalkToNPC();//��NPC����
        ReadInputForInventroy();
        UpdateUserData();// ÿ֡�����û�����
        BossSpeedChange();//�ٶ�����
        PlayerSpeedMax();//��ɫ����
    }

    public void UpdateUserData()
    {
        DataController.SaveShopItemInfo(userShopItems);
        DataController.SaveUserInfo(money, diamond);
    }

    // ��ȡ��Ӧ��Ʒ������������
    public void ReadInputForInventroy()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (userShopItems[1] >= 1)  // С����߹����ϰ�����
            {
                userShopItems[1]--;
                Component.FindObjectOfType<GamingUIControl>().busyTimeIncre += 1;
                BossWorkSpeedAdd += 1.0f;
                // 10���ص��ظ�
                IEnumerator enumerator = this.ReturnBusyTimeIncre(1);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // С���������Ч��
        {
            if (userShopItems[2] >= 1)
            {
                userShopItems[2]--;
                Component.FindObjectOfType<GamingUIControl>().lazyTimeIncre += 1;
                // 10���ص��ظ�
                IEnumerator enumerator = this.ReturnLazyTimeIncre(1);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // С������ƶ��ٶ�
        {
            if (userShopItems[3] >= 1)
            {
                userShopItems[3]--;
                speed += 1.0f;
                // 10���ص��ظ�
                IEnumerator enumerator = this.ReturnSpeed(1.0f);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // �����߹����ϰ�����
        {
            if (userShopItems[4] >= 1)
            {
                userShopItems[4]--;
                Component.FindObjectOfType<GamingUIControl>().busyTimeIncre += 2;
                BossWorkSpeedAdd += 2.0f;
                // 10���ص��ظ�
                IEnumerator enumerator = this.ReturnBusyTimeIncre(2);
                StartCoroutine(enumerator);

            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // ����������Ч��
        {
            if (userShopItems[5] >= 1)
            {
                userShopItems[5]--;
                Component.FindObjectOfType<GamingUIControl>().lazyTimeIncre += 2;
                // 10���ص��ظ�
                IEnumerator enumerator = this.ReturnLazyTimeIncre(2);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) // �������ƶ��ٶ�
        {
            if (userShopItems[6] >= 1)
            {
                userShopItems[6]--;
                speed += 2.0f;
                // 10���ص��ظ�
                IEnumerator enumerator = this.ReturnSpeed(2.0f);
                StartCoroutine(enumerator);
            }
        }
        else if (Input.GetKeyDown(KeyCode.J)) // �䶳��ҩ
        {
            if (userShopItems[7] >= 1)
            {
                Launch();
            }

        }
    }

    // �ָ��ٶ�
    IEnumerator ReturnSpeed(float dercre)
    {
        yield return new WaitForSeconds(10.0f);
        speed -= dercre;
    }

    // �ָ���������
    IEnumerator ReturnLazyTimeIncre(int dercre)
    {
        yield return new WaitForSeconds(10.0f);
        Component.FindObjectOfType<GamingUIControl>().lazyTimeIncre -= dercre;

    }

    // �ָ���������

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

        // Mathf.Approximately���Ƚ������������Ƿ�����
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {   
            // ������ ��ʾ�ı䷽��
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        // ��Ӧ����
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

    }
    private void LookDirectionChange()
    {
        if (curState == State.Working && candoit == true)
        {
            // GameObject.Find("ai").GetComponent<AIDestinationSetter>().target = changefollow;
            ChangeWorkingAI();//�ı�AiѰ�ҷ���
            
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


    public void ChangeState(State newState) // ����״̬
    {
        curState = newState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LazyPoint")
        {
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("���ո�ʼ����");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
            flag = true;
        }
        if (collision.gameObject.tag == "Boss" && curState != State.Working) // �ϰ�ץסȥ��
        {
            //Component.FindObjectOfType<GamingUIControl>().LoadSettlement();
            GameObject.Find("BossAi").GetComponent<AiRoot>().CatchPlayer();
            
        }
        if(collision.gameObject.tag == "Robot" && curState != State.Working) // ������ץסȥ��
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

        if (candoit == true && curState == State.Dawdling) // ��֤����״̬
        {
            ChangeAudioClip(1, true);
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("�����С�������");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
            // ��˾�ٶ�һֱ���ӡ���˾�ٶ������ޡ�
            ChangeFishAI();
        }
    }

    // ��������Ƭ�β�����
    public void ChangeAudioClip(int index, bool loop)
    {   
        if(this.GetComponent<AudioSource>().clip != audios[index] || this.GetComponent<AudioSource>().loop != loop) // �б仯�Ÿ���
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
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("�����С�������");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LazyPoint") // �뿪�����
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
        if(collision.gameObject.tag == "WorkPoint") //�����ص�
        {
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("���ո�ʼ����");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(true);
            work = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WorkPoint") // �뿪������
        {
            ChangeState(State.Undefined);
            ChangeAudioClip(0, true);
            Component.FindObjectOfType<GamingUIControl>().ChangeTips("");
            Component.FindObjectOfType<GamingUIControl>().ChangeTipsBg(false);
        }
        ChangeUndefinedAI();//���빤��,AI�ı�
        work = false;
        candoit = false;
    }

    private void ChangeWorkingAI() //����״̬��AI����
    {
        GameObject.Find("BossAi").GetComponent<AIDestinationSetter>().target = changefollow;
        GameObject.Find("BossAi").GetComponent<AILerp>().speed = 2.5f + BossSpeedAdd + BossWorkSpeedAdd;
    }

    private void ChangeUndefinedAI()  //��״̬��AI
    {
        GameObject.Find("BossAi").GetComponent<AIDestinationSetter>().target = this.transform;
        GameObject.Find("BossAi").GetComponent<AILerp>().speed = 2.3f + BossSpeedAdd;
    }

    private void ChangeFishAI()  //����״̬��AI
    {
        GameObject.Find("BossAi").GetComponent<AILerp>().speed = 3.1f + BossSpeedAdd;
    }

    public void ChangeAmmunitionNum(int amount) //�ı��ӵ�����
    {
        userShopItems[7]++;
    }

    void Launch() //��������
    {
        // Vector2.up (0,1) �ӵ�λ��������Ϸ� �����������������ȫ���뼴���򲻱䡣
        GameObject ice = Instantiate(iceAmmunitionPre, rigidbody2d.position + Vector2.up * 0.25f, Quaternion.identity);

        // �ӵ��Ϲ��صĽű�
        iceAttack ices = ice.GetComponent<iceAttack>();
        // �ڶ�Ӧ�������
        ices.Launch(lookDirection, 300f);
        // ��Ӧ��Ʒ��-1
        userShopItems[7]--;
    }


    void TalkToNPC()//��NPC�Ի�
    {
        if (Input.GetKeyDown(KeyCode.E))
        {   
            // ����Ӧ���򷢳�����
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();//��ͷ1��
                if (character != null)
                {
                    character.DisplayDialog();
                }
                AddLife life = hit.collider.GetComponent<AddLife>();//��ͷ2��
                if (life != null)
                {
                    life.DisplayDialog();
                }
            }
        }
    }

    // ��������
    public void DieHandle()
    {   
        //������������
        animator.SetBool("Catch", true);
        //�ýű��ر�
        this.enabled = false;
    }

    //�ı�boos�ٶ�
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

    // ����ٶ������޺�����
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
