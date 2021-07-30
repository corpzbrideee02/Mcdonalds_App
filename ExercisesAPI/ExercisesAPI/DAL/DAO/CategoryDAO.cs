using ExercisesAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ExercisesAPI.DAL.DAO
{
    public class CategoryDAO
    {
        private AppDbContext _db;
        public CategoryDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Category>> GetAll()
        {
            return await _db.Categories.ToListAsync<Category>();
        }
    }
}