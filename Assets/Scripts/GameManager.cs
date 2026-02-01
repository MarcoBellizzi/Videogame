using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get;private set; }

    public int frammenti;
    public int bonusSpada;
    public float bonusScudo;

    public bool primoDiscorso;

    public int scena;
    public bool completata;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance!= null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;    

        DontDestroyOnLoad(gameObject);

        Initialize();
        
    }

    private void Initialize()
    {
        //inzializza Variabili. 
        frammenti = 0;
        bonusSpada = 1;
        primoDiscorso = false;
        scena = 1;
        completata = false;
    }
     
    public void addFrammenti()
    {
        frammenti++;
        if (frammenti == 7)
        {
            Player.instance.mask.SetActive(true);
            completata = true;
        }
    }

    
    public void setBonusScudo()
    {
        bonusScudo = 1.5f;
    }

    public void setBonusSpada()
    {
        bonusSpada = 15;
    }


    

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
