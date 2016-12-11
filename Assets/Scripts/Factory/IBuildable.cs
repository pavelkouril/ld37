using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public abstract class Buildable : MonoBehaviour
    {
        public Tile Tile { get; set; }
        public BuildRotation Rotation { get; set; }
    }
}