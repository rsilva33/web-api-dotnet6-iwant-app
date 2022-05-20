using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    public static IResult Action(int? page, int? rows, IConfiguration configuration)
    {
        var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);

        var query =
            @"SELECT EMAIL, CLAIMVALUE AS NAME
            FROM ASPNETUSERS U 
            INNER JOIN ASPNETUSERCLAIMS C
            ON U.ID = C.USERID AND CLAIMTYPE = 'NAME'
            ORDER BY NAME
            OFFSET (@page -1 ) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var employees = db.Query<EmployeeResponse>(
           query,
           new { page, rows }
        );

        return Results.Ok(employees);
    }
}
