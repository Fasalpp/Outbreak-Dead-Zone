using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    private bool isAttacked = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetComponent<Animator>().SetTrigger("Swing");
            isAttacked = true;
        }
        else
        {
            //isAttacked = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && isAttacked)
        {
            Destroy(collision.gameObject);
        }
    }
}
