using OData_Test.Data;
using OData_Test.Data.Domain;

namespace OData_Test.Controllers.OData
{
    public class MessageTemplateApiController : GenericODataController<MessageTemplate, int>
    {
        public MessageTemplateApiController(
            IRepository<MessageTemplate> repository)
            : base(repository)
        {
        }

        protected override int GetId(MessageTemplate entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(MessageTemplate entity)
        {
        }
    }
}