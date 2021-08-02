using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            bool hasHit = Physics.Raycast(ray, out Hit);

            if (hasHit)
            {
                GetComponent<Mover>().MoveTo(Hit.point);
            }
        }
    }
}