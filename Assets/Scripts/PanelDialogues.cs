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
        person = GameObject.Find("PersonName").GetComponent<TextMeshProUGUI>();
        content = GameObject.Find("DialogueContent").GetComponent<TextMeshProUGUI>();

        dialogues = new Dictionary<string, List<(string, string)>>
        {
            {
                "welcome", new List<(string, string)> {
                    ("Ragazza", "Benvenuto in questo gioco. Muovi il mouse per guardarti intorno. Clicca per proseguire."),
                    ("Ragazza", "Usa i tasti AWDS per spostarti, vieni verso di me.")
                }
            },
            {
                "girl_run", new List<(string, string)> {
                    ("Ragazza", "Ciao, sai che se premi SHIFT mentre cammini corri?"),
                    ("Ragazza", "Vediamo se riesci a prendermi.")
                }
            },
            {
                "girl_jump", new List<(string, string)> {
                    ("Ragazza", "Ottimo. Con la tasto SPACE salti. Se sei fermo salti in alto, se ti muovi salti in avanti."),
                    ("Ragazza", "Salta questo ostacolo.")
                }
            },
            {
                "girl_items", new List<(string, string)> {
                    ("Ragazza", "Puoi raccogliere alcuni degli oggetti che trovi."),
                    ("Ragazza", "Se premi il tasto P aprirai un pannello che ti mostrerà tutti gli oggetti che hai nello zaino."),
                    ("Ragazza", "Se il pannello dello zaino è aperto puoi premere ESC per tornare al menù principale."),
                    ("Ragazza", "Raccogli questi oggetti e verifica la presenza nello zaino.")
                }
            },
            {
                "girl_attack", new List<(string, string)> {
                    ("Ragazza", "Con il tasto C estrai e riponi la spada. Quando hai la spada estratta puoi cliccare per attaccare."),
                    ("Ragazza", "Se premi il tasto Q cambi tipo di videocamera. Mirerai un obbiettivo e non lo perderai d'occhio."),
                    ("Ragazza", "Premi di nuovo Q per tornare alla camera normale."),
                    ("Ragazza", "Mira quel cilindro e colpiscilo con la spada."),
                    ("Ragazza", "Vieni da me quando vuoi allenarti in combattimento."),
                }
            }
        };
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
                    // chiudi il pannello e invoca la callback
                    content.text = string.Empty;
                    this.gameObject.SetActive(false);
                    if (functionToCall != null)
                    {
                        functionToCall?.Invoke();
                    }
                    Player.instance.Resume();
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

    IEnumerator Write((string per, string con) tuple)
    {
        person.text = tuple.per;
        foreach(char c in tuple.con)
        {
            content.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Show(string newState, Action function = null)
    {
        Player.instance.Stop();
        index = 0;
        state = newState;
        functionToCall = function;
        this.gameObject.SetActive(true);
        StartCoroutine(Write(dialogues[state][index]));
    }

}
