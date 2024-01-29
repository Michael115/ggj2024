﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private InputSystemReader _inputReader;
    
    private static GameController _instance;
    
    public static GameController Instance =>
        _instance ? _instance : (_instance = GameObject.FindWithTag("GameController").GetComponent<GameController>());
    
    public InputSystemReader InputReader
    {
        get
        {
            if (_inputReader != null) return _inputReader;
            _inputReader ??= new InputSystemReader(new Input());

            return _inputReader;
        }
    }
    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        _inputReader ??= new InputSystemReader(new Input());
        //NavMesh.AddNavMeshData(NavMeshData);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
