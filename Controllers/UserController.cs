using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TestCrud.Connection;
using TestCrud.Models;

namespace TestCrud.Controllers
{
    public class UserController : Controller
    {
        //DB connection
        DBConnection Connection = new DBConnection();
        UserConnection userConnection = new UserConnection();

        //Message cods
        public const int SUCCESSFUL = 0;
        public const int ERROR_EXISTING_USER = -1;
        public const int ERROR_NON_EXISTENT_USER = 2;
        public const int ERROR_INCOMPLETE_DATA = 3;
        public const int ERROR_EXECUTION= -2;



        // GET: UsersController
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Admin()
        {
            Users user = new Users();
            user.UserLists = userConnection.GetUserList();

            return View(user);
        }

        public ActionResult Update(int code)
        {
            Users user = userConnection.GetUserById(code);
            user.listProfiles = userConnection.GetProfiles();
            return View(user);
        }
        public ActionResult ChangeState(int code)
        {
            Users user = new Users();
            var  ret = userConnection.ChangeState(code);
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
            user.UserLists = userConnection.GetUserList();
            return View("Admin", user);
        }

        public ActionResult Add(Users user)
        {
            user.listProfiles = userConnection.GetProfiles();
            return View(user);
        }

        public ActionResult Visitor()
        {
            return View();
        }

        public ActionResult ConfirmUpdate (Users user)
        {
            var ret = userConnection.UpdateUser(user);
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
            user.UserLists = userConnection.GetUserList();
            return View("Admin", user);
        }
        public ActionResult ConfirmAdd(Users user)
        {
            var ret = userConnection.Create(user);
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
            user.UserLists = userConnection.GetUserList();
            return View("Admin", user);
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

    }
}
