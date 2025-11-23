using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    Animator aR;
    void Start()
    {
        aR = GetComponent<Animator>(); 
    }

    
    void Update()
    {
        Attack();
    }
    void Attack()
    {
        if (!Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0))
        {
            aR.SetTrigger("Fire1");
        } else if(Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0))
        {
            aR.SetTrigger("Fire2");
        }
    }
}
