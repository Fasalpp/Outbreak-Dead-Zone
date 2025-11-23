using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] private Rigidbody[] ragdollBodies;
    [SerializeField] private float knockbackForce = 10f;
    //[SerializeField] private float knockbackRadius = 2f;

    public GameObject[] drop;
    private bool isDead = false;

    private Animator animator;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;
    private Vector3 hitPoint;
    void Start()
    {
        animator = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        enemyAI = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();


        SetRagdollState(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(float damage, string animationTag, Vector3 source)
    {
        if (isDead) return;
        health -= damage;
        animator.SetTrigger("gotHit");
        hitPoint = source;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    }
    private void SetRagdollState(bool isRagdoll)
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !isRagdoll;
            rb.useGravity = isRagdoll;
        }
    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.enabled = false;
        if (agent != null) agent.enabled = false;
        if (enemyAI != null) enemyAI.enabled = false;

        AddKillCount killCount = FindAnyObjectByType<AddKillCount>();
        killCount.AddKill();
        SetRagdollState(true);
        ApplyKnockbackForce();
        Drop();
        Destroy(gameObject, 5f);
    }
    private void ApplyKnockbackForce()
    {
        foreach (var rb in ragdollBodies)
        {
            Vector3 direction = (rb.position - hitPoint).normalized;
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }

    void Drop()
    {
        Vector3 position = new Vector3(transform.position.x, 154.2f, transform.position.z);
        if (drop.Length == 0) return;

        int randomIndex = Random.Range(0, drop.Length);
        Instantiate(drop[randomIndex], position, Quaternion.identity);
    }

    //void ShuffleArray(GameObject[] array)
    //{
    //    for (int i = array.Length - 1; i > 0; i--)
    //    {
    //        int rand = Random.Range(0, i + 1);
    //        GameObject temp = array[i];
    //        array[i] = array[rand];
    //        array[rand] = temp;
    //    }
    //}
}
