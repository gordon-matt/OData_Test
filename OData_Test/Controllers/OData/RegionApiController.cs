using OData_Test.Data.Domain;
using OData_Test.Services;

namespace OData_Test.Controllers.OData
{
    public class RegionApiController : GenericODataController<Region, int>
    {
        public RegionApiController(IRegionService service)
            : base(service)
        {
        }

        protected override int GetId(Region entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Region entity)
        {
        }
    }
}