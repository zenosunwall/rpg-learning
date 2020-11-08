using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 6;

        NavMeshAgent meshAgent;
        Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            meshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            meshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void StartMovement(Vector3 desitination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(desitination, speedFraction);
        }

        public void MoveTo(Vector3 desitination, float speedFraction)
        {
            meshAgent.isStopped = false;
            meshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            meshAgent.destination = desitination;
        }

        public void Stop()
        {
            meshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = meshAgent.velocity;
            Vector3 localeVelocity = transform.InverseTransformDirection(velocity);
            float speed = localeVelocity.z;
            GetComponent<Animator>().SetFloat("FowardSpeed", speed);
        }

        public void Cancel()
        {

        }
    }
}
