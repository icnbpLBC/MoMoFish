using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回调工具
public class CallBackUtil
{

    public delegate void callback();
    public static void CatchPlayer(int a, int b, callback call)
    {
        
        call();//结算页面
    }

    //显示结算页面
    
}
