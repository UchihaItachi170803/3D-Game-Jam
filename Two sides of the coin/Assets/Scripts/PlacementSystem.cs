using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject preFab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDataBaseSO database;
    [SerializeField] private GameObject[] gridVisual;

    private int SelectedObjectIndex = -1;

    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();


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

        GameObject newObject = Instantiate(selectedData.preFab);
        newObject.transform.position = worldPos;

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
                Vector3Int cellToCheck = gridPos;
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
                Vector3Int cellToOccupy = gridPos;
                occupiedPositions.Add(cellToOccupy);
            }
        }
    }

   
}