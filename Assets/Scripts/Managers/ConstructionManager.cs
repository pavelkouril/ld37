using OneRoomFactory.Factory;
using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(TileManager))]
    public class ConstructionManager : MonoBehaviour
    {
        public LayerMask TileLayer;

        public Buildable BeltPrefab;
        public Buildable RobotHandPrefab;
        public Buildable UVStationPrefab;

        private TileManager tileManager;
        private UIManager uiManager;
        private MoneyManager moneyManager;

        private GameObject modelToPlace;
        private Buildable prefabToPlace;

        private GameObject beltModel;
        private GameObject uvStationModel;
        private GameObject robotHandModel;

        private BuildRotation currentBuildRotation = BuildRotation.Right;
        private Vector3 currentVectorRotation = Vector3.zero;
        private int currentPrice = 0;

        private bool canRotate = true;

        private Quaternion robotHandModelRot;

        private bool isDestroying = false;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
            uiManager = GetComponent<UIManager>();
            moneyManager = GetComponent<MoneyManager>();
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

        public void BuildBeltButtonClicked()
        {
            ResetConstructionParameters();
            modelToPlace = beltModel;
            prefabToPlace = BeltPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.SetActive(true);
        }

        public void BuildHandButtonClicked()
        {
            ResetConstructionParameters();
            modelToPlace = robotHandModel;
            prefabToPlace = RobotHandPrefab;
            modelToPlace.transform.rotation = robotHandModelRot;
            modelToPlace.SetActive(true);
            canRotate = false;
        }

        public void BuildUVStationButtonClicked()
        {
            ResetConstructionParameters();
            modelToPlace = uvStationModel;
            prefabToPlace = UVStationPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.SetActive(true);
        }

        public void BuildAcidSinkButtonClicked()
        {

        }

        public void BuildCleanerButtonClicked()
        {

        }

        public void BuildDrillerButtonClicked()
        {

        }

        public void BuildAssemblerButtonClicked()
        {

        }

        private void ResetConstructionParameters()
        {
            uiManager.HideBuildMenu();
            tileManager.ShowTiles();
            currentBuildRotation = BuildRotation.Right;
            currentVectorRotation = Vector3.zero;
        }

        public void EnterDestroyMode()
        {
            uiManager.HideBuildMenu();
            tileManager.ShowTiles();
            isDestroying = true;
        }

        private void Update()
        {
            if (modelToPlace == null && Input.GetKeyDown(KeyCode.D))
            {
                EnterDestroyMode();
            }

            if ((modelToPlace != null || isDestroying) && Input.GetMouseButton(1))
            {
                HideBuildingMode();
            }

            if (modelToPlace != null)
            {
                if (Input.GetKeyDown(KeyCode.R) && canRotate)
                {
                    currentBuildRotation = (BuildRotation)(((int)currentBuildRotation + 1) % 4);
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
                        if (tile.IsFree && moneyManager.CanPay(prefabToPlace.Price))
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
            var buildable = Instantiate(prefabToPlace, tile.transform.position, Quaternion.identity);
            buildable.transform.Rotate(currentVectorRotation);
            buildable.Rotation = currentBuildRotation;
            tile.Build(buildable);
            moneyManager.Pay(prefabToPlace.Price);
        }

        private void Clear(Tile tile)
        {
            tile.Clear();
        }

        public void HideBuildingMode()
        {
            isDestroying = false;
            tileManager.HideTiles();
            if (modelToPlace != null)
            {
                modelToPlace.SetActive(false);
            }
            modelToPlace = null;
            prefabToPlace = null;
            canRotate = true;
        }
    }
}
