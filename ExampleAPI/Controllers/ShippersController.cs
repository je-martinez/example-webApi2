using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using ExampleAPI.Database;
using ExampleAPI.Models;

namespace ExampleAPI.Controllers
{
    [RoutePrefix("api/shippers")]
    public class ShippersController : ApiController
    {

        [HttpGet]
        [Route("lista-shippers")]
        public async Task<IHttpActionResult> ListadoShippers()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                try
                {
                    var Listado = await context.Shippers.ToListAsync().ConfigureAwait(false);
                    return Ok(Listado);
                }
                catch (Exception)
                {
                    return BadRequest("ERROR FATAL");
                }
            }
        }

        [HttpGet]
        [Route("shipper/{id}")]
        public async Task<IHttpActionResult> ObtenerShipper(int id)
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                try
                {
                    var Shipper = await context.Shippers.Where(x => x.ShipperID == id).FirstOrDefaultAsync();
                    if(Shipper == null)
                    {
                        return NotFound();
                    }
                    return Ok(Shipper);
                }catch(Exception ex)
                {
                    return BadRequest("ERROR FATAL");
                }
            }
        }

        [HttpPost]
        [Route("guardar-shipper")]
        public async Task<IHttpActionResult> GuardarShipper(Shipper nuevo_shipper)
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if(nuevo_shipper != null)
                        {
                            Shippers Row = new Shippers() {
                                CompanyName = nuevo_shipper.CompanyName,
                                Phone = nuevo_shipper.Phone
                            };
                            context.Shippers.Add(Row);
                            await context.SaveChangesAsync();
                            transaction.Commit();
                            return Ok(Row);
                        }
                        else
                        {
                            return BadRequest("Objeto Vacio");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest("ERROR FATAL");
                    } 
                }
            }
        }

        [HttpPut]
        [Route("editar-shipper")]
        public async Task<IHttpActionResult> EditarShipper(Shipper nuevo_shipper)
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (nuevo_shipper != null)
                        {
                            var Row = await context.Shippers.Where(x => x.ShipperID == nuevo_shipper.ShipperId).FirstOrDefaultAsync();
                            if(Row == null)
                            {
                                return NotFound();
                            }
                            Row.CompanyName = nuevo_shipper.CompanyName;
                            Row.Phone = nuevo_shipper.Phone;      
                            await context.SaveChangesAsync();
                            transaction.Commit();
                            return Ok(Row);
                        }
                        else
                        {
                            return BadRequest("Objeto Vacio");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest("ERROR FATAL");
                    }
                }
            }
        }

    }
}
