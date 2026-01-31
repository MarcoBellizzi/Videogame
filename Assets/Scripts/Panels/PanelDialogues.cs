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
            // {
            //     "welcome", (new List<(string, string)> {
            //         ("Ragazza", "Benvenuto in questo gioco. Muovi il mouse per guardarti intorno. Clicca per proseguire."),
            //         ("Ragazza", "Usa i tasti AWDS per spostarti, vieni verso di me.")
            //     }, null)
            // },
            // {
            //     "girl_run", (new List<(string, string)> {
            //         ("Ragazza", "Ciao, sai che se premi SHIFT mentre cammini corri?"),
            //         ("Ragazza", "Vediamo se riesci a prendermi.")
            //     }, Girl.instance.MoveToJump)
            // },
            // {
            //     "girl_jump", (new List<(string, string)> {
            //         ("Ragazza", "Ottimo. Con la tasto SPACE salti. Se sei fermo salti in alto, se ti muovi salti in avanti."),
            //         ("Ragazza", "Salta questo ostacolo.")
            //     }, Girl.instance.MoveToItems)
            // },
            // {
            //     "girl_items", (new List<(string, string)> {
            //         ("Ragazza", "Puoi raccogliere alcuni degli oggetti che trovi."),
            //         ("Ragazza", "Se premi il tasto P aprirai un pannello che ti mostrerà tutti gli oggetti che hai nello zaino."),
            //         ("Ragazza", "Se il pannello dello zaino è aperto puoi premere ESC per tornare al menù principale."),
            //         ("Ragazza", "Raccogli questi oggetti e verifica la presenza nello zaino.")
            //     }, Girl.instance.MoveToAttack)
            // },
            // {
            //     "girl_attack", (new List<(string, string)> {
            //         ("Ragazza", "Con il tasto C estrai e riponi la spada. Quando hai la spada estratta puoi cliccare per attaccare."),
            //         ("Ragazza", "Se premi il tasto Q cambi tipo di videocamera. Mirerai un obbiettivo e non lo perderai d'occhio."),
            //         ("Ragazza", "Premi di nuovo Q per tornare alla camera normale."),
            //         ("Ragazza", "Mira quel cilindro e colpiscilo con la spada."),
            //         ("Ragazza", "Vieni da me quando vuoi allenarti in combattimento."),
            //     }, Girl.instance.MoveToTrain)
            // },
            {
                "welcome", (new List<(string, string)> {
                    ("", "Soter si trova nel villaggio e riceve una lettera da Prosos:"),
                    ("", "\"Sono vivo, ma sono in pericolo, devi venirmi a salvare."),
                    ("", "Per combattere il tuo nemico ti servirà l'antica maschera. Vieni al Monte Tmolos per la resa dei conti.\""),
                    ("Soter", "Ma... Com'è possibile? "),
                    ("Soter", "Prosos è morto durante l'alluvione, o meglio... l'abbiamo lasciato morire."),
                    ("Soter", "Devo subito andare al Monte Tmolos per controllare, ma mi serve l'antica maschera, non se ne vede una dai tempi dell'alluvione..."),
                    ("Soter", "Devo parlare subito con Thàleia e Anànke."),
                }, null)
            },
            {
                "thaelia_1", (new List<(string, string)> {
                    ("Thaleia", "Ciao Soter, ho sentito che stai cercando Prosos, ma com’è possibile?"),
                    ("Thaleia", "Quella lettera è molto strana…"),
                    ("Soter", "Ciao Thaleia, sì, non so cosa pensare… "),
                    ("Soter", "Puoi darmi una mano a trovarlo?"),
                    ("Soter", "Devo assolutamente capire cosa si nasconde dietro questa storia"),
                    ("Thaleia", "Certo Soter, ma dovrai rispondere correttamente al quesito del vecchio fabbro, così potrà ricompensarti.")
                }, Thaelia.instance.AskQuestion)
            },
            {
                "thaelia_2", (new List<(string, string)> {
                    ("Ananke", "In bocca al lupo amore mio, torna presto e stai attento."),
                }, Thaelia.instance.Idle)
            },
            {
                "ananke_1", (new List<(string, string)> {
                    ("Ananke", "Ciao Soter, ho sentito che stai cercando Prosos, ma com'è possibile?"),
                    ("Ananke", "Ho saputo della tua ricerca, non riesco a credere che Prosos sia ancora vivo, dopo 15 anni…"),
                    ("Soter", "Neanche io, ma adesso devo necessariamente andare al Monte Tmolos per sapere la verità!"),
                    ("Ananke", "Sappiamo benissimo cos’è successo quella notte del 2020, abbiamo visto Prosos morire…"),
                    ("Ananke", "Non avvicinarti troppo alla verità, potresti scoprire qualcosa di scomodo."),
                    ("Soter", "Andrò in fondo a questa storia ed espierò le mie colpe, devi aiutarmi!"),
                    ("Soter", "Sei stato un po' … meschino… a comportarti in quel modo quella notte…"),
                    ("Ananke", "Va bene, se rispondi correttamente a questo quesito dell'oracolo potrai avere una ricompensa degna della battaglia che ti spetta.")
                }, Ananke.instance.AskQuestion)
            },
            {
                "ananke_2", (new List<(string, string)> {
                    ("Ananke", "Ciò che cerchi può non essere ciò che speri di avere."),
                }, Ananke.instance.Idle)
            },
            {
                "prosos", (new List<(string, string)> {
                    ("Soter", "PROSOS! Ma... Com'è possibile? Ti ho visto morire davanti ai miei occhi..."),
                    ("Prosos", "Soter, amico mio, è il momento di fare i conti con il passato."),
                    ("Prosos", "Quella notte non sono morto, anzi, sono rinato"),
                    ("Prosos", "con questi poteri potrò finalmente farti pagare il giusto pegno per i tuoi errori passati, per il tuo abbandono."),
                    ("Soter", "Sono stato costretto da Ananke a dare la maschera a sua figlia..."),
                    ("Soter", "L'ironia delle sorte ha voluto che ci sposassimo, il dolore di quella notte ci ha uniti."),
                    ("Prosos", "So tutto, Soter! In questi quindici anni ho indossato questa maschera per osservare indisturbato le storture del mondo."),
                    ("Prosos", "Adesso siamo alla resa dei conti, battiamoci e paga col sangue."),
                    ("Soter", "No Prosos, fammi spiegare!"),
                }, Prosos.instance.StartFight)
            },
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
                    Player.instance.Resume();
                    if (dialogues[state].Item2 != null)
                    {
                        dialogues[state].Item2?.Invoke();
                    }
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
            // yield return new WaitForSeconds(0.0005f);
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
