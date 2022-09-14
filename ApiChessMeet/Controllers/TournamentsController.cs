using ApiChessMeet.DTO;
using ApiChessMeet.Mappers;
using DalChessMeet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiChessMeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentsController(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_tournamentRepository.GetTournaments().Select(t => new TournamentDTO(t)));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post(TournamentFormDTO dto)
        {
            DalChessMeet.Entities.Tournament entity = dto.FormToDalTournaments();
            Guid g = Guid.NewGuid();
            while (_tournamentRepository.ExistGuid(g))
            {
                g = Guid.NewGuid();
            }
            entity.Guid = g.ToString();
            entity.Status = "waitingForPlayers";
            entity.CurrentRound = 0;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            string[] categories = entity.Categories.Split(',');
            foreach(string category in categories)
            {
                if(!Enum.IsDefined(typeof(DalChessMeet.Enums.EnumTournamentCategories), category))
                {
                    return BadRequest("Catégorie invalide!");
                }
            }
            if(entity.PlayersMin > entity.PlayersMax) { 
                return BadRequest("Le nombre de joueurs min doit être inférieur ou égal au nombre de joueurs max!"); 
            }
            if (entity.EloMin > entity.EloMax)
            {
                return BadRequest("L'Elo min doit être inférieur ou égal à l'Elo max!");
            }
            DateTime limitDate = DateTime.Now.AddDays(entity.PlayersMin);
            if (entity.EndRegistration <= limitDate)
            {
                return BadRequest("La date de fin des inscriptions doit être supérieure au " + limitDate.ToString("dd/MM") + "!");
            }
            _tournamentRepository.AddTournament(entity);
            return NoContent();
        }

        [HttpGet("{g}")]
        public IActionResult Get(string g)
        {
            DalChessMeet.Entities.TournamentDetails entity = _tournamentRepository.GetTournamentByGuid(g);
            if (entity.Guid == "")
            {
                return NotFound();
            }
            entity.PlayersDetails = _tournamentRepository.GetTournamentPlayersDetails(g);
            return Ok(new TournamentDetailsDTO(entity));
        }

        [Authorize(Roles ="admin")]
        [HttpDelete]
        public IActionResult Delete(Guid g)
        {
            _tournamentRepository.DeleteTournament(g);
            return NoContent();
        }
    }
}
