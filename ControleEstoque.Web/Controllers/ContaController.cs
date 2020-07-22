using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ControleEtoque.Web.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous] // Tornar o metodo publico
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl; // quando o usuario fizer acesso a URL privada, vai constar aqui e vai ser redirecionado a tela de login e quando se logar vai voltar para a URL privada
            return View();
        }

        [HttpPost]
        [AllowAnonymous]        
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)  // validar se a entrada do usuario esta correto
            {
                return View(login);
            }

            var usuario = (UsuarioModel.ValidarUsuario(login.Usuario,login.Senha));

            if (usuario!=null)
            {
                FormsAuthentication.SetAuthCookie(usuario.Nome, login.LembrarMe);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login inválido. ");
            }
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home"); 
        }

    }
}