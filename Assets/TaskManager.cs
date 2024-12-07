using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public TMP_Text Title;
    public Image Item;
    public TMP_Text Quantity;
    public TMP_Text Time;
    public TMP_Text Reward;
    public List<TaskData> tasks = new List<TaskData>();

    private int _quantity;
    private int _time;
    private ItemType _itemType;
    private int _reward;

    private void Start()
    {
        ApplyTask();
    }

    public void ApplyTask()
    {
        if (tasks.Count == 0)
        {
            return;
        }

        TaskData task = tasks[0];
        tasks.RemoveAt(0);
        Title.text = task.Title;
        Item.sprite = task.ItemImage;
        Quantity.text = task.Quantity.ToString();
        Time.text = task.Time.ToString();
        Reward.text = task.Reward.ToString();
        _quantity = task.Quantity;
        _time = task.Time;
        _itemType = task.ItemType;
        _reward = task.Reward;

        StartCoroutine(ReduceTimeCoroutine());
    }

    public void AddItem(ItemType itemType)
    {
        if (itemType == _itemType)
        {
            _quantity -= 1;
            Quantity.text = _quantity.ToString();

            if (_quantity == 0)
            {
                ResourceManager.Instance.AddResources(_reward);
                ApplyTask();
            }
        }
    }

    private IEnumerator ReduceTimeCoroutine()
    {
        float remainingTime = _time;

        while (remainingTime > 0)
        {
            Time.text = $"Time: {remainingTime}";

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        ApplyTask();
    }
}

[System.Serializable]
public class TaskData
{
    public string Title;
    public Sprite ItemImage;
    public ItemType ItemType;
    public int Quantity;
    public int Time;
    public int Reward;
}
