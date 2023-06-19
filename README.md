# SEHW
Проект представляет из себя 2 микросервиса - API авторизации\регистрации и CRUD заказов из блюд.

## Nuget пакеты для корректной работы
- Microsoft.AspNetCore.Authentication.JwtBearer v6.0.1
- Microsoft.EntityFrameworkCore v6.0.1
- Microsoft.EntityFrameworkCore.Design v6.0.1
- Microsoft.EntityFrameworkCore.Relational v6.0.1
- Microsoft.Extensions.Configuration.Json v6.0.0
- Microsoft.IdentityModel.Tokens v6.30.1
- Npsql.EntityFrameworkCore.PostgreSQL v6.0.1
- Swashbuckle.AspNetCore v6.0.1
- System.IdentityModel.Tokens.Jwt v6.30.1
- FluentValidation.AspNetCore v11.3.0

## Команды для скачивания всех вышеперечисленных пакетов
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 6.0.1
dotnet add package Microsoft.EntityFrameworkCore --version 6.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.1
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 6.0.1
dotnet add package Microsoft.Extensions.Configuration.Json --version 6.0.0
dotnet add package Microsoft.IdentityModel.Tokens --version 6.30.1
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 6.0.1
dotnet add package Swashbuckle.AspNetCore --version 6.0.1
dotnet add package System.IdentityModel.Tokens.Jwt --version 6.30.1
dotnet add package FluentValidation.AspNetCore --version 11.3.0
```

## Как запустить?
1. Склонировать репозиторий себе на ПК
2. Открыть в любой IDE, поддерживающей C# (`Rider`, `Visual Studio`)
3. Запустить `Docker Desktop`
4. При необходимости заменить порт `5432` на любой другой свободный в файлах `appsettings.json` и `docker-compose.yml`
5. В терминале IDE прописать команду `docker compose up -d`
6. Для подключения IDE к базе данных необходимо ввести данные из строки подключения (находится в `appsettings.json`)
7. Запустить 2 проекта: Auth, Shop
8. Наслаждаться :)

## Автор
Черкасский Виталий Александрович; студент ВШЭ ФКН ПИ 2 курса
