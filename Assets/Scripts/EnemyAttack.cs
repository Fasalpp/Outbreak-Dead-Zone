using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth target;
    public float damage = 10;
    void Start()
    {
        target = FindAnyObjectByType<PlayerHealth>();
    }

    public void AttackHitEvnent()
    {
        if(target != null)
        {
            target.TakeDamage(damage, transform.position);
            Debug.Log("Hitting Enemy");
        }
        else
        {
            Debug.Log("Not Hitting");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
