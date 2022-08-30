using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;

    float speed = 3.0f;//��ʼ�ٶ�

    Vector2 lookDirection = new Vector2(1, 0);

    float horizontal;//����
    float vertical;//����
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d  = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");//��ȡ����λ��
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);//�õ��µ�λ��

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))//�ж��Ƿ��ƶ�
        {
            lookDirection.Set(move.x, move.y);//�����ƶ�����
            lookDirection.Normalize();//ʹ����Ϊ1�����÷���
        }
        animator.SetFloat("Look X", lookDirection.x);//����
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);//���ظ������ĳ��ȡ���ֻ���� ��������Ϊ(x * x + y * y + z * z) ��ƽ������

        
    }


    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;//��¼֮ǰλ��
        position.x = position.x + speed * horizontal * Time.deltaTime;//λ�ñ仯
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);//��ǰλ��
    }

}
