using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Transporters;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class Tile : MonoBehaviour
    {
        public bool IsOccupied { get { return BuiltObject != null; } }

        public IBuildable BuiltObject { get; private set; }

        internal void Build(IBuildable obj)
        {
            BuiltObject = obj;
            obj.Tile = this;
        }
    }
}