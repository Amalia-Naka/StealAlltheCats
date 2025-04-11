using StealAllTheCats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealAllTheCats.Application.Interfaces
{
    public interface ICatService
    {
        Task<List<Cat>> FetchCats();

        Task<Cat> GetCatById(int id);

        Task<List<Cat>> GetCatsByPaging(int page, int pageSize);
        Task<List<Cat>> GetCatsByTagPaging(int page, int pageSize, string tagName);
    }
}
