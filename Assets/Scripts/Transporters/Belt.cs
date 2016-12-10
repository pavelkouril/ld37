﻿using System;
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
                    Output = Tile.TileManager.Tiles[Tile.PosX, Tile.PosY + 1];
                    MoveVector = new Vector3(0, 0, 1);
                    break;
                case BuildRotation.Right:
                    Output = Tile.TileManager.Tiles[Tile.PosX - 1, Tile.PosY];
                    MoveVector = new Vector3(-1, 0, 0);
                    break;
                case BuildRotation.Down:
                    Output = Tile.TileManager.Tiles[Tile.PosX, Tile.PosY - 1];
                    MoveVector = new Vector3(0, 0, -1);
                    break;
                case BuildRotation.Left:
                    Output = Tile.TileManager.Tiles[Tile.PosX - 1, Tile.PosY];
                    MoveVector = new Vector3(1, 0, 0);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (ToMove != null && ToMove.LastCollider == collider)
            {
                ToMove.transform.position += MoveVector * Time.fixedDeltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Movable"))
            {
                ToMove = collision.gameObject.GetComponent<Movable>();
                if (ToMove.LastCollider != collider)
                {
                    ToMove.LastCollider = collider;
                    //                    ToMove.transform.position = InputCenter.position;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("Movable"))
            {
                ToMove = null;
            }
        }

        public void Transport()
        {

        }
    }
}