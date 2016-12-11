using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OneRoomFactory.Managers
{
    public class TileManager : MonoBehaviour
    {
        public int Size = 10;
        public Tile Prefab;
        public Transform TilesParent;

        public Tile[,] Tiles { get; private set; }

        private void Awake()
        {
            Tiles = new Tile[Size, Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    var pos = new Vector3((-Size / 2.0f) + i + 0.5f, 0.01f, (-Size / 2.0f) + j + 0.5f);
                    Tiles[i, j] = Instantiate(Prefab, pos, Quaternion.Euler(90, 0, 0), TilesParent) as Tile;
                    Tiles[i, j].TileManager = this;
                    Tiles[i, j].PosX = i;
                    Tiles[i, j].PosY = j;
                }
            }

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    var currentTile = Tiles[i, j];
                    currentTile.NeighbourLeft = i > 0 ? Tiles[currentTile.PosX - 1, currentTile.PosY] : null;
                    currentTile.NeighbourRight = i < Size - 1 ? Tiles[currentTile.PosX + 1, currentTile.PosY] : null;
                    currentTile.NeighbourDown = j > 0 ? Tiles[currentTile.PosX, currentTile.PosY - 1] : null;
                    currentTile.NeighbourUp = j < Size - 1 ? Tiles[currentTile.PosX, currentTile.PosY + 1] : null;
                }
            }
        }

        private void Start()
        {
            Tiles[6, 6].IsFree = false;
            Tiles[6, 18].IsFree = false;
            Tiles[18, 6].IsFree = false;
            Tiles[18, 18].IsFree = false;
        }

        public void ShowTiles()
        {
            TilesParent.gameObject.SetActive(true);
        }

        public void HideTiles()
        {
            TilesParent.gameObject.SetActive(false);
        }
    }
}
