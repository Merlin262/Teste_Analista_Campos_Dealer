using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TesteCamposDealer.DB;

namespace TesteCamposDealer.Controllers
{
    public class ClienteController : ApiController
    {
        /// <summary>
        /// Recupera o Cliente por Id..
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [System.Web.Http.ActionName("GetById")]
        public Cliente GetById(int idCliente)
        {
            Cliente cliret = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                cliret = (from c in db.Cliente
                          where c.idCliente == idCliente
                          select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cliret;
        }

        /// <summary>
        /// Recupera todos os clientes
        /// </summary>
        /// <returns></returns>
        public List<Cliente> GetAll()
        {
            List<Cliente> lstCliRet = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                lstCliRet = (from c in db.Cliente
                          select c).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstCliRet;
        }


        /// <summary>
        /// Cadastra um Cliente
        /// </summary>
        /// <param name="cliente"></param>
        public bool Post([FromBody] Cliente clienteDTO)
        {

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                db.Cliente.InsertOnSubmit(clienteDTO);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Altera um Cliente pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [System.Web.Http.ActionName("PutById")]
        public bool Put(int idCliente, [FromBody] Cliente clienteDTO)
        {
            Cliente cliret = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                cliret = (from c in db.Cliente
                          where c.idCliente == idCliente
                          select c).FirstOrDefault();

                cliret.endereco = clienteDTO.endereco;
                cliret.nomeCliente = clienteDTO.nomeCliente;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// Deleta um cliente pelo seu id
        /// </summary>
        /// <param name="idCliente"></param>
        [System.Web.Http.ActionName("DeleteById")]
        public bool Delete(int idCliente)
        {
            Cliente cliret = null;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
            db.DeferredLoadingEnabled = false;

            try
            {
                cliret = (from c in db.Cliente
                          where c.idCliente == idCliente
                          select c).FirstOrDefault();

                db.Cliente.DeleteOnSubmit(cliret);
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
