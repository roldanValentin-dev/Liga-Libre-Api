
using LigaLibre.Application.DTOs;
using LigaLibre.Domain.Entities;
using Mapster;

namespace LigaLibre.Application.Mapping;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        //prueba de Mapster y mapeo
        var config = TypeAdapterConfig.GlobalSettings;

        //referee mapping
        config.NewConfig<Referee, RefereeDto>();
        config.NewConfig<CreateRefereeDto, Referee>()
            .Map(dest => dest.IsActive, src => true)
            .Map(dest => dest.CreatedAt,src => DateTime.Now);

        //player mapping
        config.NewConfig<Player, PlayerDto>();
        config.NewConfig<CreatePlayerDto, Player>()
            .Map(dest => dest.IsActive, src => true)
            .Map(dest => dest.CreatedAt, src => DateTime.Now);

        //club mapping
        config.NewConfig<Club, ClubDto>();
        config.NewConfig<CreateClubDto, Club>()
            .Map(dest => dest.CreatedAt, src => DateTime.Now);

        //match mapping
        config.NewConfig<Match, MatchDto>()
            .Map(dest => dest.HomeClubName , src => src.HomeClub != null ? src.HomeClub.Name : string.Empty )
            .Map(dest => dest.AwayClubName, src => src.AwayClub != null ? src.AwayClub.Name : string.Empty)
            .Map(dest => dest.RefereeName, src => src.Referee != null ? $"{src.Referee.FirstName} {src.Referee.LastName}" : string.Empty);
        config.NewConfig<CreateClubDto, Match>()
            .Map(dest => dest.CreatedAt, src => DateTime.Now);

        //statistics mapping
        config.NewConfig<Player, LeagueStatisticsDto.TopScorersDto>()
            .Map(dest => dest.PlayerName,src=> $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.ClubName , src => src.Club != null ? src.Club.Name : "Sin club");

        config.NewConfig<Player, LeagueStatisticsDto.TopAssistsDto>()
            .Map(dest => dest.PlayerName, src=> $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.ClubName, src => src.Club != null ? src.Club.Name : "Sin club");

        config.NewConfig<Match, LeagueStatisticsDto.RecentMatchDto>()
            .Map(dest => dest.HomeClub, src => src.HomeClub != null ? src.HomeClub.Name : "TBD")
            .Map(dest => dest.AwayClub, src => src.AwayClub != null ? src.AwayClub.Name : "TBD");

        config.NewConfig<Match, LeagueStatisticsDto.PositionStatsDto>();
          

        config.Compile();
        
    }
}
