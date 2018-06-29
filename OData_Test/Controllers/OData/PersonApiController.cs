using OData_Test.Data.Domain;
using OData_Test.Services;

namespace OData_Test.Controllers.OData
{
    public class PersonApiController : GenericODataController<Person, int>
    {
        public PersonApiController(IPersonService service)
            : base(service)
        {
        }

        protected override int GetId(Person entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Person entity)
        {
        }
    }
}