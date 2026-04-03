using UnityEngine;
using UnityEngine.InputSystem;

// You were missing this line below:
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetTimeFlow(bool isMoving)
    {
        if (isMoving)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}