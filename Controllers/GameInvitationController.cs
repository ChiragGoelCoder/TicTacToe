using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class GameInvitationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}