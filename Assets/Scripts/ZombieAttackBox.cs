using UnityEngine;

public class ZombieAttackBox : MonoBehaviour
{

    public int damage = 20;
    private bool canHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                //Debug.Log("Hitedddd");
                health.TakeDamage(damage, transform.position);
                canHit = false;
            }
        }
    }

    public void EnableHit()
    {
        canHit = true;
    }

    public void DisableHit()
    {
        canHit = false;
    }
}
