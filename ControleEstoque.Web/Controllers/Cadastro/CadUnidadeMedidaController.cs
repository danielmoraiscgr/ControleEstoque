using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.Web.Models;

namespace ControleEstoque.Web.Controllers.Cadastro
{
    public class CadUnidadeMedidaController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20, 30 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = 5;
            ViewBag.PaginaAtual = 1;

            var lista = UnidadeMedidaModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = UnidadeMedidaModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UnidadeMedidaPagina(int pagina, int tamPag)
        {
            var lista = UnidadeMedidaModel.RecuperarLista(pagina, tamPag);


            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // contra CSRF. Depois tem que gerar o token na view @Html.AntiForgeryToken()
                                   // Criar também uma function na tag <script>  function add_anti_forgery_token(data) ...
                                   // passar essa função nas requisições jquery
        public JsonResult RecuperarUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarUnidadeMedida(UnidadeMedidaModel model)
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
        public JsonResult ExcluirUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.ExcluirPeloId(id));
        }

    }
}