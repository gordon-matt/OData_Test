using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using OData_Test.Data;

namespace OData_Test.Controllers.OData
{
    public class PublicUserApiController : ODataController
    {
        private readonly ApplicationDbContext context;

        public PublicUserApiController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IEnumerable<PublicUserInfo> Get(ODataQueryOptions<PublicUserInfo> options)
        {
            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var query = context.Users
                .Select(x => new PublicUserInfo
                {
                    Id = x.Id,
                    UserName = x.UserName
                });

            var results = options.ApplyTo(query);
            return (results as IQueryable<PublicUserInfo>).ToHashSet();
        }

        public virtual async Task<PublicUserInfo> Get([FromODataUri] string key)
        {
            var entity = await context.Users.FindAsync(key);
            return new PublicUserInfo
            {
                Id = entity.Id,
                UserName = entity.UserName
            };
        }
    }

    public class PublicUserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}