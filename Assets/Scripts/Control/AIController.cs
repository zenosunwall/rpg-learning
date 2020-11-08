using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 0.5f;
        [SerializeField] float stallAtWaypointTime = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;
        ActionScheduler actionScheduler;

        Vector3 guardLocation;
        int waypointIndex = 0;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSpentAtWaypoint = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            guardLocation = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (IsInAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehavior()
        { 
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    timeSpentAtWaypoint = 0;
                }
                guardLocation = GetCurrentWaypoint();
            }

            if (timeSpentAtWaypoint > stallAtWaypointTime)
            {
                
                mover.StartMovement(guardLocation, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            var distance = Vector3.Distance(GetCurrentWaypoint(), transform.position);
            return distance < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            waypointIndex = patrolPath.GetNextIndex(waypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        private void SuspicionBehaviour()
        {
            // Suspicion state
            actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool IsInAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
