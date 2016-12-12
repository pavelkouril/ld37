using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OneRoomFactory.Transporters;

namespace OneRoomFactory.Managers
{
    public class TileManager : MonoBehaviour
    {
        public int Size = 10;
        public Tile Prefab;
        public Transform TilesParent;
        public LayerMask TileLayer;

        public Tile[,] Tiles { get; private set; }
        public UIManager UIManager { get; private set; }
        public bool ChoosingTile { get { return selectingForHand != null; } }

        private ConstructionManager constructionManager;

        private Hand selectingForHand;

        private void Awake()
        {
            UIManager = GetComponent<UIManager>();
            constructionManager = GetComponent<ConstructionManager>();
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

         private void Update()
        {
            if (selectingForHand != null && Input.GetMouseButton(1))
            {
                constructionManager.HideBuildingMode();
                selectingForHand = null;
            }
            if (selectingForHand != null)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, TileLayer))
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        var tile = hit.collider.gameObject.GetComponent<Tile>();
                        
                        if ((tile.IsFree || tile.BuiltObject.GetComponent<Belt>() != null) && Vector3.Distance(tile.transform.position, selectingForHand.transform.position) <= 2)
                        {
                            Debug.Log("over free tile");
                            if (Input.GetMouseButton(0))
                            {
                                selectingForHand.Output = tile;
                                selectingForHand = null;
                                constructionManager.HideBuildingMode();
                            }
                        }
                    }
                }
            }
        }

        public void ShowTiles()
        {
            TilesParent.gameObject.SetActive(true);
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    Tiles[i, j].ResetMaterial();
                }
            }
        }

        public void HideTiles()
        {
            TilesParent.gameObject.SetActive(false);
        }

        public void SelectTargetTileForHand(Hand hand)
        {
            selectingForHand = hand;
        }
    }
}
