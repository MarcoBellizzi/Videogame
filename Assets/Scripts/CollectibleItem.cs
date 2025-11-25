using UnityEngine;

/*
 *  Script da attaccare agli oggetti che si desidera inserire nello zaino
 */
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    
    void OnTriggerEnter(Collider other)
    {
        Bag.instance.AddItem(itemName);
        Destroy(this.gameObject);
    }
}
