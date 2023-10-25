using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GameStartBtn()
    {
        SceneManager.LoadScene("GameScreen");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
