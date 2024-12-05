// BuildingButton.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingButton : MonoBehaviour
{
    public GameObject buildingPrefab;
    public TextMeshProUGUI allowanceText;

    private BuildingData buildingData;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        buildingData = ResourceManager.Instance.GetBuildingData(buildingPrefab);
    }

    void Update()
    {
        if (buildingData != null)
        {
            int remaining = buildingData.maxAllowance - buildingData.currentCount;
            allowanceText.text = remaining.ToString();
        }
    }

    void OnButtonClick()
    {
        BuildingManager.Instance.SelectBuilding(buildingPrefab);
    }
}
