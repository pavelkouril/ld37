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
        public Transform CuprexitSpawnPosition1;
        public Transform CuprexitSpawnPosition2;
        public Transform ElectronicsSpawnPosition;

        public Movable CuprexitPrefab;
        public Movable AcidPrefab;
        public Movable ElectronicsPrefab;

        public int CuprexitShipmentSize;
        public int CuprexitWaitTime;
        public bool CuprexitAccepted;

        public int AcidShipmentSize;
        public int AcidWaitTime;
        public bool AcidAccepted;

        public int ElectronicsShipmentSize;
        public int ElectronicsWaitTime;
        public bool ElectronicsAccepted;

        public IEnumerator ShipCuprexit()
        {
            yield return new WaitForSeconds(10);
            while (true)
            {
                if (CuprexitAccepted)
                {
                    for (var i = 0; i < CuprexitShipmentSize; i = i + 2)
                    {
                        Instantiate(CuprexitPrefab, CuprexitSpawnPosition1.position, Quaternion.identity);
                        Instantiate(CuprexitPrefab, CuprexitSpawnPosition2.position, Quaternion.identity);
                        yield return new WaitForSeconds(2);
                    }
                }
                yield return new WaitForSeconds(CuprexitWaitTime);
            }
        }

        public IEnumerator ShipAcid()
        {
            yield return new WaitForSeconds(10);
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

        public IEnumerator ShipElectronics()
        {
            yield return new WaitForSeconds(10);
            while (true)
            {
                if (ElectronicsAccepted)
                {
                    for (var i = 0; i < ElectronicsShipmentSize; i++)
                    {
                        Instantiate(ElectronicsPrefab, ElectronicsSpawnPosition.position, Quaternion.identity);
                        yield return new WaitForSeconds(2);
                    }
                }
                yield return new WaitForSeconds(ElectronicsWaitTime);
            }
        }
    }
}