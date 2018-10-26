using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour {

    private UnityEngine.UI.Button thisButton;

    // Use this for initialization
    void Start()
    {

        thisButton = gameObject.GetComponent<UnityEngine.UI.Button>();
        thisButton.onClick.AddListener(OnClick);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {

        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

}