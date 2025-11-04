using FluentValidation;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaLibre.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MatchController(IMatchService matchService, IValidator<CreateMatchDto> createValidator, IValidator<UpdateMatchDto> updateValidator) : ControllerBase
{
    [HttpGet]
    [Route("GetAllMatches")]
    public async Task<IActionResult> GetAllMatches()
    {
        var matches = await matchService.GetAllMatchesAsync();
        return Ok(matches);
    }

    [HttpGet]
    [Route("GetMatchById")]
    public async Task<IActionResult> GetMatchById(int matchId)
    {
        var match = await matchService.GetMatchByIdAsync(matchId);
        return match != null ? Ok(match) : NotFound();
    }

    [HttpGet]
    [Route("GetMatchesByClub")]
    public async Task<IActionResult> GetMatchesByClub(int clubId)
    {
        var matches = await matchService.GetMatchesByClubAsync(clubId);
        return Ok(matches);
    }

    [HttpGet]
    [Route("GetMatchesByRound")]
    public async Task<IActionResult> GetMatchesByRound(int round)
    {
        var matches = await matchService.GetMatchesByRoundAsync(round);
        return Ok(matches);
    }

    [HttpGet]
    [Route("GetMatchesByStatus")]
    public async Task<IActionResult> GetMatchesByStatus(MatchStatusEnum status)
    {
        var matches = await matchService.GetMatchesByStatusAsync(status);
        return Ok(matches);
    }

    [HttpPost]
    [Route("CreateMatch")]
    public async Task<IActionResult> CreateMatch(CreateMatchDto createMatchDto)
    {
        var validationResult = await createValidator.ValidateAsync(createMatchDto);
        
        return validationResult.IsValid ?
            StatusCode(201, await matchService.CreateMatchAsync(createMatchDto))
            : BadRequest(validationResult.Errors);
    }

    [HttpPut]
    [Route("UpdateMatch")]
    public async Task<IActionResult> UpdateMatch(int id, UpdateMatchDto updateMatchDto)
    {
        var validationResult = await updateValidator.ValidateAsync(updateMatchDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var match = await matchService.UpdateMatchAsync(id, updateMatchDto);
        return match != null ? Ok(match) : NotFound();
    }

    [HttpDelete]
    [Route("DeleteMatch")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        var result = await matchService.DeleteMatchAsync(id);
        return Ok(result);
    }
}
