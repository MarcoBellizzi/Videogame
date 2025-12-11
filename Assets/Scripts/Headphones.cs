using System.Diagnostics;
using UnityEngine;

public class Headphones : MonoBehaviour
{
    [SerializeField] private AudioClip sottofondo;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source.clip = sottofondo;
        // source.Play();
    }

}
