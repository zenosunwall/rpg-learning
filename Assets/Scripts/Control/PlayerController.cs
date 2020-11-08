using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Fighter fighter;
        Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) { return; }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            Debug.Log("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] raycastHits;
            raycastHits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in raycastHits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) { continue; }

                GameObject targetGameObject = target.gameObject;
                if (!GetComponent<Fighter>().CanAttack(targetGameObject)) { continue; }

                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(targetGameObject); 
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMovement(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
