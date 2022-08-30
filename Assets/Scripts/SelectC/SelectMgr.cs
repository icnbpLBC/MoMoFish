using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMgr
{
    GameObject seleCharPre;
    GameObject itemPre;
    GameObject seleChar;
    // 挂载的父对象位置
    Transform rootCanTrs;
    // 单例
    private static SelectMgr _instance;
    // 选择的角色id
    public int userRoleId = -1;
    // 角色id
    public List<int> userRoles = new List<int>();
    // 角色item
    public List<GameObject> roles = new List<GameObject>();

    // 角色技能描述
    public List<string> skillInfos = new List<string>();
    public static SelectMgr Instance // 属性获取
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SelectMgr();
            }
            return _instance;
        }
    }

    public void Init()
    // 加载对应资源并进行克隆 【重新加载场景时游戏对象，所有加载的物体都将删除，除非被标记，Object.DontDestroyOnLoad(Object)添加标记，为避免重新加载后游戏对象被清除无法使用则加if判断】
    {
        if (rootCanTrs == null)
        {
            rootCanTrs = GameObject.Find("UIRoot/Canvas").transform;
            
        }
        if(seleCharPre == null)
        {
            seleCharPre = Resources.Load("Select/SeleCharaPanel", typeof(GameObject)) as GameObject;
        }
        
        if(seleChar == null)
        {
            // 清空 【避免重新加载场景后roles的元素为null】
            userRoles.Clear();
            roles.Clear();
            seleChar = GameObject.Instantiate(seleCharPre, rootCanTrs);
            seleChar.transform.Find("btn_start").GetComponent<Button>().onClick.AddListener(StartGame);
            seleChar.transform.Find("btn_return").GetComponent<Button>().onClick.AddListener(ReturnClick);
            seleChar.transform.Find("TipsPanel/CloseBtn").GetComponent<Button>().onClick.AddListener(CloseTipsPanel);
        }

        if (userRoles == null || userRoles.Count == 0)
        {   itemPre = Resources.Load("Select/Item", typeof(GameObject)) as GameObject;
            // 获取用户的角色id
            userRoles.Add(0);
            userRoles.Add(1);
            userRoles.Add(2);
            // 角色处理
            for (int i = 0; i < userRoles.Count; i++)
            {
                GameObject o = GameObject.Instantiate(itemPre, seleChar.transform.Find("BackGround"));
                Sprite spr = Resources.Load("Select/"+ userRoles[i].ToString(), typeof (Sprite)) as Sprite;
                o.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = spr;
                roles.Add(o);
                int roleId = userRoles[i];
                // 每个item都添加点击事件监听 并传递对应角色id和item对象参数
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.SelectWindow(roleId, o); });
            }
        }

        if(skillInfos.Count == 0)
        {   
            // 技能描述
            skillInfos.Add("时间停止3秒，期间倒计时停止，上司和机器人不能移动，其他照常，技能冷却20秒。");
            skillInfos.Add("第一次释放摆放旗子到当前位置，第二次释放人物传送到旗子处，技能冷却20秒。");
            skillInfos.Add("大幅提高移动速度3秒，技能冷却20秒。");
        }
    }

    public void ChangeSelectCharActive(bool active)
    {
        seleChar.SetActive(active);
    }

    public void SelectWindow(int roleId, GameObject go)
    {
       
        
        GameObject selecting = go.transform.Find("selecting").gameObject;
        if (selecting.activeSelf)
        {   
            // 技能描述关闭
            SkillTipsChange(roleId, false);
            // item的选择框关闭
            selecting.SetActive(false);
            userRoleId = -1;
        }
        else
        {    // 其他角色选择框关闭
            CleanSelect();
            // 选择当前角色
            selecting.SetActive(true);
            // 提示未选面板关闭
            ChangeTipsPanelActive(false);
            SkillTipsChange(roleId, true);
            userRoleId = roleId;
        }
    }

    public void SkillTipsChange(int roleId, bool active)
    {
        seleChar.transform.Find("CharaTipsPanel").gameObject.SetActive(active);
        // 更新技能描述
        seleChar.transform.Find("CharaTipsPanel/SkillInfo").gameObject.GetComponent<Text>().text = skillInfos[roleId];
    }


    // 选择框关闭
    public void CleanSelect()
    {
        if(roles != null)
        {
            for(int i = 0; i < roles.Count; i++)
            {
                roles[i].transform.Find("selecting").gameObject.SetActive(false);
            }
        }
    }

    // 开始游戏按钮
    public void StartGame()
    {   
        // 根据角色id是否为-1来判断是否开始游戏
        if (userRoleId != -1)
        {
            ChangeSelectCharActive(false);
            // 时间流逝改为1
            Time.timeScale = 1;
            // 游戏中界面控制脚本启用
            GameObject.Find("UIRoot/Canvas").GetComponent<GamingUIControl>().enabled = true;
            // 根据角色id进行相应初始化
            GameObject.Find("UIRoot/Canvas").GetComponent<GamingUIControl>().Init(userRoleId);
        }
        else
        {
            // 提示未选择角色
            ChangeTipsPanelActive(true);
        }
        
    }

    public void ReturnClick()
    {
        ChangeSelectCharActive(false);
        Component.FindObjectOfType<StartPanelMgr>().ChangeStartPanelActive(true);
    }

    public void ChangeTipsPanelActive(bool active)
    {
        seleChar.transform.Find("TipsPanel").gameObject.SetActive(active);
    }

    public void CloseTipsPanel()
    {
        ChangeTipsPanelActive(false);
    }
}
