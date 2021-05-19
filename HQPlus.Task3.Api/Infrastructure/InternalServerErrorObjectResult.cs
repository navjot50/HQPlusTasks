using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HQPlus.Task3.Api.Infrastructure {
    public sealed class InternalServerErrorObjectResult : ObjectResult {
        
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}