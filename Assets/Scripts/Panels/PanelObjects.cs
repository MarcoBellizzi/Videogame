using System;
using TMPro;
using UnityEngine;

public class PanelObjects : MonoBehaviour
{
    public static PanelObjects instance;
    private TextMeshProUGUI objectName;
    private TextMeshProUGUI objectDescription;

    void Awake()
    {
        instance = this;
        objectName = GameObject.Find("ObjectName").GetComponent<TextMeshProUGUI>();
        objectDescription = GameObject.Find("ObjectDescription").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            this.gameObject.SetActive(false);
            Player.instance.Resume();
        }
    }

    public void Show(string name, string description)
    {
        objectName.text = name;
        objectDescription.text = description;
        this.gameObject.SetActive(true);
        Player.instance.Stop();
    }
}
