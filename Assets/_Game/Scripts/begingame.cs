using UnityEngine;

public class begingame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void onplaybuttonclick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }
    public void onexitbuttonclick()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
