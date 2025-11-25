using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PanelDialogues : MonoBehaviour
{
    public static PanelDialogues instance;
    private Action functionToCall;
    private TextMeshProUGUI person;
    private TextMeshProUGUI content;
    private string state;
    private int index;
    private Dictionary<string, List<(string, string)>> dialogues;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        person = GameObject.Find("PersonName").GetComponent<TextMeshProUGUI>();
        content = GameObject.Find("DialogueContent").GetComponent<TextMeshProUGUI>();

        dialogues = new Dictionary<string, List<(string, string)>>
        {
            {
                "welcome", new List<(string, string)> {
                    ("Sviluppatore", "Benvenuto in questo gioco. Muovi il mouse per guandarti intorno. Clicca per proseguire."),
                    ("Sviluppatore", "Usa i tasti AWDS per spostarti, vai verso la ragazza.")
                }
            },
            {
                "girl_0", new List<(string, string)> {
                    ("Ragazza", "Ciao, sai che se premi SHIFT mentre cammini corri?"),
                    ("Ragazza", "Vediamo se riesci a prendermi."),
                }
            },
            {
                "girl_1", new List<(string, string)> {
                    ("Ragazza", "Ottimo. Con la tasto SPACE salti. Se sei fermo salti in alto, se ti muovi salti in avanti."),
                    ("Ragazza", "Salta questo ostacolo."),
                }
            },
            {
                "girl_2", new List<(string, string)> {
                    ("Ragazza", "Puoi raccogliere alcuni degli oggetti che trovi. Prova a passare sopra e raccogliere questi oggetti."),
                    ("Ragazza", "Se premi il tasto P aprirai un pannello che ti mostrerà tutti gli oggetti che hai nello zaino."),
                }
            }
        };

        Show("welcome");
    }

    void Update()
    {
        // se il pannello è attivo e clicchi -> avanza gli index
        if (GetActive() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // se la frase è completa
            if(content.text == dialogues[state][index].Item2)
            {
                // se non è l'ultima frase
                if(index < dialogues[state].Count -1)
                {
                    // passa alla frase successiva
                    index++;
                    content.text = string.Empty;
                    StartCoroutine(Write(dialogues[state][index]));
                }
                else
                {
                    content.text = string.Empty;
                    this.gameObject.SetActive(false);
                    if (functionToCall != null)
                    {
                        functionToCall?.Invoke();
                    }
                }
            }
            else
            {
                // termina di riempire la frase
                StopAllCoroutines();
                content.text = dialogues[state][index].Item2;
            }
        }
    }

    public void Show(string newState, Action function = null)
    {
        this.gameObject.SetActive(true);
        state = newState;
        index = 0;
        functionToCall = function;
        StartCoroutine(Write(dialogues[state][index]));
    }

    IEnumerator Write((string per, string con) tuple)
    {
        person.text = tuple.per;
        foreach(char c in tuple.con)
        {
            content.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool GetActive()
    {
        return this.gameObject.activeSelf;
    }

}
