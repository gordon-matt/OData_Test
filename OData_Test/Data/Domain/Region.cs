using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OData_Test.Data.Domain
{
    public class Region : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public RegionType RegionType { get; set; }

        public string CountryCode { get; set; }

        public bool HasStates { get; set; }

        public string StateCode { get; set; }

        public int? ParentId { get; set; }

        public short? Order { get; set; }

        public virtual Region Parent { get; set; }

        public virtual ICollection<Region> Children { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public enum RegionType : byte
    {
        Other = 0,
        Continent = 1,
        Country = 2,
        State = 3,
        City = 4,
    }

    public class RegionMap : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("Mantle_Regions");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            builder.Property(m => m.RegionType).IsRequired();
            builder.Property(m => m.CountryCode).HasMaxLength(2).IsUnicode(false);//.IsFixedLength();
            builder.Property(m => m.HasStates).IsRequired();
            builder.Property(m => m.StateCode).HasMaxLength(10).IsUnicode(false);
            builder.HasOne(p => p.Parent).WithMany(p => p.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}