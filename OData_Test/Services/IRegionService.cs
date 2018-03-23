using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OData_Test.Data;
using OData_Test.Data.Domain;

namespace OData_Test.Services
{
    public interface IRegionService : IGenericDataService<Region>
    {
        Region FindOne(int id, bool includeChildren = false, bool includeParent = false);

        IEnumerable<Region> GetContinents(bool includeCountries = false);

        IEnumerable<Region> GetSubRegions(int regionId, RegionType? regionType = null);

        IEnumerable<Region> GetSubRegions(int regionId, int pageIndex, int pageSize, out int total, RegionType? regionType = null);

        IEnumerable<Region> GetCountries();

        IEnumerable<Region> GetStates(int countryId, bool includeCities = false);
    }

    public class RegionService : GenericDataService<Region>, IRegionService
    {
        public RegionService(
            IRepository<Region> repository)
            : base(repository)
        {
        }

        #region IRegionService Members

        public Region FindOne(int id, bool includeChildren = false, bool includeParent = false)
        {
            using (var connection = OpenConnection())
            {
                var query = connection.Query();

                if (includeParent)
                {
                    query = query.Include(x => x.Parent);
                }
                if (includeChildren)
                {
                    query = query.Include(x => x.Children);
                }

                var region = query.First(x => x.Id == id);

                return region;
            }
        }

        public IEnumerable<Region> GetContinents(bool includeCountries = false)
        {
            ICollection<Region> continents = null;

            using (var connection = OpenConnection())
            {
                var query = connection.Query(x =>
                    x.RegionType == RegionType.Continent);

                if (includeCountries)
                {
                    query = query.Include(x => x.Children);
                }

                continents = query
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToList();
            }

            return continents;
        }

        public IEnumerable<Region> GetSubRegions(int regionId, RegionType? regionType = null)
        {
            ICollection<Region> subRegions = null;

            using (var connection = OpenConnection())
            {
                if (regionType.HasValue)
                {
                    subRegions = connection.Query()
                        .Include(x => x.Parent)
                        .Include(x => x.Children)
                        .Where(x => x.Parent.Id == regionId && x.RegionType == regionType)
                        .OrderBy(x => x.Order == null)
                        .ThenBy(x => x.Order)
                        .ThenBy(x => x.Name)
                        .ToHashSet();
                }
                else
                {
                    subRegions = connection.Query()
                       .Include(x => x.Parent)
                       .Include(x => x.Children)
                       .Where(x => x.Parent.Id == regionId)
                       .OrderBy(x => x.Order == null)
                       .ThenBy(x => x.Order)
                       .ThenBy(x => x.Name)
                       .ToHashSet();
                }
            }

            return subRegions;
        }

        public IEnumerable<Region> GetSubRegions(int regionId, int pageIndex, int pageSize, out int total, RegionType? regionType = null)
        {
            using (var connection = OpenConnection())
            {
                IQueryable<Region> query = connection.Query()
                    .Include(x => x.Parent)
                    .Include(x => x.Children);

                if (regionType.HasValue)
                {
                    query = query.Where(x => x.Parent.Id == regionId && x.RegionType == regionType);
                }
                else
                {
                    query = query.Where(x => x.Parent.Id == regionId);
                }

                query = query
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name);

                total = query.Count();

                var subRegions = query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToHashSet();

                return subRegions;
            }
        }

        public IEnumerable<Region> GetCountries()
        {
            using (var connection = OpenConnection())
            {
                var countries = connection.Query()
                    .Include(x => x.Parent)
                    .Include(x => x.Children)
                    .Where(x =>
                        x.RegionType == RegionType.Country)
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToHashSet();

                return countries;
            }
        }

        public IEnumerable<Region> GetStates(int countryId, bool includeCities = false)
        {
            ICollection<Region> states = null;

            using (var connection = OpenConnection())
            {
                if (includeCities)
                {
                    states = connection.Query()
                        .Include(x => x.Children)
                        .Where(x => x.ParentId == countryId && x.RegionType == RegionType.State)
                        .OrderBy(x => x.Order == null)
                        .ThenBy(x => x.Order)
                        .ThenBy(x => x.Name)
                        .ToHashSet();
                }
                else
                {
                    states = connection.Query(x => x.ParentId == countryId && x.RegionType == RegionType.State)
                        .OrderBy(x => x.Order == null)
                        .ThenBy(x => x.Order)
                        .ThenBy(x => x.Name)
                        .ToHashSet();
                }
            }

            return states;
        }

        #endregion IRegionService Members

        private void Localize(ICollection<Region> regions)
        {
            var regionIds = regions.Select(x => x.Id.ToString());

            string entityType = typeof(Region).FullName;
        }
    }
}