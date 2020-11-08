using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointSize = 1f;

        private void OnDrawGizmos()
        {
            for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(GetWaypoint(childIndex), waypointSize);
                Gizmos.DrawLine(GetWaypoint(childIndex), GetWaypoint(GetNextIndex(childIndex)));
            }
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }

        public int GetNextIndex(int index)
        {
            return (index + 1) % transform.childCount; 
        }
    }
}
