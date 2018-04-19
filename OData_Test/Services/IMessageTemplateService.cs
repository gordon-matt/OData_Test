using OData_Test.Data;
using OData_Test.Data.Domain;

namespace OData_Test.Services
{
    public interface IMessageTemplateService : IGenericDataService<MessageTemplate>
    {
    }

    public class MessageTemplateService : GenericDataService<MessageTemplate>, IMessageTemplateService
    {
        public MessageTemplateService(IRepository<MessageTemplate> repository)
            : base(repository)
        {
        }
    }
}