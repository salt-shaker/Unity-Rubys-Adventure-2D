using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start_Scene_Buttons : MonoBehaviour
{

    public Button m_btnStartNew, m_btnExit;

    // Start is called before the first frame update
    void Start()
    {

        m_btnExit.onClick.AddListener(quitGame);
        m_btnStartNew.onClick.AddListener(loadNewScene);
        
    }

    void quitGame()
    {
        Application.Quit();
    }

    void loadNewScene()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
