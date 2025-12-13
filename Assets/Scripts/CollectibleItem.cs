using UnityEngine;

/*
 *  Script da attaccare agli oggetti che si desidera inserire nello zaino
 */
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Bag.instance.AddItem(itemName);
            PanelObjects.instance.Show(itemName);
            Destroy(this.gameObject);
        }   
    }
}
