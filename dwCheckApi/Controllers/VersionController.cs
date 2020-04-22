using Microsoft.AspNetCore.Mvc;
using dwCheckApi.Helpers;

namespace dwCheckApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class VersionController : BaseController
    {
        /// <summary>
        /// Gets the semver formatted version number for the application
        /// </summary>
        /// <returns>
        /// A string representing the semver formatted version number for the application
        /// </returns>
        [HttpGet]
        public string Get()
        {
            return CommonHelpers.GetVersionNumber();
        }
    }
}