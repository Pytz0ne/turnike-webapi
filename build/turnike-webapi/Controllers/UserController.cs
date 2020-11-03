using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class UserController : ControllerBase
    {
        private readonly postgresContext _context;
        public UserController(postgresContext context) => _context = context;

        /// <summary>
        /// Tüm kullanıcıları getirir
        /// </summary>
        /// <returns></returns>
        [LightQuery(forcePagination: true, defaultPageSize: 5)]
        [HttpGet]
        public IQueryable<User> Get(
            [FromQuery] int? id,
            [FromQuery] string rfid,
            [FromQuery] string name
            )
        {
            var source = _context.User.AsQueryable();
            if (id.HasValue)
                source = source.Where(f => f.Id == id.Value);
            else
            {
                if (!string.IsNullOrEmpty(rfid))
                    source = source.Where(f => f.Rfid.Contains(rfid.ToUpper()));
                if (!string.IsNullOrEmpty(name))
                    source = source.Where(f => f.Name.Contains(name.ToUpper()));
            }

            return _context.User;
        }

        /// <summary>
        /// Rfid filtrelemesi uygular
        /// </summary>
        /// <param name="rfid">Rfid kart numarası</param>
        /// <returns></returns>
        [HttpGet("{rfid}")]
        public async Task<ActionResult<User>> Get(string rfid)
        {
            var result = _context.User.Where(f => f.Rfid == rfid);
            if (!await result.AnyAsync())
                return BadRequest("Herhangi bir kayıt bulunamadı!");
            return Ok(await result.FirstOrDefaultAsync());
        }

        /// <summary>
        /// Yeni kullanıcı kaydı
        /// </summary>
        /// <param name="rfid">Rfid kart numarası</param>
        /// <param name="name">Kart sahibinin adı</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromForm, Required] string rfid, [FromForm, Required] string name)
        {
            var value = new User { Name = name, Rfid = rfid };
            _context.User.Add(value);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
                return Ok(value);
            return BadRequest("Kayıt gerçekleşemedi!");
        }

        /// <summary>
        /// Kullanıcı bilgileri güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] User user)
        {
            var result = _context.User.Where(f => f.Id == id);
            if (user.Id != id || !await result.AnyAsync())
                return BadRequest("Herhangi bir kayıt bulunamadı!");

            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = _context.User.Where(f => f.Id == id);
            if (!await result.AnyAsync())
                return BadRequest("Herhangi bir kayıt bulunamadı!");

            _context.User.Remove(await result.FirstAsync());
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
