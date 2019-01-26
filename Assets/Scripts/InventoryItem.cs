
using System;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
    public abstract void UseItem(Action<bool> whenDone);

    public Sprite InventoryIcon;
}

