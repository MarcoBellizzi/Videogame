using UnityEngine;
using UnityEngine.UI;

public class PanelHeathBars : MonoBehaviour
{
    public static PanelHeathBars instance;
    [HideInInspector] public float enemyHealtPoints; // creare un riferimento "statico"
    [HideInInspector] public Slider sliderPlayer;
    [HideInInspector] public Slider sliderEnemy;

    void Awake()
    {
        instance = this;
        sliderPlayer = GameObject.Find("SliderPlayer").GetComponent<Slider>();
        sliderEnemy = GameObject.Find("SliderEnemy").GetComponent<Slider>();
    }
    
    void Start()
    {
        sliderPlayer.maxValue = Player.instance.healthPoints;
        sliderEnemy.gameObject.SetActive(false);
    }

    void Update()
    {

        sliderPlayer.value = Player.instance.healthPoints;

        if (sliderEnemy.gameObject.activeSelf)
        {
            sliderEnemy.value = enemyHealtPoints;
        }


        // colorare le barre
    }
}
