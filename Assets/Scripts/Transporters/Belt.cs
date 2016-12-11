using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Factory;
using UnityEngine;

namespace OneRoomFactory.Transporters
{
    public class Belt : MonoBehaviour, ITransporter
    {
        public Transform InputCenter;

        public Tile Input { get; private set; }

        public Tile Output { get; private set; }

        public Tile Tile { get; set; }

        public BuildRotation Rotation { get; set; }

        public Movable ToMove { get; set; }

        public TransporterType Type { get { return TransporterType.Belt; } }

        public Vector3 MoveVector { get { return moveVectors[Rotation]; } }

        private Dictionary<BuildRotation, Vector3> moveVectors = new Dictionary<BuildRotation, Vector3>()
        {
            { BuildRotation.Up, new Vector3(0, 0, 1) },
            { BuildRotation.Right, new Vector3(-1, 0, 0) },
            { BuildRotation.Down, new Vector3(0, 0, -1) },
            { BuildRotation.Left, new Vector3(1, 0, 0) }
        };

        private void Start()
        {
            if (Tile)
            {
                switch (Rotation)
                {
                    case BuildRotation.Up:
                        Output = Tile.NeighbourDown;
                        break;
                    case BuildRotation.Right:
                        Output = Tile.NeighbourLeft;
                        break;
                    case BuildRotation.Down:
                        Output = Tile.NeighbourUp;
                        break;
                    case BuildRotation.Left:
                        Output = Tile.NeighbourRight;
                        break;
                }
            }
        }

        public Transform GetTransform()
        {
            if (Tile)
            {
                return Tile.transform;
            }
            else
            {
                return transform;
            }
        }
    }
}