using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Transporters;
using UnityEngine;
using OneRoomFactory.Managers;

namespace OneRoomFactory.Factory
{
    public class Tile : MonoBehaviour
    {
        public bool IsFree { get { return BuiltObject == null; } }

        public IBuildable BuiltObject { get; private set; }

        public TileManager TileManager { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        internal void Build(IBuildable obj)
        {
            BuiltObject = obj;
            obj.Tile = this;
        }
    }
}