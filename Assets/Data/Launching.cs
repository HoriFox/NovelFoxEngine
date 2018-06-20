using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launching : MonoBehaviour {

	void Start () {
        Invoke("LoadScene", 3);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
