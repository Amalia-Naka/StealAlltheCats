using Microsoft.EntityFrameworkCore;
using Serilog;
using StealAllTheCats.DAL.Data;
using StealAllTheCats.DAL.Models;

namespace StealAllTheCats.DAL.Helpers
{
    public class DbExtension : IDbExtension
    {
        private readonly DataContext _dbContext;

        public DbExtension(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Cats

        //load all cats with temperaments included
        public async Task<List<Cat>> LoadAllCats()
        {
            List<Cat> cats;
            try
            {
                cats = await _dbContext.Cats.Include(c => c.CatTags).ThenInclude(ct => ct.Tag).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking for existing record in database.");
                cats = null;
            }
            
            return cats;
        }

        public async Task<bool> CatExists(string CatId) 
        {
            //check id is null or empty
            if (string.IsNullOrWhiteSpace(CatId)) 
            {
                Log.Warning("Th CatId is null or empty");
                return false;
            }
            bool exists;
            try
            {
                exists = await _dbContext.Cats.AnyAsync(c => c.CatId == CatId);
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Error checking if cat exists for ID: {CatId}", CatId);
                return false;
            }
            return exists;
        }

        
        public async Task<Cat?> GetCatById(int id)
        {
            if (id <= 0)
            {
                Log.Warning("The Id given is Invalid: {Id}",id);
                return null;
            }
            Cat? cat;
            try
            {
                cat = await _dbContext.Cats.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking for existing CatId: {Id} in database.", id);
                return null;

            }
            return cat;
        }
        #endregion

        public async Task AddCat(Cat cat) 
        {

            if (cat ==null) 
            {
                Log.Warning("Error cat object is null");
            }
            try
            {
                await _dbContext.Cats.AddAsync(cat);
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Error adding new cat with CatId: {CatId}", cat.CatId);
            }
        }
        //fetches list of cat usong pagination
        public async Task<List<Cat>> GetCatByPaging(int page, int pageSize)
        {
            List<Cat> cats = new List<Cat>();
            try
            {
            //fetch cats with their temperamnets 
             cats = await _dbContext.Cats
            .Include(c => c.CatTags)
            .ThenInclude(ct => ct.Tag) //for each catTag brings the tag
            .Skip((page - 1) * pageSize) //skips entries from previous pages
            .Take(pageSize) //takes as many records as pageSize
            .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading cats with paging from database.");
            }

            return cats;
        }

        public async Task<List<Cat>> GetCatsByTagPaging(int page, int pageSize, string tagName) 
        {
            List<Cat> cats = new List<Cat>();
            try
            {    //check if tag exists in db
                var tagExists = await _dbContext.Tags.AnyAsync(t => t.Name.ToLower() == tagName.ToLower());
                if (!tagExists)
                {
                    Log.Warning("Tag not found: {TagName}", tagName);
                    throw new ArgumentException($"Tag '{tagName}' does not exist.");
                } 

                //Take cats with catTag where tag name is string given
                cats = await _dbContext.Cats.Where(c => c.CatTags.Any(ct => ct.Tag.Name.ToLower() == tagName.ToLower()))
                    .Include(c => c.CatTags)
                    .ThenInclude(ct => ct.Tag)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Error loading the cats with TagName: {TagName}", tagName);
            }
            return cats;

        }


        #region Tags
        public async Task AddTag(Tag tag) 
        {
            if (tag == null)
            {
                Log.Warning("Attempted to add null tag");
                return;
            }

            try
            {
                await _dbContext.Tags.AddAsync(tag);
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Error adding new tag: {TagName}", tag.Name);
            }

        }

        public async Task<Tag?> GetTagByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.Warning("Tag name requested is null or empty");
                return null;
            }

            Tag? tag;

            try
            {
                tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking for existing Tag: {name} in database.", name);
                return null;
            }
            return tag;

        }
        #endregion

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving changes to the database.");
            }
        }


    }
}
