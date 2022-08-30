using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceAttack : MonoBehaviour
{
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        AiRoot e = other.collider.GetComponent<AiRoot>();
        if (e != null)
        {   
            e.Changemove();  // 非敌人则进行冷冻处理
            
        }
        Destroy(gameObject);
    }
}
