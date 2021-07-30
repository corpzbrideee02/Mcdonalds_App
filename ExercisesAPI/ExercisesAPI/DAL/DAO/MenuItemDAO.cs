using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExercisesAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
namespace ExercisesAPI.DAL.DAO
{
    public class MenuItemDAO
    {
        private AppDbContext _db;
        public MenuItemDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<MenuItem>> GetAllByCategory(int id)
        {
            return await _db.MenuItems.Where(item => item.Category.Id == id).ToListAsync();
        }
    }
}