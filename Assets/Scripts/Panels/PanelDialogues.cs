using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PanelDialogues : MonoBehaviour
{
    public static PanelDialogues instance;
    
    private TextMeshProUGUI person;
    private TextMeshProUGUI content;
    private string state;
    private int index;
    private Dictionary<string, (List<(string, string)>, Action)> dialogues;

    void Awake()
    {
        instance = this;
        person = GameObject.Find("PersonName").GetComponent<TextMeshProUGUI>();
        content = GameObject.Find("DialogueContent").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        this.gameObject.SetActive(false);

        dialogues = new Dictionary<string, (List<(string, string)>, Action)>
        {
            //{
            //    "welcome", (new List<(string, string)> {
            //        ("Ragazza", "Benvenuto in questo gioco. Muovi il mouse per guardarti intorno. Clicca per proseguire."),
            //        ("Ragazza", "Usa i tasti AWDS per spostarti, vieni verso di me.")
            //    }, null)
            //},
            //{
            //    "girl_run", (new List<(string, string)> {
            //        ("Ragazza", "Ciao, sai che se premi SHIFT mentre cammini corri?"),
            //        ("Ragazza", "Vediamo se riesci a prendermi.")
            //    }, Girl.instance.MoveToJump)
            //},
            //{
            //    "girl_jump", (new List<(string, string)> {
            //        ("Ragazza", "Ottimo. Con la tasto SPACE salti. Se sei fermo salti in alto, se ti muovi salti in avanti."),
            //        ("Ragazza", "Salta questo ostacolo.")
            //    }, Girl.instance.MoveToItems)
            //},
            //{
            //    "girl_items", (new List<(string, string)> {
            //        ("Ragazza", "Puoi raccogliere alcuni degli oggetti che trovi."),
            //        ("Ragazza", "Se premi il tasto P aprirai un pannello che ti mostrerà tutti gli oggetti che hai nello zaino."),
            //        ("Ragazza", "Se il pannello dello zaino è aperto puoi premere ESC per tornare al menù principale."),
            //        ("Ragazza", "Raccogli questi oggetti e verifica la presenza nello zaino.")
            //    }, Girl.instance.MoveToAttack)
            //},
            //{
            //    "girl_attack", (new List<(string, string)> {
            //        ("Ragazza", "Con il tasto C estrai e riponi la spada. Quando hai la spada estratta puoi cliccare per attaccare."),
            //        ("Ragazza", "Se premi il tasto Q cambi tipo di videocamera. Mirerai un obbiettivo e non lo perderai d'occhio."),
            //        ("Ragazza", "Premi di nuovo Q per tornare alla camera normale."),
            //        ("Ragazza", "Mira quel cilindro e colpiscilo con la spada."),
            //        ("Ragazza", "Vieni da me quando vuoi allenarti in combattimento."),
            //    }, Girl.instance.MoveToTrain)
            //}
        };

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // se la frase è completa
            if(content.text == dialogues[state].Item1[index].Item2)
            {
                // se non è l'ultima frase
                if(index < dialogues[state].Item1.Count -1)
                {
                    // passa alla frase successiva
                    index++;
                    content.text = string.Empty;
                    StartCoroutine(Write(dialogues[state].Item1[index]));
                }
                else
                {
                    // chiudi il pannello e invoca la callback
                    content.text = string.Empty;
                    this.gameObject.SetActive(false);
                    if (dialogues[state].Item2 != null)
                    {
                        dialogues[state].Item2?.Invoke();
                    }
                    Player.instance.Resume();
                }
            }
            else
            {
                // termina di riempire la frase
                StopAllCoroutines();
                content.text = dialogues[state].Item1[index].Item2;
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

    public void Show(string newState)
    {
        Player.instance.Stop();
        index = 0;
        state = newState;
        this.gameObject.SetActive(true);
        StartCoroutine(Write(dialogues[state].Item1[index]));
    }

}
