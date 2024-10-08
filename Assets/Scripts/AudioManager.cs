using UnityEngine;
using UnityEngine.UI; // Добавлено для использования UI
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip loseSound;      // Звук для проигрыша
    public AudioClip winSound;       // Звук для выигрыша
    public AudioClip plusTenSound;   // Звук для получения 10 очков
    public AudioClip wrongButtonSound; // Звук для неверной кнопки
    public AudioSource backgroundMusic; // Компонент AudioSource для фоновой музыки

    // Слайдеры для управления громкостью
    public Slider backgroundMusicSlider; // Слайдер для фоновой музыки
    public Slider soundEffectsSlider;     // Слайдер для звуковых эффектов

    private void Start()
    {
        PlayBackgroundMusic(); // Запускаем фоновую музыку при старте

        // Устанавливаем начальные значения громкости
        backgroundMusicSlider.value = backgroundMusic.volume;
        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f); // Получаем значение громкости из PlayerPrefs

        // Привязываем методы к событиям слайдеров
        backgroundMusicSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
        soundEffectsSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
    }

    public void PlayLoseSound()
    {
        PlaySound(loseSound);
    }

    public void PlayWinSound()
    {
        PlaySound(winSound);
    }

    public void PlayPlusTenSound()
    {
        PlaySound(plusTenSound);
    }

    public void PlayWrongButtonSound()
    {
        PlaySound(wrongButtonSound);
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    private void SetBackgroundMusicVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume; // Устанавливаем громкость фоновой музыки
        }
    }

    private void SetSoundEffectsVolume(float volume)
    {
        // Устанавливаем громкость звуковых эффектов (временно)
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume); // Сохраняем громкость в PlayerPrefs
    }

    private void PlaySound(AudioClip clip)
    {
        // Создаём временный AudioSource для воспроизведения звуковых эффектов
        GameObject tempGO = new GameObject("TempAudio"); // Создаём временный объект
        AudioSource tempAudioSource = tempGO.AddComponent<AudioSource>(); // Добавляем AudioSource
        tempAudioSource.clip = clip; // Устанавливаем аудиоклип
        tempAudioSource.volume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f); // Устанавливаем громкость из PlayerPrefs
        tempAudioSource.Play(); // Воспроизводим звук
        Destroy(tempGO, clip.length); // Уничтожаем временный объект через длину клипа
    }
}
