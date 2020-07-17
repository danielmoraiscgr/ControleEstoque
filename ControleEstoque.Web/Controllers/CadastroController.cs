using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEtoque.Web.Controllers
{
    public class CadastroController : Controller
    {
        private static List<GrupoProdutoModel> _listaGrupoProdutos = new List<GrupoProdutoModel>()
        {
         new GrupoProdutoModel() { Id = 1, Nome= "Livros", Ativo=true  },
         new GrupoProdutoModel() { Id = 2, Nome = "Mouses", Ativo = true  },
         new GrupoProdutoModel() { Id = 3, Nome = "Montirores", Ativo = false  }
        };

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarGrupoProduto(int id)
        {
            return Json(_listaGrupoProdutos.Find(x => x.Id == id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarGrupoProduto(GrupoProdutoModel model)
        {
            var registroDB = _listaGrupoProdutos.Find(x => x.Id == model.Id);

            if (registroDB == null) // Inclusão
            {
                registroDB = model;
                registroDB.Id = _listaGrupoProdutos.Max(x => x.Id) + 1;
                _listaGrupoProdutos.Add(registroDB);
            }
            else // Alteracao
            {
                registroDB.Nome = model.Nome;
                registroDB.Ativo = model.Ativo; 
            }

            return Json(registroDB);

        }


        [HttpPost]
        [Authorize]
        public ActionResult ExcluirGrupoProduto(int id)
        {
            var ret = false; 
            var registroDB = _listaGrupoProdutos.Find(x => x.Id == id); 

            if (registroDB != null)
            {
                _listaGrupoProdutos.Remove(registroDB);
                ret = true; 
            }

            return Json(ret);
        }


        [Authorize]
        public ActionResult GrupoProduto()
        {
            return View(_listaGrupoProdutos);
        }

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

        [Authorize]
        public ActionResult Usuario()
        {
            return View();
        }
    }
}