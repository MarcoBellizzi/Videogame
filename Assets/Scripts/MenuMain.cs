using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
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
        source.Play();

        GameManager.Instance.scena = 1;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Villaggio2");
    }
    
    // chude l'applicazione
    public void QuitGame()
    {
        Application.Quit();
    }
}
