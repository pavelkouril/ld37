using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class TeleportBackToFactory : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Movable")
            {
                Movable m = other.gameObject.GetComponent<Movable>();
                if (m.isGrabbed)
                {
                    return;
                }
            }

            other.transform.position += Vector3.up * 2;
        }
    }
}