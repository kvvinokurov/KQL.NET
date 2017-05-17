# KQL.Geq(String, DateTime, Boolean) - метод
 

Возвращает результаты со значением свойства, большим или равным значению, указанному в ограничении свойства.

**Пространство имён:**&nbsp;<a href="3C471DD0">KQL.NET</a><br />**Сборка:**&nbsp;KQL.NET (в KQL.NET.dll) Версия: 1.0.0.0 (1.0.0.0)

## Синтаксис

**C#**<br />
``` C#
public static string Geq(
	string propertyName,
	DateTime value,
	bool excludeTime = false
)
```

**VB**<br />
``` VB
Public Shared Function Geq ( 
	propertyName As String,
	value As DateTime,
	Optional excludeTime As Boolean = false
) As String
```


#### Параметры
&nbsp;<dl><dt>propertyName</dt><dd>Тип:&nbsp;<a href="http://msdn2.microsoft.com/ru-ru/library/s1wwdcbf" target="_blank">System.String</a><br />Название свойства</dd><dt>value</dt><dd>Тип:&nbsp;<a href="http://msdn2.microsoft.com/ru-ru/library/03ybds8y" target="_blank">System.DateTime</a><br />Значение</dd><dt>excludeTime (Optional)</dt><dd>Тип:&nbsp;<a href="http://msdn2.microsoft.com/ru-ru/library/a28wyd50" target="_blank">System.Boolean</a><br />Исключить время</dd></dl>

#### Возвращаемое значение
Тип:&nbsp;<a href="http://msdn2.microsoft.com/ru-ru/library/s1wwdcbf" target="_blank">String</a><br />Выражение

## См. также


#### Ссылки
<a href="A04103EA">KQL - Класс</a><br /><a href="34B520E4">Geq - перегрузка</a><br /><a href="3C471DD0">KQL.NET - пространство имён</a><br />