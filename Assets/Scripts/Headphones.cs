using System.Diagnostics;
using UnityEngine;

public class Headphones : MonoBehaviour
{
    [SerializeField] private AudioClip sottofondo1;
    [SerializeField] private AudioClip sottofondo2;
    [SerializeField] private AudioClip sottofondo3;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (GameManager.Instance.scena == 1)
        {
            source.clip = sottofondo1;
        }
        if (GameManager.Instance.scena == 2)
        {
            source.clip = sottofondo2;
        }
        if (GameManager.Instance.scena == 3)
        {
            source.clip = sottofondo3;
        }
        source.Play();
    }

}
