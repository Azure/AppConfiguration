using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleAppConfigEventHub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace SimpleAppConfigEventHub.Controllers
{
    public class HomeController : Controller
    {
        private IEventHubService _eventHubService;

        public HomeController(IEventHubService eventHubService)
        {
            _eventHubService = eventHubService;
        }

        public IActionResult Index()
        {
            ViewData["Messages"] = _eventHubService?.Settings?.Messages;
            ViewData["FontColor"] = _eventHubService?.Settings?.FontColor;
            ViewData["FontSize"] = _eventHubService?.Settings?.FontSize;
            ViewData["BackgroundColor"] = _eventHubService?.Settings?.BackgroundColor;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
