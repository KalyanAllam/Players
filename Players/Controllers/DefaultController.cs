using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Players.Context;
using Players.Models;
//https://www.c-sharpcorner.com/article/creating-web-api-using-code-first-approach-in-entity-framework/
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



        [HttpGet]
        public IHttpActionResult Get(int id )
        {
            try
            {
                //Prepare data to be returned using Linq as follows
                var result = from players in db.Players  
                              where players.Id == id
                              select new
                             {
                                 players.Name,
                                 players.position,
                                 playerSkills = from PlayerSkills in db.PlayerSkills
                                                where PlayerSkills.playerId == players.Id
                                                select new { PlayerSkills.Id, PlayerSkills.skill, PlayerSkills.value, PlayerSkills.playerId }
                             };


                
                return Ok(result);
            }
            catch (Exception)
            {
                //If any exception occurs Internal Server Error i.e. Status Code 500 will be returned
                return InternalServerError();
            }




        }





        [HttpPost]
        public IHttpActionResult Post([FromBody] players player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                //insert new record into database.
                db.Players.Add(player);
                db.SaveChanges();
                foreach (var skill in player.playerSkills)
                {
                    //set the ID of each skill
                    skill.playerId = player.Id;
                    db.PlayerSkills.Add(skill);
                    db.SaveChanges();
                }
                return Ok(player);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] players player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                //find record based on playerId and update
                var updateplayer = db.Players.First(p => p.Id ==  id);
                updateplayer.Name = player.Name;
                updateplayer.position = player.position;
                db.SaveChanges();
              
                /*
               foreach (var playerSkill in player.playerSkills)
                {
                    //update playerSkill based on skillId
                    var currectSkill = db.PlayerSkills.First(s => s.playerId == playerSkill.Id);
                    currectSkill.skill = playerSkill.skill;
                    currectSkill.value = playerSkill.value;
                    db.SaveChanges();
                }
                */
                return Ok(player);
            }
        }


        [HttpDelete]
        public   IHttpActionResult Delete(int id)
        {

            /*
             * CREATE TABLE [dbo].[players] (
   
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
	 [Name]     NVARCHAR (MAX) NULL,
    [Position] NVARCHAR (MAX) NULL 
);

 ALTER TABLE [dbo].[players]
    ADD CONSTRAINT [PK_dbo.players] PRIMARY KEY CLUSTERED ([Id] ASC);


             * 
             */



            /* On Cascade delete
          
CREATE TABLE [dbo].[playerSkills] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [skill]      NVARCHAR (MAX) NULL,
    [value]      INT            NOT NULL,
    [playerId]   INT            NOT NULL,
    [players_Id] INT            NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_players_Id]
    ON [dbo].[playerSkills]([players_Id] ASC);


GO
ALTER TABLE [dbo].[playerSkills]
    ADD CONSTRAINT [PK_dbo.playerSkills] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[playerSkills]
    ADD CONSTRAINT [FK_dbo.playerSkills_dbo.players_players_Id] FOREIGN KEY ([players_Id]) REFERENCES [dbo].[players] ([Id]) ON DELETE CASCADE;


             * */

            var deleteplayer = db.Players.First(p => p.Id == id);
            db.Players.Remove(deleteplayer);
            db.SaveChanges();
            return Ok();
        }
    }
}
 