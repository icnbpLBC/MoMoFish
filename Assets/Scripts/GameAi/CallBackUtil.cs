using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ص�����
public class CallBackUtil
{

    public delegate void callback();
    public static void CatchPlayer(int a, int b, callback call)
    {
        
        call();//����ҳ��
    }

    //��ʾ����ҳ��
    
}
