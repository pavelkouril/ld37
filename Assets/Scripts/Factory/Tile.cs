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

        public Buildable BuiltObject { get; private set; }

        public TileManager TileManager { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Tile NeighbourLeft { get; set; }
        public Tile NeighbourRight { get; set; }
        public Tile NeighbourUp { get; set; }
        public Tile NeighbourDown { get; set; }

        public Material CurrentMaterial { get { return renderer.material; } set { renderer.material = value; } }

        public Material DefaultMaterial;
        public Material HoverMaterial;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
            CurrentMaterial = DefaultMaterial;
        }

        public void Build(Buildable obj)
        {
            BuiltObject = obj;
            obj.Tile = this;
            IsFree = false;
        }

        public void Clear()
        {
            if (BuiltObject != null)
            {
                Destroy(BuiltObject.gameObject);
                BuiltObject = null;
                IsFree = true;
            }
        }

        private void OnMouseEnter()
        {
            CurrentMaterial = HoverMaterial;
        }

        private void OnMouseExit()
        {
            CurrentMaterial = DefaultMaterial;
        }
    }
}