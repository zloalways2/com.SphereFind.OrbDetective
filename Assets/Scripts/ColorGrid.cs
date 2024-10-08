using UnityEngine;
using UnityEngine.UI;
using TMPro; // Добавлено для использования TextMeshPro
using System.Collections;
using UnityEngine.SceneManagement;

public class ColorGrid : MonoBehaviour
{
    public Button[] colorButtons;  // Все кнопки
    private int numberOfButtons;  // Количество кнопок для текущего уровня (9, 12 или 15)
    private int correctButtonIndex;  // Индекс правильной кнопки

    public TMP_Text scoreText; // Заменено на TMP_Text
    public TMP_Text plusTenText; // Заменено на TMP_Text
    public TMP_Text minusTenText; // Новый текст для отображения -10 очков
    public TMP_Text winFinalScoreText; // Текст для отображения финального счета на панели выигрыша
    public TMP_Text loseFinalScoreText; // Текст для отображения финального счета на панели проигрыша
    private int score = 0;

    [Header("Game Settings")]
    public int scoreGoal = 300; // Цель по очкам
    public float timeLimit = 60f; // Лимит времени на уровень

    private float timeRemaining;

    public GameObject losePanel;  // Панель для проигрыша
    public GameObject winPanel;   // Панель для выигрыша

    public Slider timerSlider;  // Ползунок таймера

    private Color baseColor = new Color(0.93f, 1f, 0.13f);  // Жёлтый (#EDFF21)
    private Color differentColor = new Color(0.88f, 0.8f, 0.31f);  // Изменённый жёлтый (#E1CC4F)

    private AudioManager audioManager; // Ссылка на AudioManager

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>(); // Находим AudioManager в сцене
        plusTenText.gameObject.SetActive(false);
        minusTenText.gameObject.SetActive(false); // Скрываем текст -10 очков
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        ResetGame(); // Генерация цветов и начальная инициализация игры
    }

    public void ResetGame()
    {
        score = 0; // Сбрасываем счёт
        UpdateScore(); // Обновляем отображение счёта
        StartTimer(); // Сбрасываем таймер
        GenerateColors(); // Генерируем новые цвета для уровня
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;  // Уменьшаем время
            timerSlider.value = timeRemaining;  // Обновляем значение ползунка таймера

            if (timeRemaining <= 0)
            {
                ShowLosePanel();  // Показываем панель проигрыша
            }
        }
    }

    void GenerateColors()
    {
        // Устанавливаем количество кнопок 9, 12 или 15
        int[] buttonCounts = { 9, 12, 15 };
        numberOfButtons = buttonCounts[Random.Range(0, buttonCounts.Length)];

        // Прячем все кнопки, которые не участвуют в этом уровне
        for (int i = 0; i < colorButtons.Length; i++)
        {
            colorButtons[i].gameObject.SetActive(i < numberOfButtons);
        }

        // Генерируем одну правильную кнопку
        correctButtonIndex = Random.Range(0, numberOfButtons);

        // Присваиваем цвет кнопкам
        for (int i = 0; i < numberOfButtons; i++)
        {
            if (i == correctButtonIndex)
            {
                colorButtons[i].GetComponent<Image>().color = differentColor;
                colorButtons[i].onClick.RemoveAllListeners();
                colorButtons[i].onClick.AddListener(OnCorrectButtonClick);
            }
            else
            {
                colorButtons[i].GetComponent<Image>().color = baseColor;
                colorButtons[i].onClick.RemoveAllListeners();
                colorButtons[i].onClick.AddListener(OnWrongButtonClick);
            }
        }
    }

    void OnCorrectButtonClick()
    {
        Debug.Log("Correct! +10 points");

        score += 10;
        UpdateScore();

        ShowPlusTen();
        audioManager.PlayPlusTenSound(); // Воспроизводим звук получения очков

        // Проверяем, достигли ли мы цели по очкам
        if (score >= scoreGoal)
        {
            ShowWinPanel();  // Показываем панель выигрыша
            audioManager.PlayWinSound(); // Воспроизводим звук выигрыша
        }
        else
        {
            GenerateColors();  // Генерация нового уровня
        }
    }

    void OnWrongButtonClick()
    {
        Debug.Log("Wrong! -10 points");

        score -= 10;  // Вычитаем 10 очков за неверный выбор
        UpdateScore();
        audioManager.PlayWrongButtonSound(); // Воспроизводим звук неверного выбора

        if (score < 0) score = 0;  // Не допускаем отрицательных очков

        ShowMinusTen(); // Показываем визуализацию -10 очков
        GenerateColors();  // Генерация нового уровня
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString() + " / " + scoreGoal.ToString();  // Обновление текста счета
    }

    void ShowPlusTen()
    {
        plusTenText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HidePlusTenAfterTime(0.2f));  // Скрываем текст через 0.5 секунд
    }

    void ShowMinusTen()
    {
        minusTenText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideMinusTenAfterTime(0.2f));  // Скрываем текст через 0.5 секунд
    }

    IEnumerator HidePlusTenAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        plusTenText.gameObject.SetActive(false);
    }

    IEnumerator HideMinusTenAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        minusTenText.gameObject.SetActive(false);
    }

    void StartTimer()
    {
        timeRemaining = timeLimit; // Устанавливаем оставшееся время
        timerSlider.maxValue = timeLimit; // Устанавливаем максимальное значение ползунка
        timerSlider.value = timeRemaining; // Устанавливаем текущее значение ползунка
    }

    void ShowLosePanel()
    {
        losePanel.SetActive(true); // Показываем панель проигрыша
        loseFinalScoreText.text = "Score: " + score + " / " + scoreGoal; // Отображаем финальный счет на панели проигрыша
        audioManager.PlayLoseSound(); // Воспроизводим звук проигрыша
        Time.timeScale = 0; // Останавливаем игру
    }

    void ShowWinPanel()
    {
        winPanel.SetActive(true); // Показываем панель выигрыша
        winFinalScoreText.text = "Score: " + score + " / " + scoreGoal; // Отображаем финальный счет на панели выигрыша
        Time.timeScale = 0; // Останавливаем игру
    }

    // Новый метод для обработки нажатия кнопки "NEXT"
    public void OnNextButtonClick()
    {
        // Скрываем панели и сбрасываем состояние игры
        winPanel.SetActive(false); // Скрываем панель выигрыша
        ResetGame(); // Сбрасываем счёт и таймер
        Time.timeScale = 1; // Возобновляем игру
    }

    // Новый метод для обработки нажатия кнопки "Retry"
    public void OnRetryButtonClick()
    {
        // Скрываем панели и сбрасываем состояние игры
        losePanel.SetActive(false); // Скрываем панель проигрыша
        ResetGame(); // Сбрасываем счёт и таймер
        Time.timeScale = 1; // Возобновляем игру
    }

    // Новый метод для остановки времени
    public void OnPauseButtonClick()
    {
        Time.timeScale = 0; // Останавливаем игру
    }

    // Новый метод для возобновления времени
    public void OnResumeButtonClick()
    {
        Time.timeScale = 1; // Возобновляем игру
    }

    // Метод для перезагрузки текущей сцены
    public void RestartGame()
    {
        // Получаем имя текущей сцены
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Загружаем текущую сцену заново
        SceneManager.LoadScene(currentSceneName);
    }

    public void Exit()
    {
        // Выводим сообщение в консоль для отладки
        Debug.Log("Выход из игры...");

        // Если игра запущена в редакторе, остановим игру
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // В противном случае выходим из приложения
        Application.Quit();
#endif
    }
}
