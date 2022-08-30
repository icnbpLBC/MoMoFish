using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        Player touch = player.GetComponent<Player>();

        if (touch != null)
        {
            Component.FindObjectOfType<GamingUIControl>().lazyTime += 5;
            Destroy(gameObject);
        }
    }
}
