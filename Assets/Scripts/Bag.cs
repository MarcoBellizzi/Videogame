using UnityEngine;
using System.Collections.Generic;

public class Bag : MonoBehaviour
{
    public static Bag instance;
    public Dictionary<string, int> items;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        items = new Dictionary<string, int>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PanelBag.instance.gameObject.SetActive(! PanelBag.instance.gameObject.activeSelf);
        }
    }

    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] += 1;
        }
        else
        {
            items[itemName] = 1;
        }
    }

    public int GetItemCount(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            return items[itemName];
        }
        return 0;
    }

    public void ConsumeItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName]--;
            if (items[itemName] == 0)
            {
                items.Remove(itemName);
            }
        }
        else
        {
            Debug.Log("Cannot consume " + itemName);
        }
    }
}
