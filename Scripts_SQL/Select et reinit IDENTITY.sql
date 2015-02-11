select * from Personne
select * from StatutHistorique
select * from SuiviTelecharge
select * from Photo
select * from Style

delete Photo
delete StatutHistorique
delete SuiviTelecharge
delete Personne

DBCC CHECKIDENT ('Photo',NORESEED)
DBCC CHECKIDENT ('StatutHistorique',NORESEED)
DBCC CHECKIDENT ('SuiviTelecharge',NORESEED)
DBCC CHECKIDENT ('Personne',NORESEED)

/************************************************************/

select * from UserProfile
select * from webpages_Roles
select * from webpages_UsersInRoles