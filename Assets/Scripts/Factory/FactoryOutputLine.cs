using OneRoomFactory.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class FactoryOutputLine : MonoBehaviour
    {
        public MoneyManager MoneyManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Movable"))
            {
                if (other.GetComponent<Movable>().Type == MovableType.CompletedPCB)
                {
                    MoneyManager.Recieve(250);
                }

                Destroy(other.gameObject);
            }
        }
    }
}