using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreController : ControllerBase
    {
        AppDbContext _db;
        public StoreController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet("{lat}/{lon}")]
        public async Task<ActionResult<List<Store>>> Index(float lat, float lon)
        {
            StoreDAO dao = new StoreDAO(_db);
            return await dao.GetThreeClosestStores(lat, lon);
        }
    }
}