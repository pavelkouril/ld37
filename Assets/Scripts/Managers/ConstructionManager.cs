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

        private GameObject beltModelGo;
        private GameObject uvStationModelGo;
        private GameObject robotHandModelGo;

        private BuildRotation currentBuildRotation = BuildRotation.Right;
        private Vector3 currentVectorRotation = Vector3.zero;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
        }

        private void Start()
        {
            beltModelGo = BeltPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            beltModel = Instantiate(beltModelGo, beltModelGo.transform.position, beltModelGo.transform.rotation);
            beltModel.SetActive(false);

            robotHandModelGo = RobotHandPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            robotHandModel = Instantiate(robotHandModelGo, robotHandModelGo.transform.position, robotHandModelGo.transform.rotation);
            robotHandModel.SetActive(false);

            uvStationModelGo = UVStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            uvStationModel = Instantiate(uvStationModelGo, uvStationModelGo.transform.position, uvStationModelGo.transform.rotation);
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
                modelToPlace.transform.rotation = beltModelGo.transform.rotation;
                modelToPlace.SetActive(true);
                tileManager.ShowTiles();
            }

            if (modelToPlace == null && Input.GetKeyDown(KeyCode.V))
            {
                currentBuildRotation = BuildRotation.Right;
                currentVectorRotation = Vector3.zero;
                modelToPlace = uvStationModel;
                prefabToPlace = UVStationPrefab;
                modelToPlace.transform.rotation = uvStationModelGo.transform.rotation;
                modelToPlace.SetActive(true);
                tileManager.ShowTiles();
            }

            if (modelToPlace == null && Input.GetKeyDown(KeyCode.N))
            {
                currentBuildRotation = BuildRotation.Right;
                currentVectorRotation = Vector3.zero;
                modelToPlace = robotHandModel;
                prefabToPlace = RobotHandPrefab;
                modelToPlace.transform.rotation = robotHandModelGo.transform.rotation;
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
