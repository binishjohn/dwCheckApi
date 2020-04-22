using dwCheckApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace dwCheckApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string IncorrectUseOfApi()
        {
            return CommonHelpers.IncorrectUsageOfApi();
        }
    }
}