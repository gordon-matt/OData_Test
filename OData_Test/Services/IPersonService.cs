using OData_Test.Data;
using OData_Test.Data.Domain;

namespace OData_Test.Services
{
    public interface IPersonService : IGenericDataService<Person>
    {
    }

    public class PersonService : GenericDataService<Person>, IPersonService
    {
        public PersonService(IRepository<Person> repository)
            : base(repository)
        {
        }
    }
}