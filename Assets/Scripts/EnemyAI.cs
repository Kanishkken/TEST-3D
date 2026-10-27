using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]protected float currentHealth;
    protected float maxHealth = 100f;

    public Transform player;
    [SerializeField]private NavMeshAgent agent;
    [SerializeField] protected LayerMask whatIsGround, whatIsPlayer, whatIsObstruction;

    //Patrolling
    [SerializeField]Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField]protected float walkPointRange;

    //Attacking
    [SerializeField]public float attackDamage;
    [SerializeField]protected float timeBetweenAttacks;
    bool alreadyAttacked;
    protected float aggroDuration = 5f;
    private float aggroTimer;
    bool isAggro;
   
    //States
    [SerializeField] protected float sightRange, attackRange;
    [SerializeField]bool playerInSightRange, playerInAttackRange;

    //FOV
    [SerializeField] protected float viewAngle = 120f;
    [SerializeField] bool playerInFOV;
    //bool playerObstructed;

    protected void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

  
    protected void Update()
    {
        //Check for Sight Range and Attack Range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        playerInFOV = PlayerInFOV();
        
        if (playerInSightRange) 
        Debug.Log("Player in FOV: " + playerInFOV);

        //playerObstructed = isPlayerObstructed(); Use this When you want Debug Logs about obstruction not twice.
        // BETTER APPROACH FOR FUTURE
        //if (playerinsightrange)
        //{
        //    playerinfov = playerinfov();
        //    playerobstructed = isplayerobstructed();
        //}
        //else
        //{
        //    playerinfov = false;
        //    playerobstructed = false;
        //}

        if (playerInSightRange && playerInFOV && !IsPlayerObstructed())
        {
            isAggro = true;
            aggroTimer = aggroDuration;

            Debug.Log("Player has a clear line of sight");
            Debug.DrawLine(transform.position, player.position);
        }

        if(isAggro && (!playerInSightRange || !playerInFOV || IsPlayerObstructed()))
        {
            AggroTimerCooldown();
        }
        
        if (!playerInAttackRange && !isAggro)Patrolling();
        if ( isAggro && !playerInAttackRange) ChasePlayer();
        if(isAggro &&playerInAttackRange) AttackPlayer();
    }

    protected bool PlayerInFOV()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        directionToPlayer.y = 0f;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle <= viewAngle / 2f;
    }


    private void AggroTimerCooldown()
    {
        aggroTimer -= Time.deltaTime;

        if (aggroTimer <= 0)
        {
            isAggro = false;
        }
    }

    protected void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if(walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walk Point Reached
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    protected void SearchWalkPoint()
    {
        //Calculate Random Point in Range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Make sure the enemy doesnt fall into ground
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    protected void ChasePlayer()
    {
        Debug.Log("chasing player");
        agent.SetDestination(player.position);
    }

    protected void AttackPlayer()
    {
        //Making Sure Enemy doesnt move
        agent.SetDestination(transform.position);

        //enemy looking at player when attack
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Player playerHealth = player.GetComponent<Player>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);    
        }
    }


    protected bool IsPlayerObstructed()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, whatIsObstruction))
        {
            Debug.Log("Behind WALL");
            return true;
        }

        else
            return false;
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public virtual void TakeDamage(float damage)
    {
        isAggro = true;
        aggroTimer = aggroDuration;

        //Debug.Log("Took Damage: " + damage);

        currentHealth -= damage;

        Debug.Log("current health " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);

        //Debug.Log("Enemy Died");
    }

    protected void OnDrawGizmos()
    {
        //sightRange
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        //attackRange
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //Enemy Forward Direction
        Gizmos.color = Color.gray;
        Gizmos.DrawRay(transform.position, transform.forward * sightRange);

        //FOV Boundries
        Gizmos.color = Color.blue;

        Vector3 leftBoundry = Quaternion.Euler(0f, -viewAngle/2f, 0f) * transform.forward;
        Vector3 rightBoundry = Quaternion.Euler(0f, viewAngle / 2f, 0f) * transform.forward;

        Gizmos.DrawRay(transform.position, leftBoundry * sightRange);
        Gizmos.DrawRay(transform.position, rightBoundry * sightRange);
    }
}
