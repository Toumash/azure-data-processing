﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataProcessing.Front.Models;

namespace DataProcessing.Front.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CosmosDbService<Report> Cosmos { get; }

        public HomeController(ILogger<HomeController> logger, CosmosDbService<Report> cosmos)
        {
            _logger = logger;
            Cosmos = cosmos;
        }

        public async Task<IActionResult> Index()
        {
            var items = await Cosmos.GetItemsAsync("SELECT TOP 2 * FROM c");
            return View(Tuple.Create<Report, Report>(items.First(), items.Skip(1).First()));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
