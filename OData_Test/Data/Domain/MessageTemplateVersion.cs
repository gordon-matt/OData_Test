using System;

namespace OData_Test.Data.Domain
{
    public class MessageTemplateVersion : IEntity
    {
        public int Id { get; set; }

        public int MessageTemplateId { get; set; }

        public string CultureCode { get; set; }

        public string Subject { get; set; }

        public string Data { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime DateModifiedUtc { get; set; }

        public virtual MessageTemplate MessageTemplate { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}