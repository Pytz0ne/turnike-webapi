using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turnike_webapi.Rdbms;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace turnike_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly postgresContext _context;
        public HistoryController(postgresContext context) => _context = context;


        /// <summary>
        /// Geçmiş giriş çıkışlarını getirir
        /// </summary>
        /// <param name="userid">Kullanıcı id si eşit olanları filtreler</param>
        /// <param name="rfid">Kullanıcı rfid si içinde geçenleri filtreler</param>
        /// <param name="name">Kullanıcı adının içinde geçenleri filtreler</param>
        /// <param name="mincreated">Giriş tarihinin başlayacağı zaman</param>
        /// <param name="maxcreated">Giriş tarihinin biteceği zaman</param>
        /// <param name="minrevoked">Çıkış tarihinin başlayacağı zaman</param>
        /// <param name="maxrevoked">Çıkış tarihinin biteceği zaman</param>
        /// <returns></returns>
        [LightQuery(forcePagination: true, defaultPageSize: 5)]
        [HttpGet]
        public IQueryable<History> Get(
            [FromQuery] int? userid,
            [FromQuery] string rfid,
            [FromQuery] string name,
            [FromQuery] DateTime? mincreated,
            [FromQuery] DateTime? maxcreated,
            [FromQuery] DateTime? minrevoked,
            [FromQuery] DateTime? maxrevoked
            )
        {
            var source = _context.History.AsQueryable();
            if (userid.HasValue)
                source = source.Where(f => f.Id == userid.Value);
            else
            {
                if (!string.IsNullOrEmpty(rfid))
                    source = source.Where(f => f.User.Rfid.Contains(rfid.ToUpper()));
                if (!string.IsNullOrEmpty(name))
                    source = source.Where(f => f.User.Name.Contains(name.ToUpper()));
            }
            if (mincreated.HasValue)
                source = source.Where(f => f.Created >= mincreated.Value);
            if (maxcreated.HasValue)
                source = source.Where(f => f.Created <= maxcreated.Value);

            if (minrevoked.HasValue)
                source = source.Where(f => f.Revoked >= minrevoked.Value);
            if (maxrevoked.HasValue)
                source = source.Where(f => f.Revoked <= maxrevoked.Value);


            return source
                .Include(i => i.User);
        }

    }
}
