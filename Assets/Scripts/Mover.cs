using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    //[SerializeField] Transform target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }

        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        bool hasHit = Physics.Raycast(ray, out raycastHit);

        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = raycastHit.point;
        }
    }
}