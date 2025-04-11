using StealAllTheCats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealAllTheCats.DAL.Helpers
{
    public interface IDbExtension
    {
        Task<List<Cat>> LoadAllCats();
        Task<bool> CatExists(string CatId);
        Task<Tag?> GetTagByName(string name);
        Task AddTag(Tag tag);
        Task AddCat(Cat cat);
        Task SaveChangesAsync();
        Task<Cat?> GetCatById(int id);
        Task<List<Cat>> GetCatByPaging(int page, int pageSize);
        Task<List<Cat>> GetCatsByTagPaging(int page, int pageSize, string tagName);


    }
}
