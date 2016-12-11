using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace OneRoomFactory.Machines
{
    public class Station : Buildable
    {
        public List<MovableType> AcceptedMovableTypes = new List<MovableType>();

        public Transform OutputSpawnPoint;

        public int MaxStorage = 10;

        public Movable OutputPrefab;
        public int ProductionRate = 5;

        private Dictionary<MovableType, int> storedMovables = new Dictionary<MovableType, int>();

        private IEnumerator coroutine;
        private bool coroutineRunning = false;

        private void Start()
        {
            coroutine = DoProduction();
            foreach (var type in AcceptedMovableTypes)
            {
                storedMovables[type] = 0;
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag("Movable"))
            {
                var movable = collider.GetComponent<Movable>();
                if (AcceptedMovableTypes.Contains(movable.Type) && CanStoreResources(movable.Type, movable.Units))
                {
                    storedMovables[movable.Type] = movable.Units;
                    Destroy(movable.gameObject);
                    if (HasEnoughResources() && !coroutineRunning)
                    {
                        coroutine = DoProduction();
                        coroutineRunning = true;
                        StartCoroutine(coroutine);
                    }
                }
            }
        }

        private IEnumerator DoProduction()
        {
            yield return new WaitForSeconds(ProductionRate);
            while (true)
            {
                if (HasEnoughResources())
                {
                    foreach (var type in AcceptedMovableTypes)
                    {
                        storedMovables[type]--;
                    }
                    Instantiate(OutputPrefab, OutputSpawnPoint.position, Quaternion.identity);
                    yield return new WaitForSeconds(ProductionRate);
                }
                else
                {
                    coroutineRunning = false;
                    yield break;
                }
            }
        }

        private bool HasEnoughResources()
        {
            return storedMovables.Count(p => p.Value == 0) == 0;
        }

        private bool CanStoreResources(MovableType type, int units)
        {
            return storedMovables[type] + units <= MaxStorage;
        }
    }
}
