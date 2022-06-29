using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Start Scene µÙ∑π¿Ã
public class StartScene : MonoBehaviour
{    
    void Start()
    {
        Invoke("GoLobbyDelay", 3.0f);
    }

    void GoLobbyDelay()
    {
        SceneManager.LoadScene("Lobby");
    }
}
