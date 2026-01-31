using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelChoice : MonoBehaviour
{
    public static PanelChoice instance;

    private TextMeshProUGUI question;
    private GameObject answersContainer;
    private TextMeshProUGUI[] answers;
    private int selectedIndex;
    private string state;
    private Dictionary<string, (string, List<(string, Action)>)> choises;

    void Awake()
    {
        instance = this;
        question = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();
        answersContainer = GameObject.Find("AnswersContainer");
    }

    void Start()
    {
        this.gameObject.SetActive(false);

        choises = new Dictionary<string, (string, List<(string, Action)>)>
        {
        //    {
        //        "train", ("Vuoi affrontarmi in combattimento?", new List<(string, Action)> {
        //            ("Si, affrontiamoci", Girl.instance.Unsheathe),
        //            ("No, ho bisogno di allenarmi", Girl.instance.Idle)
        //        })
        //    }
        };
    }

    void UpdateVisualSelection()
    {
        for (int i = 0; i < answers.Length; i++)
        {
            if (i == selectedIndex)
            {
                answers[i].color = Color.blue;
            }
            else
            {
                answers[i].color = Color.black;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateVisualSelection();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = Mathf.Min(answers.Length - 1, selectedIndex + 1);
            UpdateVisualSelection();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            this.gameObject.SetActive(false);
            choises[state].Item2[selectedIndex].Item2?.Invoke();
        }
    }


    public void Show(string state_id)
    {
        Player.instance.Stop();

        state = state_id;
        question.text = choises[state].Item1;

        for (int i = answersContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(answersContainer.transform.GetChild(i).gameObject);
        }

        answers = new TextMeshProUGUI[choises[state].Item2.Count];

        for (int i=0; i<choises[state].Item2.Count; i++)
        {
            GameObject answer = new GameObject("Answer", typeof(RectTransform), typeof(TextMeshProUGUI));

            TextMeshProUGUI tmp = answer.GetComponent<TextMeshProUGUI>();
            tmp.text = choises[state].Item2[i].Item1;
            tmp.fontSize = 24;
            tmp.color = Color.black;

            RectTransform rt = answer.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(500f, 30f);

            answer.transform.SetParent(answersContainer.transform, worldPositionStays: false);

            answers[i] = tmp;
        }


        selectedIndex = 0;
        UpdateVisualSelection();
        this.gameObject.SetActive(true);
    }

}
