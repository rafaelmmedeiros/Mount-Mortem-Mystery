using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            const float radius = 0.3f;

            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);

                DefineWaypointColor(i);
                Gizmos.DrawSphere(transform.GetChild(i).position, radius);

                DefineLineColor(i);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        private void DefineLineColor(int i)
        {
            if (i + 1 == transform.childCount)
            {
                Gizmos.color = Color.red;
            }
            else if (i == 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.white;
            }
        }

        private void DefineWaypointColor(int i)
        {
            if (i == 0)
            {
                Gizmos.color = Color.green;
            }
            else if (i + 1 == transform.childCount)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.white;
            }

        }

        private int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

