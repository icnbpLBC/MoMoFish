using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        Player touch = other.GetComponent<Player>();

        if (touch != null)
        {
            touch.ChangeAmmunitionNum(1); 
            Destroy(gameObject);
            Component.FindObjectOfType<NPC>().createIce = true; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
