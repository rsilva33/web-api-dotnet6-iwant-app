namespace IWantApp.Endpoints.Products;

public class ProductGetId
{
    public static string Template => "/products/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var results = products.Where(p => p.Id == id).Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Active));
        return Results.Ok(results);
    }
}
