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

        public Movable CuprexitPrefab;
        public Movable AcidPrefab;

        public int CuprexitShipmentSize;
        public int CuprexitWaitTime;

        public int AcidShipmentSize;
        public int AcidWaitTime;

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

            tile = tileManager.Tiles[9, 5];
            electronicsBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            electronicsBelt.Rotation = BuildRotation.Right;
            tile.Build(electronicsBelt);

            StartCoroutine(ShipCuprexit());
            StartCoroutine(ShipAcid());
        }

        private IEnumerator ShipCuprexit()
        {
            yield return new WaitForSeconds(5);
            while (true)
            {
                for (var i = 0; i < CuprexitShipmentSize; i++)
                {
                    var supply = Instantiate(CuprexitPrefab, cuprexitBelt.InputCenter.position + new Vector3(0, 0, -0.25f), Quaternion.identity) as Movable;
                    yield return new WaitForSeconds(2);
                }
                yield return new WaitForSeconds(CuprexitWaitTime);
            }
        }

        private IEnumerator ShipAcid()
        {
            yield return new WaitForSeconds(5);
            while (true)
            {
                for (var i = 0; i < AcidShipmentSize; i++)
                {
                    var supply = Instantiate(AcidPrefab, acidBelt.InputCenter.position, Quaternion.identity) as Movable;
                    yield return new WaitForSeconds(2);
                }
                yield return new WaitForSeconds(AcidWaitTime);
            }
        }


    }
}