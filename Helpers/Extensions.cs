using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Solstice.API.models;

namespace Solstice.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-control-Allow-Origin", "*");
        }
       
    }
}