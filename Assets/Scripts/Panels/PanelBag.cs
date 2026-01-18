using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PanelBag : MonoBehaviour
{
    public static PanelBag instance;
    private TextMeshProUGUI content;

    void Awake()
    {
        instance = this;
        content = GameObject.Find("BagContent").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Bag.instance.items.Count == 0)
        {
            content.text = "Lo zaino è vuoto";
        }
        else
        {
            content.text = "";
            foreach (var key in Bag.instance.items.Keys)
            {
                content.text += Bag.instance.items[key];
                content.text += " ";
                content.text += key;
                content.text += "\n";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Menu");
        }
    }
}
