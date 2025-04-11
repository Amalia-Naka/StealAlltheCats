using Microsoft.AspNetCore.Mvc;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.DAL.Helpers;

namespace StealAllTheCats.Application.Commands
{
    public class FetchCatsCommand
    {
        private readonly IDbExtension _dbHandler;
        private readonly ICatService _repository;

        public FetchCatsCommand(IDbExtension dbHandler,
                                ICatService repository)
        {
            _dbHandler = dbHandler;
            _repository = repository;
        }

        public async Task<IActionResult> ExecuteAsync()
        {
           
            var result = await _repository.FetchCats();

            //Call to DB or Service Bus
            return new ObjectResult("200");
        }
    }
}
