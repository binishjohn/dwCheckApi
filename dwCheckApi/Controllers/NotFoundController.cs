using Microsoft.AspNetCore.Mvc;

namespace dwCheckApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class NotFoundController : BaseController
    {
        /// <summary>
        /// Used to get a string which represents all of the controller actions
        /// available for the API
        /// </summary>
        /// <returns>
        /// A string representing all of the controller actions available for the API 
        /// </returns>
        [HttpGet]
        public string Get()
        {
            return IncorrectUseOfApi();
        }
    }


}