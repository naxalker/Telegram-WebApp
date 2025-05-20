// Assets/Plugins/WebGL/TelegramPlugin.jslib
mergeInto(LibraryManager.library, {

    // Функция для инициализации WebApp и получения данных пользователя (если нужно)
    TelegramWebAppInit: function () {
        if (window.Telegram && window.Telegram.WebApp) {
            console.log("Telegram WebApp SDK Injected and Initializing.");
            window.Telegram.WebApp.ready(); // Сообщаем Telegram, что приложение загрузилось и готово

            // Пример: получение данных пользователя
            // var initData = window.Telegram.WebApp.initDataUnsafe;
            // if (initData && initData.user) {
            //    console.log("User Data:", JSON.stringify(initData.user));
            //    // Можно отправить эти данные в Unity через SendMessage
            //    // unityInstance.SendMessage('MyGameObjectName', 'SetUserData', JSON.stringify(initData.user));
            // }

            // Пример: настройка кнопки закрытия (если нужна)
            // window.Telegram.WebApp.BackButton.show();
            // window.Telegram.WebApp.onEvent('backButtonClicked', function() {
            //    window.Telegram.WebApp.close();
            // });

            console.log("Telegram.WebApp.initDataUnsafe:", JSON.stringify(window.Telegram.WebApp.initDataUnsafe));
            console.log("Telegram.WebApp.colorScheme:", window.Telegram.WebApp.colorScheme);
            console.log("Telegram.WebApp.platform:", window.Telegram.WebApp.platform);

        } else {
            console.error("Telegram WebApp SDK not found. Make sure telegram-web-app.js is included.");
        }
    },

    // Функция для отправки данных (например, счета) боту
    TelegramWebAppDataSend: function (data) {
        var stringData = UTF8ToString(data); // Конвертируем C# string в JS string
        if (window.Telegram && window.Telegram.WebApp) {
            console.log("Sending data to bot:", stringData);
            window.Telegram.WebApp.sendData(stringData);
            // Обычно после sendData приложение закрывается, если это финальное действие
            // window.Telegram.WebApp.close();
        } else {
            console.error("Telegram WebApp SDK not found. Cannot send data.");
        }
    },

    // Функция для закрытия WebApp
    TelegramWebAppClose: function () {
        if (window.Telegram && window.Telegram.WebApp) {
            window.Telegram.WebApp.close();
        } else {
            console.error("Telegram WebApp SDK not found. Cannot close.");
        }
    },

    // Пример: Показать главную кнопку
    TelegramWebAppShowMainButton: function (text, textColor, buttonColor) {
        if (window.Telegram && window.Telegram.WebApp) {
            var jsText = UTF8ToString(text);
            var jsTextColor = UTF8ToString(textColor);
            var jsButtonColor = UTF8ToString(buttonColor);

            window.Telegram.WebApp.MainButton.setText(jsText);
            window.Telegram.WebApp.MainButton.setParams({
                text_color: jsTextColor, // например, '#ffffff'
                color: jsButtonColor,      // например, '#2481cc'
                // is_active: true,
                // is_visible: true
            });
            window.Telegram.WebApp.MainButton.show();
            // Чтобы Unity узнала о клике по этой кнопке:
            // unityInstance будет глобальной переменной, указывающей на ваш Unity инстанс.
            // Имя 'TelegramCallbackHandler' - это имя GameObject в вашей сцене Unity.
            // 'OnMainButtonPressed' - это имя публичного метода в скрипте на этом GameObject.
            window.Telegram.WebApp.onEvent('mainButtonClicked', function () {
                // Вместо MyGameObjectName используйте имя объекта, на котором будет скрипт-обработчик
                // Вместо OnMainButtonPressed - имя метода в этом скрипте
                if (typeof unityInstance !== 'undefined') {
                    unityInstance.SendMessage('TelegramCallbackHandler', 'OnMainButtonPressed');
                } else {
                    console.error("unityInstance is not defined! Cannot send message to Unity.");
                }
            });
        }
    },

    // Скрыть главную кнопку
    TelegramWebAppHideMainButton: function () {
        if (window.Telegram && window.Telegram.WebApp) {
            window.Telegram.WebApp.MainButton.hide();
        }
    }
});