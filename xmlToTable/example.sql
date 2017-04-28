use sz6_cus
go
declare @xml xml
set @xml=(select top 1 * from DecHead for xml path('DecHead'))

select * from dbo.XmlToTable(@xml,'DecHead')



--在Sql Server中执行这段代码可以开启CLR
exec sp_configure 'show advanced options', '1';
go
reconfigure;
go
exec sp_configure 'clr enabled', '1'
go
reconfigure;
exec sp_configure 'show advanced options', '0';
go