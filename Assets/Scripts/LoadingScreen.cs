using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject nextObject; // Объект, который откроется после загрузки

    private void Start()
    {
        // Делаем объект видимым при старте
        gameObject.SetActive(true);
        
        // Запускаем корутину для скрытия объекта
        StartCoroutine(HideLoadingScreen());
    }

    private IEnumerator HideLoadingScreen()
    {
        // Ждем 3 секунды
        yield return new WaitForSeconds(1.2f);
        
        // Скрываем объект LoadingScreen
        gameObject.SetActive(false);
        
        // Проверяем, что следующий объект установлен, и активируем его
        if (nextObject != null)
        {
            nextObject.SetActive(true);
        }
    }
}