### Назначение

Утилита преобразует параметризованный SQL-запрос, созданный NHibernate для MS SQL Server (sp_executesql), в обычный SQL-запрос без параметров.

![SpExecSqlConverter_DCYmeUxtNw](https://user-images.githubusercontent.com/829589/94853235-275eb880-043c-11eb-8760-989f111a0b8c.png)

Например, такой запрос:

`exec sp_executesql N'/* Web Server : profile.keep-alive : requestId=3r8t48 */ select * from Sungero_System_Clients where Id = @clientId and (ActivityTimeout > @currentDate or CheckActivity = 0)',N'@currentDate datetime,@clientId uniqueidentifier',@currentDate='2019-04-29 12:27:07.760',@clientId='DCBE933C-FEF3-431F-BAA9-C2426F9F436E'`

преобразуется в такой:

```
select *
from Sungero_System_Clients
where Id = 'DCBE933C-FEF3-431F-BAA9-C2426F9F436E'
      and (ActivityTimeout > '2019-04-29 12:27:07.760'
           or CheckActivity = 0);
```

Также утилита форматирует исходный запрос, даже если не удалось его преобразовать. Поэтому её можно использовать для форматирования запросов.
