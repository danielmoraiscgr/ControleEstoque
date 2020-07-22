using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ControleEtoque.Web.Controllers
{
    public class CadastroController : Controller
    {


        #region Usuarios

        private const string _senhaPadrao = "{$127;$188}";

        [Authorize]
        public ActionResult Usuario()
        {
            ViewBag.SenhaPadrao = _senhaPadrao;
            return View(UsuarioModel.RecuperarLista());
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // contra CSRF. Depois tem que gerar o token na view @Html.AntiForgeryToken()
                                   // Criar também uma function na tag <script>  function add_anti_forgery_token(data) ...
                                   // passar essa função nas requisições jquery
        public ActionResult RecuperarUsuario(int id)
        {
            return Json(UsuarioModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>(); // Listar os erros de validação
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)  // Se nao foi bem sucedida
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    if (model.Senha == _senhaPadrao)
                    {
                        model.Senha = "";
                    }

                    var id = model.Salvar();

                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }

                }
                catch (Exception ex)
                {

                    resultado = "ERRO";
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });  // criacao de objeto anônimo.  Variavel interna comeca com letra maiscula para seguir a nomenclatura do .Net
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPeloId(id));
        }

        #endregion

        #region Grupo de Produtos

        [Authorize]
        public ActionResult GrupoProduto()
        {
            return View(GrupoProdutoModel.RecuperarLista());
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // contra CSRF. Depois tem que gerar o token na view @Html.AntiForgeryToken()
                                   // Criar também uma function na tag <script>  function add_anti_forgery_token(data) ...
                                   // passar essa função nas requisições jquery
        public ActionResult RecuperarGrupoProduto(int id)
        {
            return Json(GrupoProdutoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarGrupoProduto(GrupoProdutoModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>(); // Listar os erros de validação
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)  // Se nao foi bem sucedida
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();               
            }
            else
            {
                try
                {
                    var id = model.Salvar(); 

                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                                      
                }
                catch (Exception ex)
                {

                    resultado = "ERRO";
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });  // criacao de objeto anônimo.  Variavel interna comeca com letra maiscula para seguir a nomenclatura do .Net
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirGrupoProduto(int id)
        {
             return Json(GrupoProdutoModel.ExcluirPeloId(id));
        }

        #endregion

        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }

        [Authorize]
        public ActionResult LocalProduto()
        {
            return View();
        }

        [Authorize]
        public ActionResult UnidadeMedida()
        {
            return View();
        }

        [Authorize]
        public ActionResult Produto()
        {
            return View();
        }

        [Authorize]
        public ActionResult Pais()
        {
            return View();
        }

        [Authorize]
        public ActionResult Estado()
        {
            return View();
        }

        [Authorize]
        public ActionResult Cidade()
        {
            return View();
        }

        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }

        [Authorize]
        public ActionResult PerfilUsuario()
        {
            return View();
        }

    }
}