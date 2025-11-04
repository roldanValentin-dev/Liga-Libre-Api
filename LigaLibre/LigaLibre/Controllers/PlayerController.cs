using FluentValidation;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LigaLibre.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController(IPlayerService playerService, IValidator<CreatePlayerDto> createValidator, IValidator<UpdatePlayerDto> updateValidator) : ControllerBase
{
    [HttpGet]
    [Route("GetAllPlayers")]
    public async Task<IActionResult> GetAllPlayers()
    {
        var players = await playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet]
    [Route("GetPlayersByClub")]
    public async Task<IActionResult> GetPlayersByClub(int clubId)
    {
        var players = await playerService.GetPlayerByClubAsync(clubId);
        return Ok(players);
    }


    [HttpGet]
    [Route("GetPlayerById")]
    public async Task<IActionResult> GetById(int id)
    {
        var player = await playerService.GetPlayerByIdAsync(id);
        return Ok(player);
    }

    [HttpPost]
    [Route("CreatePlayer")]
    public async Task<IActionResult> CreatePlayers(CreatePlayerDto createPlayerDto)
    {
        var validationResult = await createValidator.ValidateAsync(createPlayerDto);
        return validationResult.IsValid ?
            StatusCode(201, await playerService.CreatePlayerAsync(createPlayerDto))
            : BadRequest(validationResult.Errors);
    }


    [HttpPut]
    [Route("UpdatePlayer")]
    public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerDto updatePlayerDto)
    {
        var validationResult = await updateValidator.ValidateAsync(updatePlayerDto);
        
        return validationResult.IsValid ?
            Ok(await playerService.UpdatePlayerAsync(id, updatePlayerDto))
            : BadRequest(validationResult.Errors);
    }


    [HttpDelete]
    [Route("DeletePlayer")]
    public async Task<IActionResult> DeletePlayers(int id)
    {
        var player = await playerService.DeletePlayerAsync(id);
        return Ok(player);
    }
}

