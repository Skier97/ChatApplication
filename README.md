<h1>ChatApplication</h1>
<b>ChatApplication</b> - решение, реализующее работу <s>Telegram</s> простого мессенджера. Как и в любом мессенджере содержит следующий основной функционал:<br>
<ul>
  <li>Регистрация/авторизация пользователя</li>
  <li>Просмотр текущих чатов с пользователями</li>
  <li>Создание нового чата</li>
</ul><br>
Разве что-то еще нужно? <s>Стикеры, голосовые сообщения, видеокружочки, отправление файлов...</s> Согласен, и так сойдет!💩<br>
Есть еще небольшие бонусы! Во-первых, есть функционал, который отображает непрочитанные сообщения (p.s. выделяет строку более темным цветом). Во-вторых, 
каждые 5 секунд отправляются запросы на обновление данных, что позволяет в случае активного диалога получать (+-) своевременно новые сообщения. Как раз этим функционалом
и становятся сообщения прочитанными после открытия их в диалоге. Также, при открытии диалога автоматически прокручиваются сообщения к последним.<br>
Состав текущего решения:<br>
<ul>
  <li>Backend часть - .NET Core проект, который включает в себя:<br>
    <ul>Swagger для удобства тестирования</ul>
    <ul>Использование паттерна CQRS</ul>
    <ul>Асинхронная отправка запросов в БД</ul>   
    <ul>Ведение логов в БД при возникновении ошибок во время исполнения запросов</ul>
  </li>
  <li>
    Скрипты на формирование таблиц в БД и заполнение их первичными данными
  </li>
  <li>
    Frontend часть - WPF проект, который включает в себя: <br>
    <ul>Хеширование пароля при авторизации/регистрации пользователя с помощью библиотеки <b>System.Security.Cryptography</b></ul>
    <ul>Обработка запросов с Backend</ul>
    <ul>Выдача информационных сообщений и окон при возникновении ошибок</ul>
  </li>
</ul><br>
Для установки текущего проекта необходимо сначала развернуть БД с таблицами, которые указаны в скриптах CreateTableLogDB.sql, CreateTablesChatDB.sql. После этого
достаточно запустить backend и frontend части.<br>
В ветке <b>docker</b> имеется доработка на основе текущей реализации ветки <b>master</b>.<br><br>
<h3>DOCKER</h3><br>
<b>Docker</b> — это программная платформа для разработки, доставки и запуска контейнерных приложений. Он позволяет создавать контейнеры, автоматизировать их запуск и развертывание,
управляет жизненным циклом. С помощью Docker можно запускать множество контейнеров на одной хост-машине.Каждый контейнер включает все необходимое для работы приложения: библиотеки, 
системные инструменты, код и среду исполнения. Благодаря Docker можно быстро развертывать и масштабировать приложения в любой среде и сохранять уверенность в том, что код будет работать.
(<i>Дальше я думаю можно погуглить более подробно, что это такое</i>)<br>
Чуть ниже я приложу ссылки, которые я считаю полезными для того, чтобы обеспечить разворачивание и запуска контейнеров, но отмечу важные моменты:<br>
<ul>
  <li>В Visual Studio и VS Code есть возможность автоматически генерировать Docker файл, чтобы не допустить самому ошибки при формировании инструкций</li>
  <li>В случае, если проект имеет обращение к БД, необходимо добавлять и Docker compose. Он обеспечивает взаимодействие двух слоев: самого Web API и SQL Server.</li>
  <li>Для успешного подключения  БД необходимо указать в этих инструкциях пользователя sa <i>(авторизация sql пользователя!)</i></li>
  <li>Т.к. в Docker не будет sa учетке видны те данные и БД, которые были созданы вне контейнеров, необходимо сделать миграцию БД <i>(в момент работы sql контейнера!)</i></li>
  <li>При запуске docker контейнера необходимо вместо сервера обычного прописывать тот, который создается при запуске Docker</li>
</ul><br>
<b>Важные ссылки по Docker:</b><br>
<p><a href="https://code-maze.com/mysql-aspnetcore-docker-compose/">Заполнение Docker Compose</a></p>
<p><a href="https://www.section.io/engineering-education/dockerizing-an-aspnet-core-web-api-app-and-sql-server/">Взаимодействие WebAPI и SQL в Docker Compose</a></p>
<b>Ключевые слова:</b> .NET Core, async/await, Swagger, WPF, CQRS, Docker, SQL Server, Logger, C#, sql 
