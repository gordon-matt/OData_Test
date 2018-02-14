using OData_Test.Data;
using OData_Test.Data.Domain;

namespace OData_Test.Services
{
    public interface IPageTypeService : IGenericDataService<PageType>
    {
    }

    public class PageTypeService : GenericDataService<PageType>, IPageTypeService
    {
        public PageTypeService(IRepository<PageType> repository)
            : base(repository)
        {
        }
    }
}