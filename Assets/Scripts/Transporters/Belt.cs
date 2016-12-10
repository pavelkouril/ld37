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

        private Vector3 MoveVector = new Vector3(0, 0);

        private new Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        private void Start()
        {
            switch (Rotation)
            {
                case BuildRotation.Up:
                    Output = Tile.NeighbourDown;
                    MoveVector = new Vector3(0, 0, +1);
                    break;
                case BuildRotation.Right:
                    Output = Tile.NeighbourLeft;
                    MoveVector = new Vector3(-1, 0, 0);
                    break;
                case BuildRotation.Down:
                    Output = Tile.NeighbourUp;
                    MoveVector = new Vector3(0, 0, -1);
                    break;
                case BuildRotation.Left:
                    Output = Tile.NeighbourRight;
                    MoveVector = new Vector3(1, 0, 0);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (ToMove != null && ToMove.TransportedBy == this)
            {
                ToMove.transform.position += MoveVector * Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Movable"))
            {
                var movable = other.GetComponent<Movable>();
                ToMove = movable;
                if (ToMove.TransportedBy != this)
                {
                    ToMove.TransportedBy = this;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Movable"))
            {
                if (Output)
                {
                    var objectOnNextTile = Output.BuiltObject as Belt;
                    if (objectOnNextTile != null && objectOnNextTile.Rotation != Rotation)
                    {
                        ToMove.transform.position = objectOnNextTile.InputCenter.position;
                    }
                }
                ToMove = null;
            }
        }
    }
}