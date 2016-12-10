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

        public Material Mat1;
        public Material Mat2;

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
                    Tiles[i, j].GetComponent<MeshRenderer>().material = (i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1) ? Mat1 : Mat2;
                }
            }
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
