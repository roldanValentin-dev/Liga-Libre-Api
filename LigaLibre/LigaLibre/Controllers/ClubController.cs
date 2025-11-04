using FluentValidation;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaLibre.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClubController(IClubService clubService, IValidator<CreateClubDto> createValidator, IValidator<UpdateClubDto> updateValidator) : ControllerBase
{

    [HttpGet]
    [Route("GetAllClubs")]
    //toma todos los clubes
    public async Task<IActionResult> GetAll()
    {
        var clubs = await clubService.GetAllClubsAsync();
        return Ok(clubs);
    }
    //toma un club por id
    [HttpGet]
    [Route("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var club = await clubService.GetClubByIdAsync(id);
        return club == null ? NotFound() : Ok(club);
    }
    //crea un club
    [HttpPost]
    [Route("CreateClub")]
    public async Task<IActionResult> CreateClub(CreateClubDto createClubDto)
    {
        var validationResult = await createValidator.ValidateAsync(createClubDto);

        return validationResult.IsValid ?
            StatusCode(201, await clubService.CreateClubAsync(createClubDto))
            : BadRequest(validationResult.Errors);
    }

    //actualiza un club
    [HttpPut]
    [Route("UpdateClub")]
    public async Task<IActionResult> UpdateClub(int id, UpdateClubDto updateClubDto)
    {
        var validationResult = await updateValidator.ValidateAsync(updateClubDto);
        
        return validationResult.IsValid ?
            Ok(await clubService.UpdateClubAsync(id, updateClubDto))
            : BadRequest(validationResult.Errors);
    }
    //elimina un club
    [HttpDelete]
    [Route("DeleteClub")]
    public async Task<IActionResult> DeleteClub(int id)
    {
        var result = await clubService.DeleteClubAsync(id);
        return Ok(result);
    }

}

