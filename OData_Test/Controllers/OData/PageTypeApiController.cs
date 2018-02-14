using System;
using OData_Test.Data.Domain;
using OData_Test.Services;

namespace OData_Test.Controllers.OData
{
    public class PageTypeApiController : GenericODataController<PageType, Guid>
    {
        public PageTypeApiController(IPageTypeService service)
            : base(service)
        {
        }

        protected override Guid GetId(PageType entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(PageType entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}