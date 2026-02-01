using UnityEngine;
using UnityEngine.Video; // Necessario per i video
using UnityEngine.SceneManagement; // Necessario per cambiare scena

public class VideoSceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Trascina qui il componente Video Player
    public string sceneName; // Scrivi qui il nome della scena da caricare

    void Start()
    {
        // Se non hai assegnato il video player manualmente, cerca di prenderlo dallo stesso oggetto
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // "Iscriviti" all'evento che scatta quando il video finisce
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Carica la scena
        SceneManager.LoadScene(sceneName);
    }

    // Opzionale: Se vuoi poter saltare il video premendo spazio
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}