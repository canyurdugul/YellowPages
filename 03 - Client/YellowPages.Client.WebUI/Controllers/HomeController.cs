using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Client.WebUI.Models;

namespace YellowPages.Client.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Operator([FromBody] OperatorModel model)
        {
            var restClient = new RestClient(model.Url);
            var restRequest = new RestRequest();
            object result = (object)null;
            if (model.RequestType == "Post")
            {
                restRequest.Method = Method.POST;
                restRequest.RequestFormat = DataFormat.Json;
                result = GetResult<object>(restClient, restRequest, model.Data);
            }
            else if (model.RequestType == "Get")
            {
                restRequest.Method = Method.GET;
                result = GetResult<object>(restClient, restRequest);
            }
            else if (model.RequestType == "Delete")
            {
                restRequest.Method = Method.DELETE;
                result = GetResult<object>(restClient, restRequest);
            }
            else if(model.RequestType == "Download")
            {
                restRequest.Method = Method.GET;
                var file = GetResult<byte[]>(restClient, restRequest);
                return new JsonResult(file);
            }
            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private string GetResult<T>(RestClient client, RestRequest request, object obj = null, Dictionary<string, string> headers = null)
        {
            if (headers != null) //header varsa requeste headerları ekle
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (obj != null) //post,put,delete gibi işlemler için servise gönderilecek nesne varsa requeste ekle
            {
                request.AddJsonBody(obj);
            }
            //client üzerinden requesti servise yolla ve
            var response = client.Execute(request);

            return response.Content;

        }
    }
}
