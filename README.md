## Перед запуском
Необходимо заполнить поля `Token` и `Secret` файла `appsettings.json`, иначе выполнение запросов будет генерировать исключение  `EmptyTokenException` .

## Описание

Веб-сервис,  стандратизирующий полученный от пользователя адрес и возвращающий модель с определенными полями. Стандартизация адреса производится сервисом DaData (<https://dadata.ru/api/clean/address/>) по КЛАДР / ФИАС (ГАР).

Возвращаемая моедль имеет вид:
```
    {
      "country": "string",
      "city": "string",
      "street": "string",
      "house": "string",
      "flat": "string"
    }
```


Формат модели подразумевает, что вводимый пользователем адрес должен содержать в себе достаточно информации для стандартизации таких полей как `house` и `flat`. Помимо этого, вводимая пользователем строка может иметь лишнюю информацию или не быть адресом вовсе.

Api сервиса DaData позволяет определять описанные выше исключения, поэтому для проверки полноты адреса и его реальности из ответа сервиса DaData, помимо основных полей адреса, выделяются поля `qc` и `fias_level`.


Код проверки `qc` указыает на уверенность сервиса DaData в качестве распознования полученного адреса. Для тестового задания был выбран единственный подходящий код 0, говорящий о полной уверенности в распозновании. Остальные коды генерируют исключение`GarbageAdressException`, возвращающее пользователю код 400 и сообщение о некачественно введенном адресе.


Уровень детализации, до которого адрес найден в ФИАС `fias_level`. Код 9 соответствует уровню квартиры, что соответсвует модели разработанного сервиса. Коды ниже 9 вызывают исключение `FiasLevelException`, которое обрабаывается аналогично `GarbageAdressException`.


####  Выполнены  следующие требования

- ASP.NET Core WebApi 
	.Net 8.0
- Реализовани 1 GET эндпоинт контроллером `CleanAddressController`
- Помимо имеющихся сервисов был реализован сервис `ICleanAddressService`, предоставляющий метод `GetCleanAddressAsync` для получения ответа от DaData
- Automapper используется для мапинга объекта ответа сервиса `AddressResponse` (содержащего больше полей, чем передаваемая пользователю модель) в `AddressModel`, реализиующий описанную выше модель ответа пользователю.
- Для хранения информации, необходимой для доступа к API DaData, используется подход IOption.`Token`, `Secret` и тд хранятся в `appsettings.json`. Доступ к данным в классах программы производится через DI интерфейс `IOptions`.
- Реализована работа с `HttpClient` через `IHttpClientFactory`, для чего создан класс `CleanAddressClient`, реализующий упомянутый выше интерфейс `ICleanAddressService`.
- Обработка исключений основана на классе  `ExceptionHandlingMiddleware`, реализующем интерфейс `IMiddleware`. Для обработки некоторых исключений введены пользовательские классы.
- Настроено логирование в консоль и файл через Serilog
- Была установлена CROS политика, не ограничивающая междоменные запросы, так как детальных требований не было.
- Подключен Swagger

