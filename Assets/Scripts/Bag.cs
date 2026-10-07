using UnityEngine;
using System.Collections.Generic;

public class Bag : MonoBehaviour
{
    public static Bag instance;
    public Dictionary<string, (string, int)> items;

    void Awake()
    {
        instance = this;
        items = new Dictionary<string, (string, int)>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !PanelDialogues.instance.gameObject.activeSelf && !PanelObjects.instance.gameObject.activeSelf)
        {
            if (!PanelBag.instance.gameObject.activeSelf)
            {
                PanelBag.instance.gameObject.SetActive(true);
                Player.instance.Stop();
            }
            else
            {
                PanelBag.instance.gameObject.SetActive(false);
                Player.instance.Resume();
            }
            
        }
    }

    public void AddItem(string itemName, string itemDescription)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] = (items[itemName].Item1, items[itemName].Item2 + 1);
        }
        else
        {
            items[itemName] = (itemDescription, 1);
        }
    }

    public int GetItemCount(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            return items[itemName].Item2;
        }
        return 0;
    }

    // public void ConsumeItem(string itemName)
    // {
    //     if (items.ContainsKey(itemName))
    //     {
    //         items[itemName].Item2--;
    //         if (items[itemName] == 0)
    //         {
    //             items.Remove(itemName);
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("Cannot consume " + itemName);
    //     }
    // }
}
