using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ControleEtoque.Web.Controllers
{
    public class CadGrupoProdutoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20, 30}, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = 5;
            ViewBag.PaginaAtual = 1;

            var lista = GrupoProdutoModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = GrupoProdutoModel.RecuperarQuantidade(); 
                      
            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina)>0 ? 1:0;            
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            
            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult GrupoProdutoPagina(int pagina, int tamPag)
        {
            var lista = GrupoProdutoModel.RecuperarLista(pagina, tamPag);
                      

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // contra CSRF. Depois tem que gerar o token na view @Html.AntiForgeryToken()
                                   // Criar também uma function na tag <script>  function add_anti_forgery_token(data) ...
                                   // passar essa função nas requisições jquery
        public JsonResult RecuperarGrupoProduto(int id)
        {
            return Json(GrupoProdutoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarGrupoProduto(GrupoProdutoModel model)
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
        public JsonResult ExcluirGrupoProduto(int id)
        {
             return Json(GrupoProdutoModel.ExcluirPeloId(id));
        }

    }
}