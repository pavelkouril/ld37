using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class TeleportBackToFactory : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.transform.position += Vector3.up * 2;
        }
    }
}