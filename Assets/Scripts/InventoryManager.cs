using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventoryItem[] Options;
    public List<InventoryItem> CurrentInventory = new List<InventoryItem>();
    public List<Image> InventoryImages = new List<Image>();
    public RectTransform UIPanel;
    public RectTransform BuildOptionPrefab;
    private Player _player;

    public Sprite SelectedTile;
    public Sprite UnselectedTile;

    public int SelectedItem;
    public int InventoryMax;

    public float TimeForItemDrop;
    private float _elapsedTime;

    private bool _selectionChanged = false;
    public bool InventoryDisabled = false;

    public AudioSource nope;
    public AudioSource change;

    void Start()
    {
        _player = FindObjectOfType<Player>();

        while (CurrentInventory.Count < InventoryMax)
        {
            CurrentInventory.Add(null);
        }

        InventoryImages = UIPanel.GetComponentsInChildren<Image>().Where(x => x.name == "Build Option").ToList();

        LayoutRebuilder.ForceRebuildLayoutImmediate(UIPanel);

        AddItemToInventory();
        AddItemToInventory();
        AddItemToInventory();
        AddItemToInventory();
        AddItemToInventory();

        SetSelectedItem(0);
    }

    void Update()
    {
        if (_player.IsDead)
        {
            return;
        }

        //_elapsedTime += Time.deltaTime;
        //if(_elapsedTime >= TimeForItemDrop)
        //{
        //    AddItemToInventory();
        //    _elapsedTime -= TimeForItemDrop;
        //}

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
                change.Play();

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
                BeginUseInventoryItem();
            }
        }        
    }

    void SetSelectedItem(int index)
    {
        index = index < 0 ? CurrentInventory.Count - 1 : index % CurrentInventory.Count;
        for (int i = 0; i < CurrentInventory.Count; i++)
        {
            InventoryImages[i].sprite = i == index ? SelectedTile : UnselectedTile;
            var promptImage = InventoryImages[i].transform.Find("Activate").GetComponent<Image>();
            promptImage.enabled = i == index;
        }
        SelectedItem = index;
    }

    void AddItemToInventory()
    {
        var firstEmptySlot = CurrentInventory.FindIndex(x => x == null);
        if (firstEmptySlot < 0)
        {
            return;
        }

        var optionNumber = Random.Range(0, Options.Length);
        var option = Options[optionNumber];

        CurrentInventory[firstEmptySlot] = option;
        var iconImage = InventoryImages[firstEmptySlot].transform.Find("MenuIconImage").GetComponent<Image>();
        iconImage.sprite = option.InventoryIcon;
        iconImage.color = new Color(1, 1, 1, 1);
    }

    void BeginUseInventoryItem()
    {
        if (CurrentInventory[SelectedItem] == null)
        {
            nope.Play();
            return;
        }

        InventoryDisabled = true;
        CurrentInventory[SelectedItem].UseItem(FinishUseInventoryItem);
    }

    void FinishUseInventoryItem(bool used)
    {
        if (used)
        {
            CurrentInventory[SelectedItem] = null;
            var iconImage = InventoryImages[SelectedItem].transform.Find("MenuIconImage").GetComponent<Image>();
            iconImage.color = new Color(0, 0, 0, 0);
            AddItemToInventory();
        }
        else
        {
            nope.Play();
        }

        _selectionChanged = true;
        InventoryDisabled = false;
    }
}
