using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { Begin(); });
    }
    void Begin()
    {
        SceneManager.LoadScene(1);//the play scene
    }
}
