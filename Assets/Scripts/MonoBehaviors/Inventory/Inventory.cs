﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public Image[] itemImages = new Image[numItemSlots];
    public Item[] items = new Item[numItemSlots];

    public const int numItemSlots = 4;

    public void AddItem(Item itemToAdd) {
        for(int i = 0; i < items.Length; i++) {
            if(items[i] == null) {  //if the item slot in empty, populate the slot.
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].gameObject.SetActive(true);
                return;
            }
        }
    }

    public void RemoveItem(Item itemToRemove) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == itemToRemove) {  //if the item slot has the item, empty the slot.
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].gameObject.SetActive(false);
                return;
            }
        }
    }
}
