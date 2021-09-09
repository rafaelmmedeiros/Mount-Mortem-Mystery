using RPG.Attributes;
using RPG.Control;
using RPG.Control.Enums;
using RPG.Control.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if(healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }

            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool toShow)
        {
            GetComponent<Collider>().enabled = toShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(toShow);
            }
        }

        //  INTERFACES
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(playerController.gameObject);
            }
            return true;
        }
    }
}