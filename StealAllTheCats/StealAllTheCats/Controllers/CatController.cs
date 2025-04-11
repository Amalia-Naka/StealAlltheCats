using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using StealAllTheCats.Application.Commands;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.DAL.Models;

namespace StealAllTheCats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        //fetches 25 cats and saves them in db 
        [HttpPost("fetch")]
        public async Task<ActionResult> FetchCats()
        {
            var cats = await _catService.FetchCats();
            if (cats.Count == 0)
            {
                return Ok(new
                {
                    Message = "No new cats were added.",
                    Count = 0
                });
            }
            return Ok(new
            {
                Message = "Cats fetched and stored successfully!",
                Count = cats.Count,
                Cats = cats
            });

        }

        //fetches a cat with a specific given id
        [HttpGet("{id}")]
        public async Task<ActionResult<Cat>> GetCatById(int id)
        {
            var cat = await _catService.GetCatById(id);
            if (cat == null)
            {
                return NotFound(new { Message = $"Cat was not found." });
            }
            return Ok(cat);
        }

        //fetches  a paged list of cats 
        [HttpGet("paged")]
        public async Task<ActionResult> GetCatsPaged([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            // set default if not provided by user 
            int actualPage = page ?? 1;
            int actualPageSize = pageSize ?? 10;

            var cats = await _catService.GetCatsByPaging(actualPage, actualPageSize);
            return Ok(new
            {
                Page = actualPage,
                PageSize = actualPageSize,
                Count = cats.Count,
                Cats = cats
            });
        }

        [HttpGet("tagg")]
        //fetches a paged list of cats but bases on a specefic temperament
        public async Task<ActionResult> GetCatsByTagPaging([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string tagName) 
        {
            int actualPage = page ?? 1;
            int actualPageSize = pageSize ?? 10;

            var cats = await _catService.GetCatsByTagPaging(actualPage, actualPageSize, tagName);

            return Ok(new
            {
                Page = actualPage,
                PageSize = actualPageSize,
                Count = cats.Count,
                Cats = cats
            });

        }
    }




}

