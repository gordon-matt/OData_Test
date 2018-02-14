using Microsoft.EntityFrameworkCore;

namespace OData_Test.Data
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}