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
                    ("Ragazza", "Benvenuto in questo gioco. Muovi il mouse per guandarti intorno. Clicca per proseguire."),
                    ("Ragazza", "Usa i tasti AWDS per spostarti, vieni verso di me.")
                }
            },
            {
                "girl_0", new List<(string, string)> {
                    ("Ragazza", "Ciao, sai che se premi SHIFT mentre cammini corri?"),
                    ("Ragazza", "Vediamo se riesci a prendermi.")
                }
            },
            {
                "girl_1", new List<(string, string)> {
                    ("Ragazza", "Ottimo. Con la tasto SPACE salti. Se sei fermo salti in alto, se ti muovi salti in avanti."),
                    ("Ragazza", "Salta questo ostacolo.")
                }
            },
            {
                "girl_2", new List<(string, string)> {
                    ("Ragazza", "Puoi raccogliere alcuni degli oggetti che trovi. Prova a passare sopra e raccogliere questi oggetti."),
                    ("Ragazza", "Se premi il tasto P aprirai un pannello che ti mostrerà tutti gli oggetti che hai nello zaino."),
                    ("Ragazza", "Se il pannello dello zaino è aperto puoi premere ESC per tornare al menù principale.")
                }
            },
            {
                "girl_3", new List<(string, string)> {
                    ("Ragazza", "Se premi il tasto Q cambi tipo di videocamera."),
                    ("Ragazza", "Mirerai un obbiettivo e non lo perderai d'occhio. Prova a tenere d'occhio quel cilindro li a destra"),
                    ("Ragazza", "Premi di nuovo Q per tornare alla camera normale.")
                }
            },
            {
                "girl_4", new List<(string, string)> {
                    ("Ragazza", "Se premi il tasto E cambi di nuovo tipo di videocamera."),
                    ("Ragazza", "Questa è una telecamera di tipo fps, ottima per sparare."),
                    ("Ragazza", "Premi di nuovo E per tornare alla camera normale.")
                }
            },
            {
                "girl_5", new List<(string, string)> {
                    ("Ragazza", "Mentre sei con latelecamera base o la telecamera in lock puoi tirare un pugno se clicchi con il tasto sinistro."),
                    ("Ragazza", "Prova a distruggere quel cilindo."),
                }
            }
        };
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
                    Player.instance.canMove = true;
                    Player.instance.canPunchNext = true;
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
        Player.instance.canMove = false;
        Player.instance.canPunch = false;
        index = 0;
        state = newState;
        functionToCall = function;
        this.gameObject.SetActive(true);
        StartCoroutine(Write(dialogues[state][index]));
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

    public bool GetActive()
    {
        return this.gameObject.activeSelf;
    }

}
