using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turnike_webapi.Rdbms;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace turnike_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly postgresContext _context;
        private readonly UserController userController;
        public EventController(postgresContext context)
        {
            _context = context;
            userController = new UserController(_context);
        }

        /// <summary>
        /// Durum sorgulama
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        [HttpGet("status/{rfid}")]
        public async Task<ActionResult<string>> GetStatus(string rfid)
        {
            var qUser = _context.User.Where(f => f.Rfid == rfid);
            if (!await qUser.AnyAsync())
                return BadRequest("Rfid tanımsız!");


            var qUnrevoked = _context.History.Where(f => f.User.Rfid == rfid && f.Revoked == null);
            if (await qUnrevoked.AnyAsync())
                return Ok($"{(await qUser.FirstAsync()).Name} has inside");
            return Ok($"{(await qUser.FirstAsync()).Name} has outside");
        }

        /// <summary>
        /// Turnikeden içeri girerken
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        [HttpPost("enter")]
        public async Task<ActionResult<History>> PostEnter([FromForm, Required] string rfid)
        {
            var qUser = _context.User.Where(f => f.Rfid == rfid);
            if (!await qUser.AnyAsync())
                return BadRequest("Rfid tanımsız!");


            var qUnrevoked = _context.History.Where(f => f.User.Rfid == rfid && f.Revoked == null);
            if (await qUnrevoked.AnyAsync())
                return BadRequest("Önce çıkış yapılması lazım!");

            var value = new History { Created = DateTime.Now, UserId = (await qUser.FirstAsync()).Id };
            _context.History.Add(value);
            var res = await _context.SaveChangesAsync();

            if (res > 0)
                return Ok(value);
            return BadRequest("Kayıt gerçekleşemedi!");
        }

        /// <summary>
        /// Turnikeden dışarı çıkarken
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        [HttpPost("leave")]
        public async Task<ActionResult<History>> PostLeave([FromForm, Required] string rfid)
        {
            var qUser = _context.User.Where(f => f.Rfid == rfid);
            if (!await qUser.AnyAsync())
                return BadRequest("Rfid tanımsız!");

            var qUnrevoked = _context.History.Where(f => f.User.Rfid == rfid && f.Revoked == null);
            if (!(await qUnrevoked.AnyAsync()))
                return BadRequest("Çıkış yapabilmek için önce giriş yapılması lazım!");

            var session = await qUnrevoked.FirstAsync();
            session.Revoked = DateTime.Now;
            var res = await _context.SaveChangesAsync();

            if (res > 0)
                return Ok(session);
            return BadRequest("Çıkış işlemi gerçekleşemedi!");
        }
    }
}
