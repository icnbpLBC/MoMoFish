using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopController
{
    GameObject itemBuyPanelPrefab;
    GameObject shopItemPrefab;
    // 物品描述面板【共用】
    GameObject itemBuyPanel;
    GameObject shopPre;
    GameObject shop;
    Transform rootCanTrs;

    // 对应的3个物品项
    public List<GameObject> shopItems;
    private static ShopController _instance;
    public static ShopController Instance // 属性获取
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ShopController();
            }
            return _instance;
        }
    }

    // 商店项类
    public class ShopItem
    {
        public int shopId;
        public string itemName;
        public string itemPath;
        public string itemInfo;
        public int itemPrice;
        public ShopItem(int shopId, string itemName, string itemPath, string itemInfo, int itemPrice)
        {
            this.shopId = shopId;
            this.itemName = itemName;
            this.itemPath = itemPath;
            this.itemPrice = itemPrice;
            this.itemInfo = itemInfo;
        }

        public GameObject gameObject;
        // 商店可卖物品
        public static List<ShopItem> cache = new List<ShopItem>();

    }
    // Start is called before the first frame update
    public void Init()
    {   
        if(rootCanTrs == null)
        {
            rootCanTrs = GameObject.Find("UIRoot/Canvas").transform;
        }
        if(shop == null)
        {
            shopPre = Resources.Load("Shop/Shop", typeof(GameObject)) as GameObject;
            shop = GameObject.Instantiate(shopPre, rootCanTrs);
            shop.transform.Find("RefreshBtn").GetComponent<Button>().onClick.AddListener(RefreshBtnClick);
            shop.transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(CloseShop);
        }
        
        if(ShopItem.cache.Count == 0)
        {
            ShopItem.cache.Add(new ShopItem(1, "红色药水", "Shop/ShopItem/05", "一段时间内，小幅提高工作状态和老板回去的速度", 10));
            ShopItem.cache.Add(new ShopItem(2, "蓝色药水", "Shop/ShopItem/06", "一段时间内，小幅提高摸鱼效率", 10));
            ShopItem.cache.Add(new ShopItem(3, "金色药水", "Shop/ShopItem/07", "一段时间内，小幅提高移动速度", 10));
            ShopItem.cache.Add(new ShopItem(4, "紫色药水", "Shop/ShopItem/08", "一段时间内，大幅提高工作状态和老板回去的速度", 15));
            ShopItem.cache.Add(new ShopItem(5, "绿色药水", "Shop/ShopItem/09", "一段时间内，大幅提高摸鱼效率", 15));
            ShopItem.cache.Add(new ShopItem(6, "橙色药水", "Shop/ShopItem/10", "一段时间内，大幅提高移动速度", 15));
            ShopItem.cache.Add(new ShopItem(7, "冷冻弹药", "Shop/ShopItem/11", "一发冷冻弹药", 20));
        }
        
        if(itemBuyPanelPrefab == null)
        {
            itemBuyPanelPrefab = Resources.Load("Shop/ItemBuyPanel", typeof(GameObject)) as GameObject;
        }
        
        if(shopItemPrefab == null || shopItems == null || shopItems[0] == null) // 重新加载场景时 GO都会被清除。。
        {
            shopItemPrefab = Resources.Load("Shop/ShopItem", typeof(GameObject)) as GameObject;
            shopItems = new List<GameObject>();
            // 先克隆出三个游戏对象 后续再更改图片
            for (int i = 0; i < 3; i++)
            {
                GameObject o = GameObject.Instantiate(shopItemPrefab, shop.transform.Find("ShopPanel"));
                shopItems.Add(o);
            }
        }
        
        
        
    }

    // 随机产生3个商品
    public void RandomProduceShopItem()
    {
        for(int i = 0; i < 3; i++)
        {
            int id = Random.Range(0, ShopItem.cache.Count);
            GameObject o = shopItems[i];
            Transform itemBg = o.transform.Find("ItemBg");
            // 根据产生的商品id来获取对应的商品信息并更改面板展示的商品
            itemBg.Find("Item").gameObject.GetComponent<Image>().sprite = Resources.Load(ShopItem.cache[id].itemPath, typeof(Sprite)) as Sprite;
            itemBg.Find("ItemName").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemName;
            itemBg.Find("Price").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemPrice.ToString();
            itemBg.GetComponent<Button>().interactable = true;
            int index = i;
            //lambda表达式转换为委托类型  点击事件就能接收参数
            itemBg.GetComponent<Button>().onClick.AddListener(delegate () { this.ShopItemClick(id, index); });
        }
    }

    // id对应商品id ，index 在三个展示商品中的索引
    public void ShopItemClick(int id, int index)
    {
        if(itemBuyPanel == null)
        {
            itemBuyPanel = GameObject.Instantiate(itemBuyPanelPrefab, shop.transform);
            // 关闭物品描述点击事件
            itemBuyPanel.transform.Find("Cancel").gameObject.GetComponent<Button>().onClick.AddListener(CloseShopTipsInfo);
        }
        // 先清除所有监听器避免重复添加，因为itemBuyPanel共用
        itemBuyPanel.transform.Find("Buy").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        // 按钮绑定对应处理
        itemBuyPanel.transform.Find("Buy").gameObject.GetComponent<Button>().onClick.AddListener(delegate () { this.BuyShopItem(id, index); });
        OpenShopTipsInfo();
        // 更新对应的物品描述
        UpdateShopTipsInfo(id);
    }

    // 根据index处理
    public void BuyShopItem(int id, int index)
    {   
        if(Component.FindObjectOfType<Player>().money < ShopItem.cache[id].itemPrice)
        {   
            // 金钱不够的处理：显示一段文本
            NoMoneyHandle();
        }
        else
        {
            // 金钱减少
            Component.FindObjectOfType<Player>().money -= ShopItem.cache[id].itemPrice;
            // 塞进物品栏
            Component.FindObjectOfType<Player>().userShopItems[id + 1]++;
            // 更新剩余金钱显示：
            ChangeUserMoney();
            // 对应物品售罄 不可点击
            GameObject o = shopItems[index];
            Transform itemBg = o.transform.Find("ItemBg");
            // 还是更改图片
            itemBg.Find("Item").gameObject.GetComponent<Image>().sprite = Resources.Load("Shop/SoldOut", typeof(Sprite)) as Sprite;
            itemBg.GetComponent<Button>().interactable = false;
            CloseShopTipsInfo();
        }
        
    }

    public void CloseShopTipsInfo()
    {
        itemBuyPanel.SetActive(false);
        shop.transform.Find("NoMoneyTips").gameObject.SetActive(false);
    }

    public void OpenShopTipsInfo()
    {
        itemBuyPanel.SetActive(true);
    }

    // 更改物品面板描述
    public void UpdateShopTipsInfo(int id)
    {
        Transform item = itemBuyPanel.transform;
        item.Find("ItemName").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemName;
        item.Find("Info").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemInfo;
    }

    public void ChangeUserMoney()
    {   
        shop.transform.Find("UserMoney").GetComponent<Text>().text = "当前剩余金钱：" + Component.FindObjectOfType<Player>().money.ToString();
    }

    public void CloseShop()
    {
        shop.SetActive(false);
    }

    public void OpenShop()
    {
        ChangeUserMoney();
        shop.SetActive(true);
        RandomProduceShopItem();
    }


    // 刷新按钮点击事件
   public void RefreshBtnClick()
    {
        if (Component.FindObjectOfType<Player>().money < 5)
        {
            NoMoneyHandle();
        }
        else
        {
            // 点击后扣钱
            Component.FindObjectOfType<Player>().money -= 5;
            ChangeUserMoney();
            RandomProduceShopItem();
        }
        
    }


    public void NoMoneyHandle()
    {
        shop.transform.Find("NoMoneyTips").gameObject.SetActive(true);
    }


}
