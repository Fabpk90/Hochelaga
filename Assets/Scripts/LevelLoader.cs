using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private Button btn;
    public int levelIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
