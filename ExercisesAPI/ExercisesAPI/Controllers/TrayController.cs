﻿using System;
using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using ExercisesAPI.APIHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExercisesAPI.Helpers;
using System.Collections.Generic;
namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrayController : ControllerBase
    {
        AppDbContext _ctx;
        public TrayController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(TrayHelper helper)
        {
            string retVal = "";
            try
            {
                UserDAO uDao = new UserDAO(_ctx);
                User trayOwner = await uDao.GetByEmail(helper.email);
                TrayDAO tDao = new TrayDAO(_ctx);
                int trayId = await tDao.AddTray(trayOwner.Id, helper.selections);
                if (trayId > 0)
                {
                    retVal = "Tray " + trayId + " saved!";
                }
                else
                {
                    retVal = "Tray not saved";
                }
            }
            catch (Exception ex)
            {
                retVal = "Tray not saved " + ex.Message;
            }
            return retVal;
        }

        [Route("{email}")]
        public async Task<ActionResult<List<Tray>>> List(string email)
        {
            List<Tray> trays = new List<Tray>();
            UserDAO uDao = new UserDAO(_ctx);
            User trayOwner = await uDao.GetByEmail(email);
            TrayDAO tDao = new TrayDAO(_ctx);
            trays = await tDao.GetAll(trayOwner.Id);
            return trays;
        }

        [Route("{trayid}/{email}")]
        public async Task<ActionResult<List<TrayDetailsHelper>>> GetTrayDetails(int trayid, string email)
        {
            TrayDAO dao = new TrayDAO(_ctx);
            return await dao.GetTrayDetails(trayid, email);
        }
    }
}