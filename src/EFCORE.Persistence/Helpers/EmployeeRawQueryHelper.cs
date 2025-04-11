
namespace EFCORE.Persistence.Helpers;

public static class EmployeeRawQueryHelper
{
    public static string EmployeesWithDepartmentQuery => @"create table #Temp
                                                        (
                                                            EmployeeId uniqueidentifier,
                                                            EmployeeName nvarchar(100),
                                                            DepartmentId uniqueidentifier,
                                                            DepartmentName nvarchar(100)
                                                        )
                                                        INSERT INTO #Temp
                                                        SELECT 
	                                                        e.Id,
	                                                        e.Name,
	                                                        d.Id,
	                                                        d.Name
                                                        FROM Employee e
                                                        INNER JOIN Departments d 
                                                        ON e.DepartmentId = d.Id
                                                        SELECT * FROM #Temp
                                                        ORDER BY EmployeeId
                                                        OFFSET (@PageSize * (@PageIndex - 1)) ROWS
                                                        FETCH NEXT @PageSize ROWS ONLY
                                                        SELECT COUNT(*) FROM #Temp";
    public static string EmployeeWithProjects => @"
                        CREATE TABLE #TempResult
                        (
                            EmployeeId uniqueidentifier,
                            EmployeeName nvarchar(100),
	                        DepartmentId uniqueidentifier,
	                        JoinedDate Date,
	                        ProjectsJson nvarchar(max)
                        )
                        CREATE TABLE #Temp
                        (
                            EmployeeId uniqueidentifier,
                            EmployeeName nvarchar(100),
	                        DepartmentId uniqueidentifier,
	                        JoinedDate Date,
                            ProjectId uniqueidentifier,
                            ProjectName nvarchar(100),
                            Enable BIT
                        )
                        INSERT INTO #Temp
                        SELECT 
	                        e.Id,
	                        e.Name,
	                        e.DepartmentId,
	                        e.JoinedDate,
	                        p.Id,
	                        p.Name,
	                        p.Enable
                        FROM Employee e 
                        LEFT JOIN 
                        (SELECT p.Id as Id, p.Name as Name, pe.Enable as Enable, pe.EmployeeId as EmployeeId
                        FROM Project p INNER JOIN ProjectEmployees pe 
                        ON p.Id = pe.ProjectId
                        ) p 
                        ON p.EmployeeId = e.Id

                        INSERT INTO #TempResult
                        SELECT DISTINCT
                            t.EmployeeId,
                            t.EmployeeName,
	                        t.DepartmentId,
	                        t.JoinedDate,
                            (
                                SELECT 
                                    t2.ProjectId, 
                                    t2.ProjectName, 
                                    t2.Enable
                                FROM #Temp t2
                                WHERE t2.EmployeeId = t.EmployeeId
                                FOR JSON PATH
                            ) AS ProjectsJson
                        FROM #Temp t

                        SELECT * FROM #TempResult t
                        ORDER BY t.EmployeeId
                        OFFSET (@PageSize * (@PageIndex - 1)) ROWS
                        FETCH NEXT @PageSize ROWS ONLY;

                        SELECT COUNT(*) FROM #TempResult

                            ";
    public static string EmployeeWithConditionSalaryAndJoinedDate => @"
                                            CREATE TABLE #Temp
                                            (
                                                EmployeeId uniqueidentifier,
                                                EmployeeName nvarchar(100),
                                                DepartmentId uniqueidentifier,
                                                JoinedDate Date,
                                                SalaryId uniqueidentifier,
                                                Amount decimal(18, 2)
                                            )
                                            INSERT INTO #Temp
                                            SELECT 
	                                            e.Id, 
	                                            e.Name, 
	                                            e.DepartmentId, 
	                                            e.JoinedDate, 
	                                            s.Id, 
	                                            s.Amount 
                                            FROM Employee e
                                            INNER JOIN Salary s
                                            ON e.Id = s.EmployeeId
                                            WHERE (@MinSalary IS NULL OR s.Amount > @MinSalary) 
                                            AND (@JoinedFromDate IS NULL OR e.JoinedDate >= @JoinedFromDate)

                                            SELECT * FROM #Temp
                                            ORDER BY EmployeeId
                                            OFFSET (@PageSize * (@PageIndex - 1)) ROWS
                                            FETCH NEXT @PageSize ROWS ONLY
                                            SELECT COUNT(*) FROM #Temp";
}
