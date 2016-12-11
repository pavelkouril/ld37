using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public abstract class Buildable : MonoBehaviour
    {
        public int Price;
        public Tile Tile { get; set; }
        public BuildRotation Rotation { get; set; }
    }
}