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
        public GameObject RobotHandPrefab;
        public GameObject UVStationPrefab;

        private TileManager tileManager;

        private GameObject modelToPlace;
        private GameObject prefabToPlace;

        private GameObject beltModel;
        private GameObject uvStationModel;
        private GameObject robotHandModel;
        
        private BuildRotation currentBuildRotation = BuildRotation.Right;
        private Vector3 currentVectorRotation = Vector3.zero;

        private bool canRotate = true;

        private Quaternion robotHandModelRot;

        private bool isDestroying = false;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
        }

        private void Start()
        {
            var beltModelGo = BeltPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            beltModel = Instantiate(beltModelGo, beltModelGo.transform.position, Quaternion.identity);
            beltModel.SetActive(false);

            var robotHandModelGo = RobotHandPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            robotHandModelRot = robotHandModelGo.transform.rotation;
            robotHandModel = Instantiate(robotHandModelGo, robotHandModelGo.transform.position, robotHandModelRot);
            robotHandModel.SetActive(false);

            var uvStationModelGo = UVStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            uvStationModel = Instantiate(uvStationModelGo, uvStationModelGo.transform.position, Quaternion.identity);
            uvStationModel.SetActive(false);
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
                modelToPlace = uvStationModel;
                prefabToPlace = UVStationPrefab;
                modelToPlace.transform.rotation = Quaternion.identity;
                modelToPlace.SetActive(true);
                tileManager.ShowTiles();
            }

            if (modelToPlace == null && Input.GetKeyDown(KeyCode.N))
            {
                currentBuildRotation = BuildRotation.Right;
                currentVectorRotation = Vector3.zero;
                modelToPlace = robotHandModel;
                prefabToPlace = RobotHandPrefab;
                modelToPlace.transform.rotation = robotHandModelRot;
                modelToPlace.SetActive(true);
                tileManager.ShowTiles();
                canRotate = false;
            }

            if (modelToPlace == null && Input.GetKeyDown(KeyCode.D))
            {
                tileManager.ShowTiles();
                isDestroying = true;
            }

            if ((modelToPlace != null || isDestroying) && Input.GetMouseButton(1))
            {
                HideBuildingMode();
                isDestroying = false;
            }

            if (modelToPlace != null)
            {
                if (Input.GetKeyDown(KeyCode.R) && canRotate)
                {
                    currentBuildRotation++;
                    currentVectorRotation += new Vector3(0, -90, 0);
                    modelToPlace.transform.Rotate(new Vector3(0, -90, 0));
                }

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, TileLayer))
                {
                    modelToPlace.transform.position = new Vector3(hit.point.x, modelToPlace.transform.position.y, hit.point.z);
                    if (hit.collider.CompareTag("Tile"))
                    {
                        var tile = hit.collider.gameObject.GetComponent<Tile>();
                        if (tile.IsFree)
                        {
                            modelToPlace.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 0.2f);
                            if (Input.GetMouseButton(0))
                            {
                                Build(tile);
                            }
                        }
                        else
                        {
                            modelToPlace.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.2f);
                        }
                    }
                }
            }

            if (isDestroying)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, TileLayer))
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        var tile = hit.collider.gameObject.GetComponent<Tile>();
                        if (tile.BuiltObject != null)
                        {
                            if (Input.GetMouseButton(0))
                            {
                                Clear(tile);
                            }
                        }
                    }
                }
            }
        }

        private void Build(Tile tile)
        {
            var obj = Instantiate(prefabToPlace, tile.transform.position, Quaternion.identity) as GameObject;
            obj.transform.Rotate(currentVectorRotation);
            var buildable = obj.GetComponent<Buildable>();
            buildable.Rotation = currentBuildRotation;
            tile.Build(buildable);
            HideBuildingMode();
        }

        private void Clear(Tile tile)
        {
            tile.Clear();
        }

        private void HideBuildingMode()
        {
            tileManager.HideTiles();
            modelToPlace.SetActive(false);
            modelToPlace = null;
            prefabToPlace = null;
            canRotate = true;
        }
    }
}
