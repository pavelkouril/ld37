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
        public bool IsFree { get { return BuiltObject == null; } }

        public IBuildable BuiltObject { get; private set; }

        public TileManager TileManager { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Tile NeighbourLeft { get; set; }
        public Tile NeighbourRight { get; set; }
        public Tile NeighbourUp { get; set; }
        public Tile NeighbourDown { get; set; }


        internal void Build(IBuildable obj)
        {
            BuiltObject = obj;
            obj.Tile = this;
        }

        private void Start()
        {
            GetComponentInChildren<Text>().text = string.Format("[{0}, {1}]", PosX, PosY);
        }
    }
}