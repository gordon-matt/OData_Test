using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OData_Test.Data.Domain;
using OData_Test.Services;

namespace OData_Test.Controllers
{
    [Route("people")]
    public class PersonController : Controller
    {
        private readonly IPersonService service;

        public PersonController(IPersonService service)
        {
            this.service = service;
        }

        [Route("")]
        public IActionResult Index()
        {
            if (service.Count() == 0)
            {
                // Populate for testing purposes

                var people = new List<Person>();

                people.Add(new Person { FamilyName = "Jordan", GivenNames = "Michael", DateOfBirth = new DateTime(1963, 2, 17) });
                people.Add(new Person { FamilyName = "Johnson", GivenNames = "Dwayne", DateOfBirth = new DateTime(1972, 5, 2) });
                people.Add(new Person { FamilyName = "Froning", GivenNames = "Rich", DateOfBirth = new DateTime(1987, 7, 21) });

                service.Insert(people);
            }

            return View();
        }
    }
}