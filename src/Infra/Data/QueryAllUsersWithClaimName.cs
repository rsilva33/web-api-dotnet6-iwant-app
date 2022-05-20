using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    public readonly IConfiguration Configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(Configuration["ConnectionString:IWantDb"]);

        var query =
            @"SELECT EMAIL, CLAIMVALUE AS NAME
            FROM ASPNETUSERS U 
            INNER JOIN ASPNETUSERCLAIMS C
            ON U.ID = C.USERID AND CLAIMTYPE = 'NAME'
            ORDER BY NAME
            OFFSET (@page -1 ) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        return db.Query<EmployeeResponse>(
           query,
           new { page, rows }
        );
    }
}
