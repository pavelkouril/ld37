using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Transporters;
using UnityEngine;
using OneRoomFactory.Managers;
using UnityEngine.UI;

namespace OneRoomFactory.Factory
{
    public class Tile : MonoBehaviour
    {
        public bool IsFree = true;

        public IBuildable BuiltObject { get; private set; }

        public TileManager TileManager { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Tile NeighbourLeft { get; set; }
        public Tile NeighbourRight { get; set; }
        public Tile NeighbourUp { get; set; }
        public Tile NeighbourDown { get; set; }
        
        public void Build(IBuildable obj)
        {
            BuiltObject = obj;
            obj.Tile = this;
            IsFree = false;
            Debug.Log("built at " + PosX + "," + PosY);
        }

        public void Clean()
        {
            if (BuiltObject != null)
            {
                BuiltObject = null;
                IsFree = true;
            }
        }
    }
}