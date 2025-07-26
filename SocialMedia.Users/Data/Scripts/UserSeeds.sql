insert into [user].[User] (Id, FirstName, LastName, Username, Email, Password, GenderId, IsPrivate, IsActive, IsLocked, CreatedBy, CreatedOn, LastChangedBy, LastChangedOn)
values
(
	'00000000-0000-0000-0000-000000000001',
	'System',
	'Admin',
	'admin',
	'sysadmin@socialmedia.com',
	'$2a$12$ASR.eawVK67Rp6cXiul9zuYYYgmeosye4mLSEbqTj.lsunl8S63wi', --Pa$$w0rd
	0,
	1,
	1,
	0,
	'00000000-0000-0000-0000-000000000001',
	CURRENT_TIMESTAMP,
	'00000000-0000-0000-0000-000000000001',
	CURRENT_TIMESTAMP
);

insert into [user].[UserRole] (UserId, RoleId)
values ('00000000-0000-0000-0000-000000000001', 1);