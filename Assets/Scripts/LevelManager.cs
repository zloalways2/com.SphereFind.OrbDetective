using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject levelPanel;  // Панель уровней
    public GameObject gamePanel;    // Игровая панель
    public Button[] levelButtons;   // Кнопки уровней
    public GameObject winPanel;      // Панель победы
    public GameObject losePanel;     // Панель поражения

    private int currentLevel = 1;    // Текущий уровень
    private int totalLevels = 10;     // Общее количество уровней
    private int unlockedLevels;        // Открытые уровни

    void Start()
    {
        unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1); // Получаем количество открытых уровней из PlayerPrefs
        UpdateLevelButtons(); // Обновляем состояние кнопок уровней
        levelPanel.SetActive(true); // Показываем панель уровней
        gamePanel.SetActive(false); // Скрываем игровую панель
    }

    public void OnLevelButtonClicked(int level)
    {
        currentLevel = level;
        StartGame();
    }

    void StartGame()
    {
        levelPanel.SetActive(false); // Скрываем панель уровней
        gamePanel.SetActive(true);   // Показываем игровую панель

        // Здесь можно инициализировать игровую механику для текущего уровня
        Debug.Log("Запущен уровень: " + currentLevel); // Дебаг: текущий уровень
    }

    public void LevelCompleted()
    {
        unlockedLevels = Mathf.Min(unlockedLevels + 1, totalLevels); // Увеличиваем количество открытых уровней
        PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels); // Сохраняем количество открытых уровней
        PlayerPrefs.Save();
        Debug.Log("Разблокирован уровень: " + (unlockedLevels)); // Дебаг: разблокировка уровня
        winPanel.SetActive(true); // Показываем панель победы
    }

    public void RetryLevel()
    {
        // Логика для перезапуска текущего уровня
        gamePanel.SetActive(true); // Показываем игровую панель
        losePanel.SetActive(false); // Скрываем панель поражения
    }

    public void NextLevel()
    {
        // Проверяем, если текущий уровень меньше общего количества уровней
        if (currentLevel < totalLevels)
        {
            currentLevel++;  // Увеличиваем номер текущего уровня
            unlockedLevels = Mathf.Min(unlockedLevels + 1, totalLevels);  // Разблокируем следующий уровень
        
            // Сохраняем количество открытых уровней
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels);
            PlayerPrefs.Save();

            // Обновляем кнопки уровней для корректной разблокировки
            UpdateLevelButtons();

            // Скрываем панель победы
            winPanel.SetActive(false);

            // Находим компонент ColorGrid и сбрасываем игру для нового уровня
            ColorGrid colorGrid = FindObjectOfType<ColorGrid>();
            if (colorGrid != null)
            {
                colorGrid.ResetGame();
            }

            // Активируем игровой экран
            gamePanel.SetActive(true);

            // Запускаем следующий уровень
            StartGame();

            Debug.Log("Перешли на уровень " + currentLevel + ". Разблокированные уровни: " + unlockedLevels);
        }
        else
        {
            Debug.Log("Все уровни завершены!");
        }
    }


    void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < unlockedLevels)
            {
                levelButtons[i].interactable = true; // Кнопка активна
                int levelIndex = i + 1; // Индекс уровня
                levelButtons[i].onClick.AddListener(() => OnLevelButtonClicked(levelIndex)); // Добавляем слушателя нажатия
            }
            else
            {
                levelButtons[i].interactable = false; // Кнопка неактивна
            }
        }
    }
}
