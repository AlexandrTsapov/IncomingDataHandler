# Обработчик событий приходящих со случайной меткой времени
Извне приходят объекты, называющиеся событиями, имеющие Id, метку времени, некие полезные данные.
Время события случайно, но всегда меньше реального времени и отстаёт он него не больше, чем за T.
Нужно написать класс, получающий на вход поток событий, на выход отправляющий их на обработку с задержкой, но упорядочено по метке времени.
Также нужно написать две “болванки”, первая для генерации этих событий, вторая для имитации обработки.
Обе должны фиксировать события, либо писать их в лог-файл, либо в таблицу в БД.
То есть результатом будет два списка, первый – неупорядоченный, в том порядке, в котором сгенерировался, второй – упорядоченный.
