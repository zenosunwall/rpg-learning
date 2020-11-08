using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        bool isAlive = true;

        public void TakeDamage(float damage)
        { 
            health = Mathf.Max(health - damage, 0);
            Debug.Log(health);
            if (isAlive && health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("Die");
            isAlive = false;

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool IsDead()
        {
            return !isAlive;
        }
    }
}
