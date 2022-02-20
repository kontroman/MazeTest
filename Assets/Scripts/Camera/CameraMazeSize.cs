using UnityEngine;

//Масштабируем камеру под размеры нового лабиринта
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
