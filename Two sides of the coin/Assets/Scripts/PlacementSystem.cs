using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject preFab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDataBaseSO database;
    [SerializeField] private GameObject[] gridVisual;
    [SerializeField] private ResourceManager resourceManager;


    private int SelectedObjectIndex = -1;

    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();
    private Dictionary<int, System.Action> spawnActions;

    private void Awake()
    {
        spawnActions = new Dictionary<int, System.Action>
        {
            {1,resourceManager.Tree },
            {2,resourceManager.RainWaterHarvestingSystem },
            {3,resourceManager.GroundWaterSystem }
        
        };
    }


    private void Start()
    { 
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        SelectedObjectIndex = database.objectsData.FindIndex(Data => Data.ID == ID);
        
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError("No ID Found " + ID);
            return;
        }
        foreach(var obj in gridVisual)
        {
           obj.SetActive(true);
        }
        
        preFab.SetActive(true);

        inputManager.OnCLicked += PlacedStructure;
        inputManager.OnExit += StopPlacement;
    }

    

    private void PlacedStructure()
    {
        if (inputManager.IsPointingUI()) return;
        Vector3 mousepos = inputManager.GetSelectMapPosition();
        Vector3Int GridPos = grid.WorldToCell(mousepos);
        ObjectData selectedData = database.objectsData[SelectedObjectIndex];

        if (!CanPlaceAt(GridPos, selectedData.Size))
        {
            Debug.Log("Cannot place here - already occupied!");
            return;
        }

        Vector3 worldPos = grid.GetCellCenterWorld(GridPos);
        worldPos.y = 0;
        if(selectedData.ID == 1 && resourceManager.economy >= 30)
        {
            ObjectSpawningAndSpaceOccupy(GridPos, selectedData, worldPos);
        }
        else if(selectedData.ID == 2 && resourceManager.economy >= 50)
        {
            ObjectSpawningAndSpaceOccupy(GridPos, selectedData, worldPos);
        }
        else if(selectedData.ID == 3 && resourceManager.economy >= 150)
        {
            ObjectSpawningAndSpaceOccupy(GridPos, selectedData, worldPos);
        }
        else if(selectedData.ID == 4 && resourceManager.economy >= 250)
        {
            ObjectSpawningAndSpaceOccupy(GridPos, selectedData, worldPos);
        }

    }

    private void ObjectSpawningAndSpaceOccupy(Vector3Int GridPos, ObjectData selectedData, Vector3 worldPos)
    {
        GameObject newObject = Instantiate(selectedData.preFab);
        newObject.transform.position = worldPos;
        if (spawnActions.TryGetValue(selectedData.ID, out var action))
        {
            action?.Invoke();
        }
        OccupyCells(GridPos, selectedData.Size);
    }

    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        foreach (var obj in gridVisual)
        {
            obj.SetActive(false);
        }
        preFab.SetActive(false);
        inputManager.OnCLicked -= PlacedStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (SelectedObjectIndex < 0) return;

        Vector3 mousepos = inputManager.GetSelectMapPosition();
        Vector3Int GridPos = grid.WorldToCell(mousepos);

        Vector3 centerWorldPos = grid.GetCellCenterWorld(GridPos);
        centerWorldPos.y = 0;
        preFab.transform.position = centerWorldPos;
    }

    private bool CanPlaceAt(Vector3Int gridPos, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cellToCheck = gridPos + new Vector3Int(x,0,y);
                if (occupiedPositions.Contains(cellToCheck))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void OccupyCells(Vector3Int gridPos, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cellToOccupy = gridPos + new Vector3Int(x,0,y);
                occupiedPositions.Add(cellToOccupy);
            }
        }
    }

   
}