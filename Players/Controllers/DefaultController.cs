﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Players.Context;
 

namespace Players.Controllers
{
    public class DefaultController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        //Creating a method to return Json data
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                //Prepare data to be returned using Linq as follows
                var result = from players in db.Players
                             select new
                             {
                                 players.Name,
                                 players.position,
                                 playerSkills = from PlayerSkills in db.PlayerSkills
                                                where PlayerSkills.playerId == players.Id
                                                select new { PlayerSkills.Id,PlayerSkills.skill,PlayerSkills.value, PlayerSkills.playerId}
                             };                         
                           
                return Ok(result);
            }
            catch (Exception)
            {
                //If any exception occurs Internal Server Error i.e. Status Code 500 will be returned
                return InternalServerError();
            }




        }

         
         
    }
}
 