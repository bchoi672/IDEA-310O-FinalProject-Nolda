using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Nolda.EnemyManager
{
    public class EnemyAI : MonoBehaviour
    {
        #region Class Variables
        public NavMeshAgent navMeshAgent;
        Animator animator;
        private static int isStoppedHash = Animator.StringToHash("isStopped");
        private static int isWalkingHash = Animator.StringToHash("isWalking");
        private static int isChasingHash = Animator.StringToHash("isChasing");
        private static int isAttackingHash = Animator.StringToHash("isAttacking");
        

        public float startWaitTime = 4; 
        public float timeToRotate = 2;
        public float speedWalk = 6;
        public float speedRun = 9;

        public float viewRadius = 15;
        public float viewAngle = 90;
        [SerializeField] float attackRange;
        public LayerMask playerMask;
        public LayerMask obstacleMask;
        public float meshResolution = 1f;
        public int edgeIterations = 4;
        public float edgeDistance = 0.5f;

        public Transform[] waypoints;
        int m_CurrentWaypointIndex;

        Vector3 playerLastPosition = Vector3.zero;
        Vector3 m_PlayerPosition;

        float m_WaitTime;
        float m_TimeToRotate;
        bool m_PlayerInRange;
        bool m_PlayerInAttackRange;
        bool m_PlayerNear;
        bool m_IsPatrol;
        bool m_CaughtPlayer; 
        #endregion

        #region Start And Update
        void Start()
        {
            m_PlayerPosition = Vector3.zero;
            m_IsPatrol = true;
            m_CaughtPlayer = false;
            m_PlayerInRange = false;
            m_WaitTime = startWaitTime;
            m_TimeToRotate = timeToRotate;

            m_CurrentWaypointIndex = 0;
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speedWalk;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }

        void Update()
        {
            EnvironmentView();
            m_PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

            if(!m_IsPatrol)
            {
                Chasing();
            }
            else
            {
                Patrolling();
            }
        }
        #endregion

        #region Enemy State Logic
        private void Chasing()
        {
            if (!m_CaughtPlayer)
            {
                Move(speedRun);
                navMeshAgent.SetDestination(m_PlayerPosition);
                if(m_PlayerInAttackRange && m_PlayerInRange)
                {   
                    Stop();
                    Attack();
                }
            }
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if(m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6)
                {
                    m_IsPatrol = true;
                    m_PlayerNear = false;
                    Move(speedWalk);
                    m_TimeToRotate = timeToRotate;
                    m_WaitTime = startWaitTime;
                    navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                }
                else
                {
                    if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    {
                        Stop();
                        m_WaitTime -= Time.deltaTime;
                    }
                }
            }
        }

        private void Patrolling()
        {
            if(m_PlayerNear)
            {
                    if (m_TimeToRotate <= 0)
                    {
                        Move(speedWalk);
                        LookingPlayer(playerLastPosition);
                    }
                    else
                    {
                        Stop();
                        m_TimeToRotate -= Time.deltaTime;
                    }
            }
            else
            {
                m_PlayerNear = false;
                playerLastPosition = Vector3.zero;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if(m_WaitTime <= 0)
                    {
                        NextPoint();
                        Move(speedWalk);
                        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                        m_WaitTime = startWaitTime;
                    }
                    else{
                        Stop();
                        m_WaitTime -= Time.deltaTime;
                    }
                }
            }
        }
        #endregion

        #region Enemy Actions 
        void Attack()
        {
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("WK_heavy_infantry_09_attack_B"))
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.speed = 0;
                animator.SetBool(isAttackingHash, true);
                navMeshAgent.SetDestination(transform.position);
            }
        }

        void Move(float speed)
        {
            AnimationStateReset();
            if(speed == speedWalk)
            {
                animator.SetBool(isWalkingHash, true);
            } 
            else
            {
                animator.SetBool(isChasingHash, true);
            }
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
        }

        void Stop()
        {
            AnimationStateReset();
            animator.SetBool(isStoppedHash, true);
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;
        }

        public void NextPoint()
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        }

        #endregion

        #region Enemy View
        void LookingPlayer(Vector3 player)
        {
            navMeshAgent.SetDestination(player);
            if(!navMeshAgent.pathPending && Vector3.Distance(transform.position, player) <= 0.3)
            {
                if(m_WaitTime <= 0)
                {
                    m_PlayerNear = false;
                    Move(speedWalk);
                    navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                    m_WaitTime = startWaitTime;
                    m_TimeToRotate = timeToRotate;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }

        void EnvironmentView()
        {
            Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

            for (int i = 0; i < playerInRange.Length; i++)
            {
                Transform player = playerInRange[i].transform;
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                {
                    float dstToPlayer = Vector3.Distance(transform.position, player.position);
                    if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                    {
                        m_PlayerInRange = true;
                        m_IsPatrol = false;
                    }
                    else
                    {
                        m_PlayerInRange = false;
                    }
                }
                if(Vector3.Distance(transform.position, player.position) > viewRadius)
                {
                    m_PlayerInRange = false;
                }
            
                if(m_PlayerInRange)
                {
                    m_PlayerPosition = player.transform.position;
                }
            }
        
        }

        void AnimationStateReset()
        {
            animator.SetBool(isStoppedHash, false);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isChasingHash, false);
            animator.SetBool(isAttackingHash, false);
        }
        #endregion
    }
}
