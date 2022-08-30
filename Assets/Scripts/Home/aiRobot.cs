using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiRobot : MonoBehaviour
{
    public float moveSpeed;
    public float startTime;
    public float waitTime;
    
    public Transform leftPos;
    public Transform rightPos;
    public Transform movePos;

    public Animator anim;
    public bool isTurnLeft = false;

    void Start()
    {

        waitTime = startTime;
        movePos.position = leftPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Component.FindObjectOfType<GamingUIControl>().isTimeStop)
        {
            return;
        }
        transform.position = Vector2.MoveTowards
                                     (transform.position, movePos.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, leftPos.position) < 0.5f)
            if (waitTime <= 0)
            {
                isTurnLeft = false;
                movePos.position = rightPos.position;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        if (Vector2.Distance(transform.position, rightPos.position) < 0.5f)
            if (waitTime <= 0)
            {
                isTurnLeft = true;
                movePos.position = leftPos.position;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        ChangeState();
    }
    public void ChangeActiveAnimaotr(bool active)
    {
        anim.enabled = active;
    }
    void ChangeState() {
        if (isTurnLeft) {
            anim.SetBool("turnLeft", true);
        }
        else
        {
            anim.SetBool("turnLeft", false);
        }
    }
}
