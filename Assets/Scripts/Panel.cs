using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class Panel : MonoBehaviour
{
    // singleton
    public static Panel Instance;
    private Action funzioneDaChiamare;

    private TextMeshProUGUI persona;
    private TextMeshProUGUI testo;

    private string stato;
    private int index;
    
    private Dictionary<string, List<(string, string)>> conversazioni = new Dictionary<string, List<(string, string)>>
    {
        {
            "benvenuto", new List<(string, string)> {
                ("Sviluppatore", "Benvenuto in questo gioco. Muovi il mouse per guandarti intorno. Clicca per proseguire."),
                ("Sviluppatore", "Usati i tasti AWDS per spostarti, vai verso la ragazza.")
            }
        },
        {
            "ragazza_1", new List<(string, string)> {
                ("ragazza", "Ciao, sai che se premi shift mentre cammini corri? Prova a prendermi."),
            }
        }
    };

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        persona = GameObject.Find("NomePersona").GetComponent<TextMeshProUGUI>();
        testo = GameObject.Find("TestoConversazione").GetComponent<TextMeshProUGUI>();
        Mostra("benvenuto");
    }

    void Update()
    {
        // se il pannello è attivo e clicchi -> avanza gli index
        if (GetAttivo() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // se la frase è completa
            if(testo.text == conversazioni[stato][index].Item2)
            {
                // se non è l'ultima frase
                if(index < conversazioni[stato].Count -1)
                {
                    // passa alla frase successiva
                    index++;
                    testo.text = string.Empty;
                    StartCoroutine(Scrivi(conversazioni[stato][index]));
                }
                else
                {
                    testo.text = string.Empty;
                    gameObject.SetActive(false);
                    if (funzioneDaChiamare != null)
                    {
                        funzioneDaChiamare?.Invoke();
                    }
                }
            }
            else
            {
                // termina di riempire la frase
                StopAllCoroutines();
                testo.text = conversazioni[stato][index].Item2;
            }
        }
    }

    public void Mostra(string nuovoStato, Action funzione = null)
    {
        gameObject.SetActive(true);
        stato = nuovoStato;
        index = 0;
        funzioneDaChiamare = funzione;
        StartCoroutine(Scrivi(conversazioni[stato][index]));
    }

    IEnumerator Scrivi((string per, string dis) tupla)
    {
        persona.text = tupla.per;
        foreach(char c in tupla.dis)
        {
            testo.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool GetAttivo()
    {
        return gameObject.activeSelf;
    }

}
