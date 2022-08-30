using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;

    float speed = 3.0f;//初始速度

    Vector2 lookDirection = new Vector2(1, 0);

    float horizontal;//左右
    float vertical;//上下
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d  = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");//获取输入位置
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);//得到新的位置

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))//判断是否移动
        {
            lookDirection.Set(move.x, move.y);//设置移动参数
            lookDirection.Normalize();//使长度为1，设置方向
        }
        animator.SetFloat("Look X", lookDirection.x);//传参
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);//返回该向量的长度。（只读） 向量长度为(x * x + y * y + z * z) 的平方根。

        
    }


    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;//记录之前位置
        position.x = position.x + speed * horizontal * Time.deltaTime;//位置变化
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);//当前位置
    }

}
