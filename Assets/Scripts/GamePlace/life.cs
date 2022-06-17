using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life : MonoBehaviour
{
    public GameObject obj;
    void OnTriggerEnter2D(Collider2D other)
    {
        Player touch = other.GetComponent<Player>();

        if (touch != null)
        {
            obj.SetActive(false);
            GameObject.Find("NPC_oldmanTwo").GetComponent<AddLife>().create = 2;
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
