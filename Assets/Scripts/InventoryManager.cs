using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventoryItem[] Options;
    public List<InventoryItem> CurrentInventory = new List<InventoryItem>();
    public List<Image> InventoryImages = new List<Image>();
    public RectTransform UIPanel;
    public RectTransform BuildOptionPrefab;

    public int SelectedItem;
    public int InventoryMax;

    public float TimeForItemDrop;
    private float _elapsedTime;

    private bool _selectionChanged = false;
    public bool InventoryDisabled = false;

    void Start()
    {
        while (CurrentInventory.Count < InventoryMax)
        {
            AddItemToInventory();
        }

        SetSelectedItem(0);
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= TimeForItemDrop)
        {
            AddItemToInventory();
            _elapsedTime -= TimeForItemDrop;
        }

        if(!InventoryDisabled)
        {
            var selectionAxis = Input.GetAxis("Inventory");
            if (_selectionChanged && selectionAxis == 0 && !Input.GetButton("Inventory Use"))
            {
                _selectionChanged = false;
            }

            if (!_selectionChanged && selectionAxis != 0)
            {
                _selectionChanged = true;

                if (selectionAxis < 0)
                {
                    SetSelectedItem(SelectedItem - 1);
                }
                else
                {
                    SetSelectedItem(SelectedItem + 1);
                }
            }

            if (Input.GetButtonDown("Inventory Use") && !_selectionChanged)
            {
                UseInventoryItem();
            }
        }        
    }

    void SetSelectedItem(int index)
    {
        if (CurrentInventory.Count == 0)
        {
            SelectedItem = 0;
            return;
        }

        index = index < 0 ? CurrentInventory.Count - 1 : index % CurrentInventory.Count;
        for (int i = 0; i < CurrentInventory.Count; i++)
        {
            InventoryImages[i].color = i == index ? Color.green : Color.white;
        }
        SelectedItem = index;
    }

    void AddItemToInventory()
    {
        if (CurrentInventory.Count >= InventoryMax)
        {
            return;
        }

        var optionNumber = Random.Range(0, Options.Length);
        var option = Options[optionNumber];

        CurrentInventory.Add(option);
        var inventoryTile = Instantiate(BuildOptionPrefab);
        inventoryTile.SetParent(UIPanel);
        InventoryImages.Add(inventoryTile.GetComponent<Image>());
        SetSelectedItem(SelectedItem);
    }

    void UseInventoryItem()
    {
        if (SelectedItem > CurrentInventory.Count || SelectedItem < 0)
        {
            return;
        }

        CurrentInventory[SelectedItem].UseItem();
        CurrentInventory.RemoveAt(SelectedItem);
        Destroy(InventoryImages[SelectedItem].gameObject);
        InventoryImages.RemoveAt(SelectedItem);
        SetSelectedItem(SelectedItem);
        _selectionChanged = true;
    }
}
