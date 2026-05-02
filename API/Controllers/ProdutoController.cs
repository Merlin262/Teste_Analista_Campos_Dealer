using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TesteCamposDealer.DB;

namespace TesteCamposDealer.Controllers
{
    public class ProdutoController : ApiController
    {
        /// <summary>
        /// Recupera o Produto por Id..
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        [System.Web.Http.ActionName("GetById")]
        public Produto GetById(int idProduto)
        {
            Produto produtoRet = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                produtoRet = (from c in db.Produto
                          where c.idProduto == idProduto
                          select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return produtoRet;
        }

        /// <summary>
        /// Recupera todos os Produtos
        /// </summary>
        /// <returns></returns>
        public List<Produto> GetAll()
        {
            List<Produto> lstProdutoRet = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                lstProdutoRet = (from c in db.Produto
                             select c).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstProdutoRet;
        }


        /// <summary>
        /// Cadastra um Produto
        /// </summary>
        /// <param name="produtoDTO"></param>
        public bool Post([FromBody] Produto produtoDTO)
        {

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                db.Produto.InsertOnSubmit(produtoDTO);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Altera um Produto pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [System.Web.Http.ActionName("PutById")]
        public bool Put(int idProduto, [FromBody] Produto produtoDTO)
        {
            Produto prodRet = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                prodRet = (from c in db.Produto
                          where c.idProduto == idProduto
                          select c).FirstOrDefault();

                prodRet.dscProduto = produtoDTO.dscProduto;
                
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// Deleta um Produto pelo seu id
        /// </summary>
        /// <param name="idProduto"></param>
        [System.Web.Http.ActionName("DeleteById")]
        public bool Delete(int idProduto)
        {
            Produto prodRet = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                prodRet = (from c in db.Produto
                          where c.idProduto == idProduto
                          select c).FirstOrDefault();

                db.Produto.DeleteOnSubmit(prodRet);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
    }
}