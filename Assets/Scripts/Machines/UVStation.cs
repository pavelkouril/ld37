using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OneRoomFactory.Machines
{
    public class UVStation : MonoBehaviour, IBuildable
    {
        public BuildRotation Rotation { get; set; }    
        public Tile Tile { get; set; }

        private void OnCollisionEnter(Collision collision)
        {
            
        }

    }
}

