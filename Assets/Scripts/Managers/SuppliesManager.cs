using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(TileManager))]
    public class SuppliesManager : MonoBehaviour
    {
        public Belt BeltPrefab;

        private TileManager tileManager;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
        }

        private void Start()
        {
            var tile = tileManager.Tiles[9, 3];
            Belt cuprexitBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            tile.Build(cuprexitBelt);

            tile = tileManager.Tiles[9, 4];
            Belt acidBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            tile.Build(acidBelt);

            tile = tileManager.Tiles[9, 5];
            Belt electronicsBelt = Instantiate(BeltPrefab, tile.transform.position, Quaternion.identity) as Belt;
            tile.Build(electronicsBelt);
        }
    }
}