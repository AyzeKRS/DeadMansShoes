using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager       Instance;
    public int                  score           = 0;
    public bool[]               killed          = { false, false, false };

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void ReturnToMenu()
    {
        // Reset values to ensure game resets properly and avoid messing up the endings
        score = 0;

        for (int i = 0; i < killed.Length; i++)
            killed[i] = false;

        SceneManager.LoadScene(0);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NextScene(bool good_point)
    {
        if (good_point)
            score++;
            
        else
        {
            score--;
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 2:                 // First level
                    killed[0] = true;
                    break;

                case 3:                 // Second level
                    killed[1] = true;
                    break;

                case 4:                 // Third level
                    killed[2] = true;
                    break;
            }
        }
            
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
