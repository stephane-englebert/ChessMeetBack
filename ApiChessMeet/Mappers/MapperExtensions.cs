using ApiChessMeet.DTO;

namespace ApiChessMeet.Mappers
{
    public static class MapperExtensions
    {
        public static DalChessMeet.Entities.Member FormToDalMembers(this MemberFormDTO dto)
        {
            return new DalChessMeet.Entities.Member
            {
                Pseudo = dto.Pseudo,
                Email = dto.Email,
                Birthdate = dto.Birthdate,
                Gender = dto.Gender,
                Elo = dto.Elo
            };
        }

        public static DalChessMeet.Entities.Tournament FormToDalTournaments(this TournamentFormDTO dto)
        {
            return new DalChessMeet.Entities.Tournament{
                Name = dto.Name,
                Place = dto.Place,
                PlayersMin = dto.PlayersMin,
                PlayersMax = dto.PlayersMax,
                EloMin = dto.EloMin,
                EloMax = dto.EloMax,
                Categories = dto.Categories,
                WomenOnly = dto.WomenOnly,
                EndRegistration = dto.EndRegistration
            };
        }

        public static DalChessMeet.Entities.Registration FormToDalRegistrations(this RegistrationFormDTO dto)
        {
            return new DalChessMeet.Entities.Registration
            {
                Id = 0,
                PlayerId = 0,
                TournamentGuid = dto.TournamentGuid
            };
        }
    }
}
