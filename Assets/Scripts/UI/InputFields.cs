using System;
using UnityEngine;
using UnityEngine.UI;

public class InputFields : MonoBehaviour
{
    [SerializeField] private MazeCreateor currentMaze;

    public bool isX;

    private void Start()
    {
        var input = gameObject.GetComponent<InputField>();
        input.onEndEdit.AddListener(GetValues);
    }

    private void GetValues(string arg0)
    {
        if (isX)
            currentMaze.SetNewMazeSizeX(Int32.Parse(arg0));
        else
            currentMaze.SetNewMazeSizeY(Int32.Parse(arg0));
    }
}
