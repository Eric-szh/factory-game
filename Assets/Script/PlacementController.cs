// PlacementController.cs
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementController : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;

    private BuildingManager buildingManager;
    private GameObject previewInstance;
    private int rotation = 0;

    void Start()
    {
        buildingManager = BuildingManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotation = (rotation - 90) % 360;
            if (previewInstance != null)
                previewInstance.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        HandleInput();
        UpdatePreview();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    void PlaceBuilding()
    {
        GameObject selectedBuilding = buildingManager.GetSelectedBuilding();
        if (selectedBuilding == null)
            return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        if (!tilemap.HasTile(cellPosition))
        {
            Debug.Log("Cannot place building outside the grid.");
            return;
        }

        if (BuildingRegistry.Instance.GetBuildingAtPosition(cellPosition) != null)
        {
            Debug.Log("A building already exists here!");
            return;
        }

        BuildingData data = ResourceManager.Instance.GetBuildingData(selectedBuilding);

        if (data != null)
        {
            if (ResourceManager.Instance.totalResources < data.cost)
            {
                Debug.Log("Not enough resources to build " + data.buildingName);
                return;
            }

            if (data.currentCount >= data.maxAllowance)
            {
                Debug.Log("Maximum allowance reached for " + data.buildingName);
                return;
            }

            ResourceManager.Instance.SpendResources(data.cost);
            data.currentCount++;
        }

        Vector3 spawnPosition = grid.GetCellCenterWorld(cellPosition);
        GameObject buildingObj = Instantiate(selectedBuilding, spawnPosition, Quaternion.Euler(0, 0, rotation));

        BuildingInstance instance = buildingObj.AddComponent<BuildingInstance>();
        instance.buildingData = data;

        BuildingBase buildingBase = buildingObj.GetComponent<BuildingBase>();
        if (buildingBase != null)
        {
            buildingBase.GridPosition = cellPosition;
            BuildingRegistry.Instance.AddBuilding(cellPosition, buildingBase);

            buildingBase.OnPlaced();

            NotifyNeighbors(cellPosition);
        }
    }

    void UpdatePreview()
    {
        GameObject selectedBuilding = buildingManager.GetSelectedBuilding();

        if (selectedBuilding == null)
        {
            if (previewInstance != null)
            {
                Destroy(previewInstance);
                previewInstance = null;
            }
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        if (!tilemap.HasTile(cellPosition))
        {
            if (previewInstance != null)
            {
                Destroy(previewInstance);
                previewInstance = null;
            }
            return;
        }

        Vector3 spawnPosition = grid.GetCellCenterWorld(cellPosition);

        if (previewInstance == null)
        {
            previewInstance = Instantiate(selectedBuilding, spawnPosition, Quaternion.identity);
            SetSpriteTransparency(previewInstance, 0.5f);
        }
        else
        {
            previewInstance.transform.position = spawnPosition;
        }

        previewInstance.transform.rotation = Quaternion.Euler(0, 0, rotation);

        bool canPlace = CanPlaceBuildingAtPosition(cellPosition, selectedBuilding);

        SetPreviewColor(canPlace ? Color.white : Color.red);
    }

    bool CanPlaceBuildingAtPosition(Vector3Int cellPosition, GameObject selectedBuilding)
    {
        if (BuildingRegistry.Instance.GetBuildingAtPosition(cellPosition) != null)
        {
            return false;
        }

        BuildingData data = ResourceManager.Instance.GetBuildingData(selectedBuilding);

        if (data != null)
        {
            if (ResourceManager.Instance.totalResources < data.cost)
            {
                return false;
            }

            if (data.currentCount >= data.maxAllowance)
            {
                return false;
            }
        }

        return true;
    }

    void SetPreviewColor(Color color)
    {
        if (previewInstance == null)
            return;

        SpriteRenderer[] renderers = previewInstance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            Color c = renderer.color;
            c.r = color.r;
            c.g = color.g;
            c.b = color.b;
            renderer.color = c;
        }
    }

    void SetSpriteTransparency(GameObject obj, float alpha)
    {
        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }

    void HandleRightClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        BuildingBase building = BuildingRegistry.Instance.GetBuildingAtPosition(cellPosition);
        if (building != null)
        {
            building.OnRemoved();

            BuildingRegistry.Instance.RemoveBuilding(cellPosition);

            NotifyNeighbors(cellPosition);

            BuildingInstance buildingInstance = building.GetComponent<BuildingInstance>();
            if (buildingInstance != null)
            {
                buildingInstance.Dismantle();
            }
            else
            {
                Destroy(building.gameObject);
            }
            return;
        }

        buildingManager.SelectBuilding(null);

        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }

    void NotifyNeighbors(Vector3Int position)
    {
        Vector3Int[] neighborPositions = new Vector3Int[]
        {
            position + Vector3Int.up,
            position + Vector3Int.down,
            position + Vector3Int.left,
            position + Vector3Int.right
        };

        foreach (Vector3Int neighborPos in neighborPositions)
        {
            BuildingBase neighbor = BuildingRegistry.Instance.GetBuildingAtPosition(neighborPos);
            if (neighbor != null)
            {
                neighbor.UpdateSelf();
            }
        }
    }
}
