if not exists (select 1 from TaskInfo)
begin
	insert into Category(name)
	values ('Lab'),
	('Test'),
	('Other')


	insert into TaskInfo(name, timeStart, deadline, passed, idCategory)
	values ('Networks 6', '2022-04-28 19:00:00', '2022-5-05 19:00:00', 0, 1),
	('Networks 5', '2022-04-24 19:00:00', '2022-05-04 19:00:00', 0, 1),
	('Systems 4', '2022-04-25 19:00:00', '2022-05-14 19:00:00', 0, 1),
	('Systems 3', '2022-04-21 19:00:00', '2022-05-11 19:00:00', 1, 1),
	('Networks Test 5', '2022-04-25 11:40:00', '2022-05-01 11:40:00', 1, 2),
	('System Test 4', '2022-04-29 13:00:00', '2022-05-05 11:40:00', 0, 2),
	('ToDo list', '2022-04-19 18:00:00', '2022-05-03 17:00:00', 0, 3)

	insert into TaskInfo(name, timeStart, deadline, passed, idCategory)
	values ('Systems 1', '2022-02-24 10:00:00', null, 0, 2)
end