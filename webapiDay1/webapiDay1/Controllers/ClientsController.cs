using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using webapiDay1.Models;

namespace webapiDay1.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
        }
        // GET: api/Clients
        [Route("")]
        [ResponseType(typeof(IQueryable<Client>))]
        public IHttpActionResult GetClient()
        {
            if (!Request.IsLocal())
            {

            }
            return Ok(db.Client);
        }

        // GET: api/Clients/5
        [Route("{id:int}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // GET: api/Clients/5
        [Route("{id:int}/orders")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetClientOrders(int id)
        {
            Order client = db.Order.Where(w=>w.ClientId == id ).FirstOrDefault();
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [Route("{id:int}/orders/{*date:datetime}")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetClientOrdersByDate(int id, DateTime date)
        {
            var bDate = date.Date;
            var eDate = date.AddDays(1);
            Order client = db.Order.Where(w => w.OrderDate > bDate && w.OrderDate <eDate).FirstOrDefault();
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }


        [Route("{id:int}/orders/top10")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetClientOrdersByTop10(int id)
        {
        
            var client = db.Order.Where(w => w.ClientId == id).OrderByDescending(w=>w.OrderDate).Take(10).ToList();
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}