using ApiChessMeet.DTO;
using ApiChessMeet.Mappers;
using DalChessMeet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiChessMeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ITournamentRepository _tournamentRepository;

        public RegistrationsController(IRegistrationRepository registrationRepository, ITournamentRepository tournamentRepository)
        {
            _registrationRepository = registrationRepository;
            _tournamentRepository = tournamentRepository;
        }

        [Authorize(Roles = "admin,player")]
        [HttpPost]

        public IActionResult Post(RegistrationFormDTO dto)
        {
            DalChessMeet.Entities.Registration registration = dto.FormToDalRegistrations();
            DalChessMeet.Entities.TournamentDetails tournamentDetails = _tournamentRepository.GetTournamentByGuid(registration.TournamentGuid);
            int userId = int.Parse(User.FindFirstValue("Id"));
            int userElo = int.Parse(User.FindFirstValue("Elo"));
            string userGender = User.FindFirstValue("Gender");
            DateTime userBirthdate = DateTime.Parse(User.FindFirstValue("Birthdate"));
            int userAge = GetAge(userBirthdate, tournamentDetails.EndRegistration);
            string userCategory = "";
            if (userAge < 18) { userCategory = "junior"; }
            if (userAge >= 18 && userAge < 60) { userCategory = "senior"; }
            if (userAge >= 60) { userCategory = "veteran"; }
            string[] categories = tournamentDetails.Categories.Split(',');
            int categoryCount = 0;
            foreach (string category in categories)
            {
                if (userCategory == category) { categoryCount++; }
            }
            if (tournamentDetails.Status != "waitingForPlayers")
            {
                return BadRequest("Trop tard, le tournoi a déjà commencé!");
            }
            if (tournamentDetails.EndRegistration < DateTime.Now)
            {
                return BadRequest("Trop tard, la date de fin des inscriptions est dépassée!");
            }
            if (_registrationRepository.CheckIfRegistered(registration.TournamentGuid, userId.ToString()))
            {
                return BadRequest("Vous êtes déjà inscrit(e) à ce tournoi!");
            }
            if (tournamentDetails.PlayersMin == tournamentDetails.PlayersMax)
            {
                return BadRequest("Désolé, le tournoi est complet!");
            }
            if (categoryCount == 0)
            {
                return BadRequest("Les catégories du tournoi ne correspondent pas à votre âge!");
            }
            if (tournamentDetails.EloMax > 0 && userElo > tournamentDetails.EloMax)
            {
                return BadRequest("L'Elo maximum autorisé pour participer est : " + tournamentDetails.EloMax);
            }
            if (tournamentDetails.EloMin > 0 && userElo < tournamentDetails.EloMin)
            {
                return BadRequest("L'Elo minimum pour participer est : " + tournamentDetails.EloMin);
            }
            if (tournamentDetails.WomenOnly && userGender == "male")
            {
                return BadRequest("Ce tournoi est interdit aux hommes!");
            }
            registration.PlayerId = userId;
            _registrationRepository.AddRegistration(registration);
            return NoContent();
        }

        private int GetAge(DateTime birthdate, DateTime endRegistrations)
        {
            int year_birthdate = birthdate.Year;
            int year_registration = endRegistrations.Year;
            return Math.Abs(year_registration - year_birthdate);
        }

        [Authorize(Roles = "admin,player")]
        [HttpDelete]
        public IActionResult Delete(RegistrationFormDTO dto)
        {
            DalChessMeet.Entities.Registration registration = dto.FormToDalRegistrations();
            int userId = int.Parse(User.FindFirstValue("Id"));
            registration.PlayerId = userId;
            DalChessMeet.Entities.TournamentDetails tournamentDetails = _tournamentRepository.GetTournamentByGuid(registration.TournamentGuid);
            if(tournamentDetails.Status == "inProgress")
            {
                BadRequest("Le tournoi est en cours.  Vous ne pouvez plus vous désinscrire!");
            }
            if(tournamentDetails.Status == "Closed")
            {
                BadRequest("Le tournoi est fini.");
            }
            if (!_registrationRepository.CheckIfRegistered(registration.TournamentGuid, userId.ToString()))
            {
                BadRequest("Vous n'êtes pas inscrit à ce tournoi!...");
            }
            _registrationRepository.DeleteRegistration(registration);
            
            return NoContent();
        }
    }
}
