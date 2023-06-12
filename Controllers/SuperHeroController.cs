using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Data;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
       private static List<SuperHero> heros = new List<SuperHero> {
                new SuperHero {Id=1, Name="Spiderman",FirstName="Peter", LastName="Parker",Place="New York"},
                new SuperHero {Id=2, Name="Superman",FirstName="Kent", LastName="Clark",Place="New York"},
            };

        private DataContext _dataContext;
        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpGet("getHero/{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if(hero == null) { return BadRequest("No Hero Found with the id."); }
            return Ok(hero);
        }

        [HttpPost("addHero")]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(int id, SuperHero req)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if(hero == null)
            {
                return BadRequest("Not found");
            }
            hero.Name = req.Name;
            hero.FirstName = req.FirstName;
            hero.LastName = req.LastName;
            hero.Place = req.Place;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Not found");
            }
            _dataContext.SuperHeroes.Remove(hero);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }
    }
}
