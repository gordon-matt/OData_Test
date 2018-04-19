using System;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OData_Test.Data.Domain;
using OData_Test.Services;

namespace OData_Test.Controllers.OData
{
    public class MessageTemplateVersionApiController : GenericODataController<MessageTemplateVersion, int>
    {
        private readonly IMessageTemplateService messageTemplateService;

        public MessageTemplateVersionApiController(
            IMessageTemplateVersionService service,
            IMessageTemplateService messageTemplateService)
            : base(service)
        {
            this.messageTemplateService = messageTemplateService;
        }

        protected override int GetId(MessageTemplateVersion entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(MessageTemplateVersion entity)
        {
        }

        [HttpGet]
        public IActionResult GetCurrentVersion([FromODataUri] int templateId, [FromODataUri] string cultureCode)
        {
            // Test OkObjectResult (issue #1369: https://github.com/OData/WebApi/issues/1369)
            return Ok(new MessageTemplateVersion
            {
                MessageTemplateId = templateId,
                CultureCode = cultureCode,
                Subject = "Name",
                DateCreatedUtc = DateTime.UtcNow,
                DateModifiedUtc = DateTime.UtcNow
            });

            //var currentVersion = ((IMessageTemplateVersionService)Service).FindOne(
            //    templateId,
            //    cultureCode);

            //if (currentVersion == null)
            //{
            //    var template = messageTemplateService.FindOne(templateId);

            //    var utcNow = DateTime.UtcNow;
            //    currentVersion = new MessageTemplateVersion
            //    {
            //        MessageTemplateId = templateId,
            //        CultureCode = cultureCode,
            //        Subject = template.Name,
            //        DateCreatedUtc = utcNow,
            //        DateModifiedUtc = utcNow
            //    };
            //    Service.Insert(currentVersion);
            //}
            ////else if (!string.IsNullOrEmpty(currentVersion.Data) && currentVersion.Data.Contains("gjs-assets"))
            ////{
            ////    if (currentVersion.Data.Contains("mjml"))
            ////    {
            ////        return BadRequest("Please open this template in GrapesJS.");
            ////    }
            ////    // Since we wish to edit in normal HTML editor this time (was GrapesJS before), then we need to extract just the HTML
            ////    var data = currentVersion.Data.JsonDeserialize<GrapesJsStorageData>();
            ////    currentVersion.Data = data.Html;
            ////}

            //return new JsonResult(JObject.FromObject(currentVersion)); // Works, but shouldn't need to use this.. Ok() was fine in MVC5
            ////return Ok(currentVersion); // Broken
        }
    }
}