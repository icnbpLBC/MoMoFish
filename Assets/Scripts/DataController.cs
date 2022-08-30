using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController
{   
    // 数据文件名

    // 持有商品数据：行号-id，行内容：个数
    public static string shopTxtName = "ShopInfo.txt"; 
    public static string userTxtName = "UserInfo.txt";
    public static string rankTxtName = "RankList.txt";

    public static void WriteData(string txtName, List<int> list)                       //用于写入数据,参数为文件名(带后缀) + 数据列表
    {
        string[] strs = new string[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            strs[i] = list[i].ToString();
        }
        string path = Application.streamingAssetsPath + "/Json/"+ txtName;
        File.WriteAllLines(path, strs); // 覆盖行
    }

    public static List<int> ReadData(string txtName)
    {
        List<int> list = new List<int>();
        string path = Application.streamingAssetsPath + "/Json/" + txtName;
        //逐行读取返回的为数组数据
        string[] strs = File.ReadAllLines(path);
        foreach (string item in strs)
        {
            list.Add(int.Parse(item));
        }
        return list;
    }

    // 获取当前账户金钱
    public static int GetUserMoney()
    {
        List<int> list = ReadData(userTxtName);
        return list[0];
    }

    public static int GetDiamond()
    {
        List<int> list = ReadData(userTxtName);
        return list[1];
    }

    public static void SaveUserInfo(int money, int diamond)
    {
        List<int> list = new List<int>();
        list.Add(money);
        list.Add(diamond);
        WriteData(userTxtName, list);
    }

    public static Dictionary<int, int> GetShopItemInfo()
    {
        List<int> list = ReadData(shopTxtName);
        Dictionary<int, int> dict = new Dictionary<int, int>();
        for(int i = 0; i < list.Count; i++)
        {
            dict.Add(i + 1, list[i]);
        }
        return dict;
    }

    public static void SaveShopItemInfo(Dictionary<int, int> dict)
    {   // 对应个数
        List<int> list = new List<int>(dict.Values);
        WriteData(shopTxtName,list);
    }

    public static Dictionary<int,int> GetRankList()
    {
        List<int> list = ReadData(rankTxtName);
        Dictionary<int, int> dict = new Dictionary<int, int>();
        for (int i = 0; i < list.Count; i++)
        {
            dict.Add(i + 1, list[i]);
        }
        return dict;
    }

    public static void SaveRankList(int newRank)
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();
        List<int> list = new List<int>(GetRankList().Values);
        List<int> listans = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            queue.Add(list[i]);
        }
        queue.Add(newRank);
        for(int i = queue.Size()-1; i >=0 && i >= queue.Size()-10 ; i--)
        {
            listans.Add(queue.PeekLast());
            queue.PopLast();
        }
        WriteData(rankTxtName, listans);

    }
}
