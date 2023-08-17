namespace ContRev.Backend;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// Refactor
// Do not use std ATtribute from  Microsoft.AspNetCore.Authorization anymore.
// If user is needed in the header in the future - check id claim here
// Later I  a refresh token might be nescessary (get it here?)


// [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
// public class AuthorizeAttribute : Attribute, IAuthorizationFilter
// {
//     public void OnAuthorization(AuthorizationFilterContext context)
//     {
//         // var user = (User)context.HttpContext.Items["User"];
//         // if (user == null)
//         // {
//         //     // not logged in
//         //     context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//         // }
//         context.
//     }
// }