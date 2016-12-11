using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class SupplyBeltInitializer : MonoBehaviour
    {
        private void Awake()
        {
            foreach (var b in GetComponentsInChildren<Belt>())
            {
                b.Rotation = BuildRotation.Left;
            }
        }
    }
}