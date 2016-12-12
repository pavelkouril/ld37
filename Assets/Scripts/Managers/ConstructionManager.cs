using OneRoomFactory.Factory;
using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(TileManager))]
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(MoneyManager))]
    [RequireComponent(typeof(GamestateManager))]
    public class ConstructionManager : MonoBehaviour
    {
        public Transform BlueprintsParent;
        public LayerMask TileLayer;

        public Buildable BeltPrefab;
        public Buildable RobotHandPrefab;
        public Buildable UVStationPrefab;
        public Buildable AcidSinkStationPrefab;
        public Buildable CleaningStationPrefab;
        public Buildable DrillingStationPrefab;
        public Buildable AssemblyStationPrefab;

        private TileManager tileManager;
        private UIManager uiManager;
        private MoneyManager moneyManager;
        private GamestateManager gamestateManager;

        private Buildable modelToPlace;
        private Buildable prefabToPlace;

        private Buildable beltModel;
        private Buildable robotHandModel;
        private Buildable uvStationModel;
        private Buildable acidSinkStationModel;
        private Buildable cleaningStationModel;
        private Buildable drillingStationModel;
        private Buildable assemblyStationModel;

        private BuildRotation currentBuildRotation = BuildRotation.Right;
        private Vector3 currentVectorRotation = Vector3.zero;
        private int currentPrice = 0;

        private bool canRotate = true;

        private Quaternion robotHandModelRot;

        private bool isDestroying = false;

        private bool isGoingToBuildAcidStation = false;
        private bool isGoingToBuildAssemblyStation = false;
        private bool isGoingToBuildUVStation = false;
        private bool isGoingToBuildHand = false;

        private bool isFirstBuild = true;

        private void Awake()
        {
            tileManager = GetComponent<TileManager>();
            uiManager = GetComponent<UIManager>();
            moneyManager = GetComponent<MoneyManager>();
            gamestateManager = GetComponent<GamestateManager>();
        }

        private void Start()
        {
            var beltModelGo = BeltPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            beltModel = Instantiate(BeltPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            beltModel.gameObject.SetActive(false);
            beltModel.enabled = false;
            foreach (var c in beltModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            var robotHandModelGo = RobotHandPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            robotHandModel = Instantiate(RobotHandPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            robotHandModel.gameObject.SetActive(false);
            robotHandModel.enabled = false;
            foreach (var c in robotHandModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            var uvStationModelGo = UVStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            uvStationModel = Instantiate(UVStationPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            uvStationModel.gameObject.SetActive(false);
            uvStationModel.enabled = false;
            foreach (var c in uvStationModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            var acidSinkStationModelGo = AcidSinkStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            acidSinkStationModel = Instantiate(AcidSinkStationPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            acidSinkStationModel.gameObject.SetActive(false);
            acidSinkStationModel.enabled = false;
            foreach (var c in acidSinkStationModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            var cleaningStationModelGo = CleaningStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            cleaningStationModel = Instantiate(CleaningStationPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            cleaningStationModel.gameObject.SetActive(false);
            cleaningStationModel.enabled = false;
            foreach (var c in cleaningStationModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            var drillingStationModelGo = DrillingStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            drillingStationModel = Instantiate(DrillingStationPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            drillingStationModel.gameObject.SetActive(false);
            drillingStationModel.enabled = false;
            foreach (var c in drillingStationModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            var assemblyStationModelGo = AssemblyStationPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            assemblyStationModel = Instantiate(AssemblyStationPrefab, Vector3.zero, Quaternion.identity, BlueprintsParent);
            assemblyStationModel.gameObject.SetActive(false);
            assemblyStationModel.enabled = false;
            foreach (var c in assemblyStationModel.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }

        public void BuildBeltButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = beltModel;
            prefabToPlace = BeltPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
        }

        public void BuildHandButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = robotHandModel;
            prefabToPlace = RobotHandPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
            canRotate = false;
            isGoingToBuildHand = true;
        }

        public void BuildUVStationButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = uvStationModel;
            prefabToPlace = UVStationPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
            isGoingToBuildUVStation = true;
        }

        public void BuildAcidSinkButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = acidSinkStationModel;
            prefabToPlace = AcidSinkStationPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
            isGoingToBuildAcidStation = true;
        }

        public void BuildCleanerButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = cleaningStationModel;
            prefabToPlace = CleaningStationPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
        }

        public void BuildDrillerButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = drillingStationModel;
            prefabToPlace = DrillingStationPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
        }

        public void BuildAssemblerButtonClicked()
        {
            PrepareBuilding();
            modelToPlace = assemblyStationModel;
            prefabToPlace = AssemblyStationPrefab;
            modelToPlace.transform.rotation = Quaternion.identity;
            modelToPlace.gameObject.SetActive(true);
            isGoingToBuildAssemblyStation = true;
        }

        private void PrepareBuilding()
        {
            if (isFirstBuild)
            {
                isFirstBuild = false;
                uiManager.DisplayText("You need to place machines on the tiles. Rotation matters, so press <b>R</b> to <b>rotate the objects</b>. If you do a mistake, press <b>F</b> to <b>enter destroy mode</b>.", 6);
            }
            uiManager.HideBuildMenu();
            tileManager.ShowTiles();
            currentBuildRotation = BuildRotation.Right;
            currentVectorRotation = Vector3.zero;
            isGoingToBuildAcidStation = false;
            isGoingToBuildAssemblyStation = false;
            isGoingToBuildUVStation = false;
            isGoingToBuildHand = false;
        }

        public void EnterDestroyMode()
        {
            uiManager.HideBuildMenu();
            tileManager.ShowTiles();
            isDestroying = true;
        }

        private void Update()
        {
            if (modelToPlace == null && Input.GetKeyDown(KeyCode.F))
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
                            modelToPlace.GetComponentInChildren<MeshRenderer>().material.color = new Color(0, 1, 0, 0.1f);
                            if (Input.GetMouseButton(0))
                            {
                                Build(tile);
                            }
                        }
                        else
                        {
                            modelToPlace.GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 0, 0, 0.2f);
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
            if (isGoingToBuildUVStation)
            {
                gamestateManager.StartShippingCuprexit();
            }
            if (isGoingToBuildAcidStation)
            {
                gamestateManager.StartShippingAcid();
            }
            if (isGoingToBuildAssemblyStation)
            {
                gamestateManager.StartShippingElectronics();
            }
            if (isGoingToBuildHand)
            {
                gamestateManager.ShowHandTutorial();
            }
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
                modelToPlace.gameObject.SetActive(false);
            }
            modelToPlace = null;
            prefabToPlace = null;
            canRotate = true;
        }
    }
}
