using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestCrud.Models;
using System.Data.SqlClient;
using TestCrud.Connection;

namespace TestCrud.Controllers
{
    public class HomeController : Controller
    {
        //Message cods
        public const long SUCCESSFUL = 0;
        public const long ERROR_EXISTING_USER = 1;
        public const long ERROR_NON_EXISTENT_USER = -1;
        public const long ERROR_INCOMPLETE_DATA = 3;
        public const long ERROR_EXECUTION = -2;

        public const int Admin = 1;
        public const int Visitante = 2;



        private readonly ILogger<HomeController> _logger;
        DBConnection Connection = new DBConnection();
        UserConnection userConnection = new UserConnection();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(Users users)
        {
            users.listProfiles = userConnection.GetProfiles();
            return View(users);
        }

        public IActionResult Register(Users users)
        {
            users.listProfiles = userConnection.GetProfiles();

            return View(users);
        }

      
        [HttpPost]

        public ActionResult SignUp(Users user)
        {
            int ret = userConnection.Create(user);
            if (ret == 0)
            {
                user.Confirm = true;
            }
            else
            {
                user.Confirm = false;
                user.ErrorExists = true;
                user.msgError = GetMessage(ret);
            }

            return View("Register",user);
        }

        [HttpPost]
        public ActionResult SignIn(Users user)
        {
            var ret = userConnection.GetUserId(user);
            if (ret != ERROR_NON_EXISTENT_USER)
            {
                if (user.Profile == Admin)
                {
                    return RedirectToAction("Admin", "User", new { id = ret });
                }
                else
                {
                    return RedirectToAction("visitor", "User", new { id = ret });
                }
            }
            else
            {
                user.Confirm = false;
                user.ErrorExists = true;
                user.msgError = GetMessage(ret);
            }

            return RedirectToAction("Index",user);
        }

        //-----------------------------------------------------------------------------------------------------
        //Obtiene un mensaje en base al codigo que se envia
        //-----------------------------------------------------------------------------------------------------
        public string GetMessage(long cod)
        {
            Dictionary<long, string> message = new Dictionary<long, string>();

            message.Add(SUCCESSFUL, "Ejecución exitosa");
            message.Add(ERROR_EXISTING_USER, "Usuario existente");
            message.Add(ERROR_NON_EXISTENT_USER, "Usuario inexistente");
            message.Add(ERROR_INCOMPLETE_DATA, "Faltan datos para ejecutar la función");
            message.Add(ERROR_EXECUTION, "Error de Ejecución");

            return message[cod];

        } //fin getMessage


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
