using System.Collections;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public static PlayerCore instance;

    [Header("Components")]
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Transform playerOrientation;
    [HideInInspector] public Transform toFollowVirtual;

    [Header("Settings & Flags")]
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool canAttackNext;
    [HideInInspector] public bool isHolding;
    [HideInInspector] public bool canGetHit;

    // Riferimenti agli oggetti fisici (utilizzati potenzialmente da altri script o animazioni)
    [SerializeField] public Transform playerModel;
    [SerializeField] public Transform sword;
    [SerializeField] public Transform swordPoint;
    [SerializeField] public Transform rightHandPoint;

    void Awake()
    {
        instance = this;
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        // Assicurati che questi oggetti esistano nella scena o gestisci l'errore
        var orientationObj = GameObject.Find("Orientation");
        if (orientationObj) playerOrientation = orientationObj.transform;

        var followObj = GameObject.Find("ToFollow");
        if (followObj) toFollowVirtual = followObj.transform;

        InitializeState();
    }

    void Start()
    {
        
    }

    private void InitializeState()
    {
       
         
    }

   

    // Metodi globali per bloccare/sbloccare il giocatore (usati dai Menu)
    public void Stop()
    {
        canMove = false;
        canAttack = false;
        // Resetta animazioni di movimento quando ci si ferma forzatamente
        animator.SetFloat("horInput", 0);
        animator.SetFloat("verInput", 0);
        animator.SetFloat("speed", 0);
    }

    public void Resume()
    {
        canMove = true;
        canAttackNext = true;
    }
}