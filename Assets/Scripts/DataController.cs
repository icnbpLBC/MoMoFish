using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController
{   
    // �����ļ���

    // ������Ʒ���ݣ��к�-id�������ݣ�����
    public static string shopTxtName = "ShopInfo.txt"; 
    public static string userTxtName = "UserInfo.txt";
    public static string rankTxtName = "RankList.txt";

    public static void WriteData(string txtName, List<int> list)                       //����д������,����Ϊ�ļ���(����׺) + �����б�
    {
        string[] strs = new string[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            strs[i] = list[i].ToString();
        }
        string path = Application.streamingAssetsPath + "/Json/"+ txtName;
        File.WriteAllLines(path, strs); // ������
    }

    public static List<int> ReadData(string txtName)
    {
        List<int> list = new List<int>();
        string path = Application.streamingAssetsPath + "/Json/" + txtName;
        //���ж�ȡ���ص�Ϊ��������
        string[] strs = File.ReadAllLines(path);
        foreach (string item in strs)
        {
            list.Add(int.Parse(item));
        }
        return list;
    }

    // ��ȡ��ǰ�˻���Ǯ
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
    {   // ��Ӧ����
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
