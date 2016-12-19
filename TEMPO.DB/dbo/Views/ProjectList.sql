
CREATE view [ProjectList]

as

select 
	substring(cast(jobyear as char(4)), 3, 2) + '-' + 	
	cast(jobnum as varchar(10) ) + ' ' + cast([description] as varchar(30))
	
	as ProjectName, 
projectid, clientid, jobnumyear, jobnum, refjobnum, projecttypeid, [description] from project, jobyear
where project.jobnumyear = jobyear.jobyearid

