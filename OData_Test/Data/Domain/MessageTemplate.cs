using System;

namespace OData_Test.Data.Domain
{
    public class MessageTemplate : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Editor { get; set; }

        public Guid? OwnerId { get; set; }

        public bool Enabled { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}