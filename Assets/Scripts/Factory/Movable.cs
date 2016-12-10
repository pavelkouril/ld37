using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class Movable : MonoBehaviour
    {
        public Collider LastCollider { get; set; }
        public MovableType Type;
        public int Units = 1;
        public ITransporter TransportedBy;
    }
}