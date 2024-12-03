using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WishAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IResult> WishsAsync()
        {
            return Results.Json("jbio");
        }
    }
}
