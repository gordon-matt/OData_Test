using OData_Test.Data;
using OData_Test.Data.Domain;

namespace OData_Test.Services
{
    public interface IMessageTemplateVersionService : IGenericDataService<MessageTemplateVersion>
    {
    }

    public class MessageTemplateVersionService : GenericDataService<MessageTemplateVersion>, IMessageTemplateVersionService
    {
        public MessageTemplateVersionService(IRepository<MessageTemplateVersion> repository)
            : base(repository)
        {
        }
    }
}