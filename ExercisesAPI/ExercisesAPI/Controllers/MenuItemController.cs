﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExercisesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        AppDbContext _db;
        public MenuItemController(AppDbContext context)
        {
            _db = context;
        }
        [Route("{catid}")]
        public async Task<ActionResult<List<MenuItem>>> Index(int catid)
        {
            MenuItemDAO dao = new MenuItemDAO(_db);
            List<MenuItem> itemsForCategory = await dao.GetAllByCategory(catid);
            return itemsForCategory;
        }
    }
}

