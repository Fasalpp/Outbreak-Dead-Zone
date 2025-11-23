//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyAI : MonoBehaviour
//{
//    public Transform PlayerTarget;
//    float chaceRange = 10f;
//    NavMeshAgent agent;
//    float distanceToPlayer = Mathf.Infinity;
//    private Animator animator;
//    private float attackDistance = 2f;

//    public float rotationSpeed = 5f;
//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        distanceToPlayer = Vector3.Distance(transform.position, PlayerTarget.position);
//        if (distanceToPlayer <= chaceRange && CanEnemySee())
//        {
//            RotateTowardsPlayer();
//            agent.SetDestination(PlayerTarget.position);
//            animator.SetTrigger("Move");
//            if(distanceToPlayer <= attackDistance)
//            {
//                animator.SetBool("Attack",true);
//            }
//            else
//            {
//                animator.SetBool("Attack", false);
//            }
//        }
//        else
//        {
//            animator.SetTrigger("Idle");
//        }
//    }

//    private bool CanEnemySee()
//    {
//        Vector3 direction = (PlayerTarget.position - transform.position).normalized;
//        float distance = Mathf.Min(distanceToPlayer, chaceRange);
//        if(Physics.Raycast(transform.position,direction,out RaycastHit hit, distance))
//        {
//            return hit.transform == PlayerTarget;
//        }
//        return true;
//    }
//    private void RotateTowardsPlayer()
//    {
//        Vector3 direction = (PlayerTarget.position - transform.position).normalized;
//        direction.y = 0; 
//        if (direction.magnitude > 0.01f)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
//        }
//    }
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, chaceRange);
//    }

//}

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTarget;

    [Header("Detection Settings")]
    public float chaseTarget = 5f;
    public float vision = 10f;
    public float turnSpeed = 5f;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked; 
    public float alertRadius = 10f;

    [Header("Roaming Settings")]
    public float roamRadius = 10f;
    public float roamDelay = 3f;
    private float roamTimer = 0f;
    private Vector3 roamTarget;
    private bool hasRoamTarget = false;

    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;
    [SerializeField] private ZombieAttackBox hitBox;

    NavMeshAgent navMeshAgent;
    Animator animator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    private void Update()
    {
        if (playerTarget == null) Roam();
        else
        {
            distanceToTarget = Vector3.Distance(playerTarget.position, transform.position);

            if (isProvoked)
            {
                if (distanceToTarget > chaseTarget * 2.5f)
                {
                    isProvoked = false;
                    navMeshAgent.ResetPath();
                    hasRoamTarget = false;
                    return;
                }

                EngageTarget();
            }
            else if (distanceToTarget <= chaseTarget)
            {
                isProvoked = true;
                hasRoamTarget = false;
            }
            else
            {
                //Roam();
            }
        }
    }

    private void EngageTarget()
    {
        FaceTarget();
        AlertNearbyEnemies();
        navMeshAgent.isStopped = true;
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttack", false);
            if(animator.GetBool("isHeadShot") == true)
            {
                Invoke("Animation", 2f);
            }
            navMeshAgent.speed = 3f;
            navMeshAgent.isStopped = false;
            ActiveSetDestination(playerTarget.position);
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("isAttack", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            if (animator.GetBool("isHeadShot") == true)
            {
                Invoke("Animation", 2f);
            }
            Invoke(nameof(EnableHitZone), 0.8f);
            Invoke(nameof(DisableHitZone), 1f);
            //Debug.Log("Zombie attacks!");
            nextAttackTime = Time.time + attackCooldown;
        }
    }
    void AlertNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alertRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyAI nearbyEnemy = hitCollider.GetComponentInParent<EnemyAI>();
            if (nearbyEnemy != null && nearbyEnemy != this)
            {
                nearbyEnemy.AlertedByOtherEnemy(playerTarget);
            }
        }
    }
    public void AlertedByOtherEnemy(Transform playerTransform)
    {
        if (!isProvoked)
        {
            playerTarget = playerTransform;
            isProvoked = true;
            alertRadius=navMeshAgent.stoppingDistance = chaseTarget * 0.2f;
        }
    }
    private void Roam()
    {
        if (isProvoked) return;
        roamTimer += Time.deltaTime;

        if (!hasRoamTarget || Vector3.Distance(transform.position, roamTarget) < 1f || roamTimer >= roamDelay)
        {
            roamTarget = GetRandomRoamPosition();
            ActiveSetDestination(roamTarget);
            roamTimer = 0f;
            hasRoamTarget = true;
        }

        navMeshAgent.isStopped = false;
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttack", false);
        if (animator.GetBool("isHeadShot") == true)
        {
            Invoke("Animation", 2f);
        } 
    }

    void ActiveSetDestination(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
    }

    private Vector3 GetRandomRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    public void OnDamageTaken(Vector3 hitPoint)
    {
        float distanceToHitPoint = Vector3.Distance(hitPoint, transform.position);

        if (distanceToHitPoint <= chaseTarget)
        {
            isProvoked = true;
            hasRoamTarget = false;
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    void DisableHitZone()
    {
        hitBox.DisableHit();
    }
    void EnableHitZone()
    {
        hitBox.EnableHit();
    }
    void Animation()
    {
        animator.SetBool("isHeadShot", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseTarget);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}

