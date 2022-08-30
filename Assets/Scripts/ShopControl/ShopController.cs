using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopController
{
    GameObject itemBuyPanelPrefab;
    GameObject shopItemPrefab;
    // ��Ʒ������塾���á�
    GameObject itemBuyPanel;
    GameObject shopPre;
    GameObject shop;
    Transform rootCanTrs;

    // ��Ӧ��3����Ʒ��
    public List<GameObject> shopItems;
    private static ShopController _instance;
    public static ShopController Instance // ���Ի�ȡ
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

    // �̵�����
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
        // �̵������Ʒ
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
            ShopItem.cache.Add(new ShopItem(1, "��ɫҩˮ", "Shop/ShopItem/05", "һ��ʱ���ڣ�С����߹���״̬���ϰ��ȥ���ٶ�", 10));
            ShopItem.cache.Add(new ShopItem(2, "��ɫҩˮ", "Shop/ShopItem/06", "һ��ʱ���ڣ�С���������Ч��", 10));
            ShopItem.cache.Add(new ShopItem(3, "��ɫҩˮ", "Shop/ShopItem/07", "һ��ʱ���ڣ�С������ƶ��ٶ�", 10));
            ShopItem.cache.Add(new ShopItem(4, "��ɫҩˮ", "Shop/ShopItem/08", "һ��ʱ���ڣ������߹���״̬���ϰ��ȥ���ٶ�", 15));
            ShopItem.cache.Add(new ShopItem(5, "��ɫҩˮ", "Shop/ShopItem/09", "һ��ʱ���ڣ�����������Ч��", 15));
            ShopItem.cache.Add(new ShopItem(6, "��ɫҩˮ", "Shop/ShopItem/10", "һ��ʱ���ڣ��������ƶ��ٶ�", 15));
            ShopItem.cache.Add(new ShopItem(7, "�䶳��ҩ", "Shop/ShopItem/11", "һ���䶳��ҩ", 20));
        }
        
        if(itemBuyPanelPrefab == null)
        {
            itemBuyPanelPrefab = Resources.Load("Shop/ItemBuyPanel", typeof(GameObject)) as GameObject;
        }
        
        if(shopItemPrefab == null || shopItems == null || shopItems[0] == null) // ���¼��س���ʱ GO���ᱻ�������
        {
            shopItemPrefab = Resources.Load("Shop/ShopItem", typeof(GameObject)) as GameObject;
            shopItems = new List<GameObject>();
            // �ȿ�¡��������Ϸ���� �����ٸ���ͼƬ
            for (int i = 0; i < 3; i++)
            {
                GameObject o = GameObject.Instantiate(shopItemPrefab, shop.transform.Find("ShopPanel"));
                shopItems.Add(o);
            }
        }
        
        
        
    }

    // �������3����Ʒ
    public void RandomProduceShopItem()
    {
        for(int i = 0; i < 3; i++)
        {
            int id = Random.Range(0, ShopItem.cache.Count);
            GameObject o = shopItems[i];
            Transform itemBg = o.transform.Find("ItemBg");
            // ���ݲ�������Ʒid����ȡ��Ӧ����Ʒ��Ϣ���������չʾ����Ʒ
            itemBg.Find("Item").gameObject.GetComponent<Image>().sprite = Resources.Load(ShopItem.cache[id].itemPath, typeof(Sprite)) as Sprite;
            itemBg.Find("ItemName").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemName;
            itemBg.Find("Price").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemPrice.ToString();
            itemBg.GetComponent<Button>().interactable = true;
            int index = i;
            //lambda���ʽת��Ϊί������  ����¼����ܽ��ղ���
            itemBg.GetComponent<Button>().onClick.AddListener(delegate () { this.ShopItemClick(id, index); });
        }
    }

    // id��Ӧ��Ʒid ��index ������չʾ��Ʒ�е�����
    public void ShopItemClick(int id, int index)
    {
        if(itemBuyPanel == null)
        {
            itemBuyPanel = GameObject.Instantiate(itemBuyPanelPrefab, shop.transform);
            // �ر���Ʒ��������¼�
            itemBuyPanel.transform.Find("Cancel").gameObject.GetComponent<Button>().onClick.AddListener(CloseShopTipsInfo);
        }
        // ��������м����������ظ���ӣ���ΪitemBuyPanel����
        itemBuyPanel.transform.Find("Buy").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        // ��ť�󶨶�Ӧ����
        itemBuyPanel.transform.Find("Buy").gameObject.GetComponent<Button>().onClick.AddListener(delegate () { this.BuyShopItem(id, index); });
        OpenShopTipsInfo();
        // ���¶�Ӧ����Ʒ����
        UpdateShopTipsInfo(id);
    }

    // ����index����
    public void BuyShopItem(int id, int index)
    {   
        if(Component.FindObjectOfType<Player>().money < ShopItem.cache[id].itemPrice)
        {   
            // ��Ǯ�����Ĵ�����ʾһ���ı�
            NoMoneyHandle();
        }
        else
        {
            // ��Ǯ����
            Component.FindObjectOfType<Player>().money -= ShopItem.cache[id].itemPrice;
            // ������Ʒ��
            Component.FindObjectOfType<Player>().userShopItems[id + 1]++;
            // ����ʣ���Ǯ��ʾ��
            ChangeUserMoney();
            // ��Ӧ��Ʒ���� ���ɵ��
            GameObject o = shopItems[index];
            Transform itemBg = o.transform.Find("ItemBg");
            // ���Ǹ���ͼƬ
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

    // ������Ʒ�������
    public void UpdateShopTipsInfo(int id)
    {
        Transform item = itemBuyPanel.transform;
        item.Find("ItemName").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemName;
        item.Find("Info").gameObject.GetComponent<Text>().text = ShopItem.cache[id].itemInfo;
    }

    public void ChangeUserMoney()
    {   
        shop.transform.Find("UserMoney").GetComponent<Text>().text = "��ǰʣ���Ǯ��" + Component.FindObjectOfType<Player>().money.ToString();
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


    // ˢ�°�ť����¼�
   public void RefreshBtnClick()
    {
        if (Component.FindObjectOfType<Player>().money < 5)
        {
            NoMoneyHandle();
        }
        else
        {
            // ������Ǯ
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
