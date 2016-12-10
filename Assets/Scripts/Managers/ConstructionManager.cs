using OneRoomFactory.Factory;
using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(TileManager))]
    public class ConstructionManager : MonoBehaviour
    {
        public LayerMask TileLayer;

        public GameObject BeltPrefab;
        public GameObject UVStationPrefab;

        private TileManager tileManager;

        private GameObject modelToPlace;
        private GameObject prefabToPlace;
        private GameObject beltModel;
        private BuildRotation currentBuildRotation = BuildRotation.Right;
        private Vector3 currentVectorRotation = Vector3.zero;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
        }

        private void Start()
        {
            var beltModelGo = BeltPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            beltModel = Instantiate(beltModelGo);
            beltModel.SetActive(false);
        }

        private void Update()
        {
            if (modelToPlace == null && Input.GetKeyDown(KeyCode.B))
            {
                currentBuildRotation = BuildRotation.Right;
                currentVectorRotation = Vector3.zero;
                modelToPlace = beltModel;
                prefabToPlace = BeltPrefab;
                modelToPlace.transform.rotation = Quaternion.identity;
                modelToPlace.SetActive(true);
                tileManager.ShowTiles();
            }

            if (modelToPlace == null && Input.GetKeyDown(KeyCode.V))
            {
                currentBuildRotation = BuildRotation.Right;
                currentVectorRotation = Vector3.zero;
                modelToPlace = beltModel;
                prefabToPlace = BeltPrefab;
                modelToPlace.transform.rotation = Quaternion.identity;
                modelToPlace.SetActive(true);
                tileManager.ShowTiles();
            }

            if (modelToPlace != null && Input.GetMouseButton(1))
            {
                HideBuildingMode();
            }

            if (modelToPlace != null)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentBuildRotation++;
                    currentVectorRotation += new Vector3(0, -90, 0);
                    modelToPlace.transform.Rotate(new Vector3(0, -90, 0));
                }

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, TileLayer))
                {
                    modelToPlace.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                    if (hit.collider.CompareTag("Tile"))
                    {
                        var tile = hit.collider.gameObject.GetComponent<Tile>();
                        if (tile.IsFree)
                        {
                            modelToPlace.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 0.5f);
                            if (Input.GetMouseButton(0))
                            {
                                Build(tile);
                            }
                        }
                        else
                        {
                            modelToPlace.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.5f);
                        }
                    }
                }
            }
        }

        private void Build(Tile tile)
        {
            var obj = Instantiate(prefabToPlace, tile.transform.position, Quaternion.identity) as GameObject;
            obj.transform.Rotate(currentVectorRotation);
            var buildable = obj.GetComponent<IBuildable>();
            buildable.Rotation = currentBuildRotation;
            tile.Build(buildable);
            HideBuildingMode();
        }

        private void HideBuildingMode()
        {
            tileManager.HideTiles();
            modelToPlace.SetActive(false);
            modelToPlace = null;
            prefabToPlace = null;
        }
    }
}
