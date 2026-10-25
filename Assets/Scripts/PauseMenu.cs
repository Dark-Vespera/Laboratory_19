using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Если уже на паузе - продолжаем
            }
            else
            {
                PauseGame(); // Если играем - ставим на паузу
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Прячем меню
        Time.timeScale = 1f; // Возвращаем время
        Cursor.lockState = CursorLockMode.Locked; // Прячем курсор
        Cursor.visible = false;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Показываем меню
        Time.timeScale = 0f; // Останавливаем время
        Cursor.lockState = CursorLockMode.None; // Показываем курсор
        Cursor.visible = true;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}