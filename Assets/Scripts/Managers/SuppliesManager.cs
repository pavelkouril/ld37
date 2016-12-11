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
        public Transform AcidSpawnPosition;
        public Transform CuprexitSpawnPosition;

        public Movable CuprexitPrefab;
        public Movable AcidPrefab;

        public int CuprexitShipmentSize;
        public int CuprexitWaitTime;
        public bool CuprexitAccepted;

        public int AcidShipmentSize;
        public int AcidWaitTime;
        public bool AcidAccepted;

        private void Start()
        {
            StartCoroutine(ShipCuprexit());
            StartCoroutine(ShipAcid());
        }

        private IEnumerator ShipCuprexit()
        {
            yield return new WaitForSeconds(5);
            while (true)
            {
                if (CuprexitAccepted)
                {
                    for (var i = 0; i < CuprexitShipmentSize; i++)
                    {
                        Instantiate(CuprexitPrefab, CuprexitSpawnPosition.position, Quaternion.identity);
                        yield return new WaitForSeconds(2);
                    }
                }
                yield return new WaitForSeconds(CuprexitWaitTime);
            }
        }

        private IEnumerator ShipAcid()
        {
            yield return new WaitForSeconds(5);
            while (true)
            {
                if (AcidAccepted)
                {
                    for (var i = 0; i < AcidShipmentSize; i++)
                    {
                        Instantiate(AcidPrefab, AcidSpawnPosition.position, Quaternion.identity);
                        yield return new WaitForSeconds(2);
                    }
                }
                yield return new WaitForSeconds(AcidWaitTime);
            }
        }
    }
}