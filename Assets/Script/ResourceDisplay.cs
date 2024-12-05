using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    public TextMeshProUGUI resourceText;

    void Update()
    {
        resourceText.text = "Resources: " + ResourceManager.Instance.totalResources;
    }
}
