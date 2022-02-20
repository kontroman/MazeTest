using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMazeSize : MonoBehaviour
{
    private void Start()
    {
        MazeCreateor.Instance.onMazeDeleteAction += UpdateSize;
    }

    private void OnDisable()
    {
        MazeCreateor.Instance.onMazeDeleteAction -= UpdateSize;
    }

    public void UpdateSize()
    {
        Camera.main.orthographicSize = MazeCreateor.Instance.GetHighSize();
    }
}
