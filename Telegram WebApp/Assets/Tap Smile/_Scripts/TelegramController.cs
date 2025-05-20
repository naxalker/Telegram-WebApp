using UnityEngine;
using System.Runtime.InteropServices;

public class TelegramController : MonoBehaviour
{
    // Импортируем функции из нашего .jslib файла
    // "__Internal" указывает, что функции находятся в том же "модуле" (т.е. в .jslib, скомпилированном с игрой)
#if UNITY_WEBGL && !UNITY_EDITOR // Эти функции будут доступны только в WebGL сборке
    [DllImport("__Internal")]
    private static extern void TelegramWebAppInit();

    [DllImport("__Internal")]
    private static extern void TelegramWebAppDataSend(string data);

    [DllImport("__Internal")]
    private static extern void TelegramWebAppClose();

    [DllImport("__Internal")]
    private static extern void TelegramWebAppShowMainButton(string text, string textColor, string buttonColor);

    [DllImport("__Internal")]
    private static extern void TelegramWebAppHideMainButton();
#endif

    public static TelegramController Instance; // Синглтон для удобства

    // Имя GameObject в сцене, на котором будет этот скрипт
    // Используется в TelegramPlugin.jslib для вызова метода OnMainButtonPressed
    // Убедитесь, что имя этого объекта в сцене совпадает
    public const string CallbackObjectName = "TelegramCallbackHandler";


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.name = CallbackObjectName; // Устанавливаем имя объекта для колбэка
            DontDestroyOnLoad(gameObject); // Чтобы не удалялся при смене сцен, если нужно
        }
        else
        {
            Destroy(gameObject);
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        TelegramWebAppInit(); // Инициализируем Telegram WebApp при старте
        // Пример использования главной кнопки (можно вызывать из другого места по логике игры)
        // ShowMainButton("PRESS ME", "#FFFFFF", "#1A73E8");
#else
        Debug.Log("TelegramController: Not in WebGL build or in Editor. Telegram SDK calls will be skipped.");
#endif
    }

    // Публичные методы для вызова из других скриптов (например, GameManager)
    public void SendDataToBot(string data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        TelegramWebAppDataSend(data);
#else
        Debug.Log($"[Editor/Non-WebGL] SendDataToBot: {data}");
#endif
    }

    public void CloseWebApp()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        TelegramWebAppClose();
#else
        Debug.Log("[Editor/Non-WebGL] CloseWebApp called.");
        // Для удобства отладки в редакторе можно добавить Application.Quit();
        // Application.Quit();
#endif
    }

    public void ShowMainButton(string text, string textColor, string buttonColor)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        TelegramWebAppShowMainButton(text, textColor, buttonColor);
#else
        Debug.Log($"[Editor/Non-WebGL] ShowMainButton: {text}, TextColor: {textColor}, ButtonColor: {buttonColor}");
#endif
    }

    public void HideMainButton()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        TelegramWebAppHideMainButton();
#else
        Debug.Log($"[Editor/Non-WebGL] HideMainButton called.");
#endif
    }

    // --- Метод-обработчик для клика по главной кнопке Telegram ---
    // Этот метод будет вызван из JavaScript (см. TelegramPlugin.jslib)
    public void OnMainButtonPressed()
    {
        Debug.Log("Main Button in Telegram was pressed!");
        // Здесь ваша логика обработки нажатия кнопки
        // Например, отправить результат игры
        // if (GameManager.Instance != null)
        // {
        //     SendDataToBot("{\"score\":" + GameManager.Instance.GetScore() + "}"); // Пример отправки JSON
        // }
        // HideMainButton(); // Можно скрыть кнопку после нажатия
        // CloseWebApp(); // Можно закрыть приложение
    }

    // Пример получения данных пользователя (вызывается из JS)
    // Это для случая, если бы мы использовали SendMessage из JS в TelegramWebAppInit
    // public void SetUserData(string jsonUserData)
    // {
    //    Debug.Log("Received User Data from Telegram: " + jsonUserData);
    //    // Тут можно парсить JSON и использовать данные
    // }
}
