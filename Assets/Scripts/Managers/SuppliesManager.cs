using OneRoomFactory.Factory;
using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(TileManager))]
    public class SuppliesManager : MonoBehaviour
    {
        public Belt BeltPrefab;

        public Movable SupplyPrefab;

        private TileManager tileManager;

        private Belt cuprexitBelt;
        private Belt acidBelt;
        private Belt electronicsBelt;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
        }

        private void Start()
        {
            var tile = tileManager.Tiles[9, 3];
            cuprexitBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            cuprexitBelt.Rotation = BuildRotation.Right;
            tile.Build(cuprexitBelt);

            tile = tileManager.Tiles[9, 4];
            acidBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            acidBelt.Rotation = BuildRotation.Right;
            tile.Build(acidBelt);

            tile = tileManager.Tiles[8, 4];
            var belt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            belt.Rotation = BuildRotation.Right;
            tile.Build(belt);

            tile = tileManager.Tiles[7, 4];
            belt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            belt.Rotation = BuildRotation.Right;
            tile.Build(belt);

            tile = tileManager.Tiles[9, 5];
            electronicsBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            electronicsBelt.Rotation = BuildRotation.Right;
            tile.Build(electronicsBelt);

            StartCoroutine(ShipSupplies());
        }

        private IEnumerator ShipSupplies()
        {
            while (true)
            {
                var supply = Instantiate(SupplyPrefab, acidBelt.transform.position + Vector3.up, Quaternion.identity) as Movable;
                yield return new WaitForSeconds(10);
            }
        }
    }
}