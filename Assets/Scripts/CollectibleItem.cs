using TMPro;
using UnityEngine;

/*
 *  Script da attaccare agli oggetti che si desidera inserire nello zaino
 */
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    private Canvas canvas;
    private TextMeshProUGUI content;
    private Vector3 canvasOffset;
    private bool isClicking;

    void Awake()
    {
        canvas = GameObject.Find("CanvasOnTop").GetComponent<Canvas>();
        content = GameObject.Find("TextOnTop").GetComponent<TextMeshProUGUI>();
        canvasOffset = new Vector3(0, 1f, 0);
        isClicking = false;
    }

    void Update()
    {
        // non posso controllare il click del personaggio nel OnTriggerStay perchè viene chiamato meno dell update
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isClicking = true; 
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isClicking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            canvas.gameObject.SetActive(true);
            content.text = "Clicca per raccogliere";
            Player.instance.Stop();
            Player.instance.canMove = true; 
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {        
            canvas.transform.position = transform.position + canvasOffset;
            canvas.transform.rotation = 
                Quaternion.LookRotation(canvas.transform.position - MainCamera.instance.transform.position);

            if (isClicking)
            {
                Bag.instance.AddItem(itemName, itemDescription);
                PanelObjects.instance.Show(itemName, itemDescription);
                canvas.gameObject.SetActive(false); 
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            canvas.gameObject.SetActive(false);
            Player.instance.Resume();
        }
    }
}
