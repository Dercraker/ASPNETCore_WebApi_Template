using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.Dto.Token;
using Template.Domain.Dto.User;
using Template.Domain.Entities;
using Template.Domain.ErrorCodes;
using Template.Domain.Settings;
using Template.EFCore;
using Template.EFCore.Migrations;
using Template.Platform.Interfaces;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Template.Domain.Models.HttpError;

namespace Template.API.Controllers.V1._0;

/// <summary>
/// Authentication and user management controller
/// </summary>

[Authorize]
[ApiController]
[Route("api/v1.0/[controller]")]
public class AuthController : ControllerBase
{
    #region Props
    private readonly TemplateContext _context;
    private readonly JWTSettings _jwtSettings;
    private readonly UserManager<UserApi> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IValidator<RegisterUserDto> _registerUserValidator;
    private readonly IValidator<LoginUserDto> _loginUserValidator;
    private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
    private readonly IUserPlatform _userPlatform;
    private readonly IMapper _mapper;
    #endregion

    #region CTOR
    public AuthController(TemplateContext context,
                          JWTSettings jwtSettings,
                          UserManager<UserApi> userManager,
                          RoleManager<IdentityRole<Guid>> roleManager,
                          IValidator<RegisterUserDto> registerUserValidator,
                          IUserPlatform userPlatform,
                          IValidator<LoginUserDto> loginUserValidator,
                          IMapper mapper,
                          IValidator<ForgotPasswordDto> forgotPasswordDto,
                          IValidator<ResetPasswordDto> resetPasswordValidator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _registerUserValidator = registerUserValidator ?? throw new ArgumentNullException(nameof(registerUserValidator));
        _userPlatform = userPlatform ?? throw new ArgumentNullException(nameof(_userPlatform));
        _loginUserValidator = loginUserValidator ?? throw new ArgumentNullException(nameof(loginUserValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _forgotPasswordValidator = forgotPasswordDto ?? throw new ArgumentNullException(nameof(forgotPasswordDto));
        _resetPasswordValidator = resetPasswordValidator ?? throw new ArgumentNullException(nameof(resetPasswordValidator));
    }
    #endregion

    /// <summary>
    /// Initialise les rôles et l'utilisateur Admin
    /// </summary>
    /// <response code="200 + Message"></response>
    [AllowAnonymous]
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult), Status201Created)]
    [HttpPost]
    [Route("Initialize")]
    public async Task<IActionResult> Initialize()
    {
        bool result = await DBInitializer.Initialize(_context, _userManager, _roleManager);
        string resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

        return Ok(resultMessage);
    }

    /// <summary>
    /// Register Route
    /// </summary>
    [AllowAnonymous]
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<UserDto>), Status201Created)]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        ValidationResult validationResult = _registerUserValidator.Validate(registerUserDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));


        bool userExists = await _userPlatform.ExistAsync(null, registerUserDto.UserName, registerUserDto.Email);
        if (userExists) return BadRequest(EUserErrorCodes.UserAlreadyExist);

        UserApi user = new UserApi
        {
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
            CreatedAt = DateTime.Now,
        };

        IdentityResult? result = await _userPlatform.CreateAsync(user, registerUserDto.Password);
        if (result.Succeeded)
        {
            await _userPlatform.AddRoleAsync(user, Roles.User);
        }
        else return BadRequest(result.Errors);

        return Created("", _mapper.Map<UserApi, UserDto>(user));
    }

    /// <summary>
    /// Account Validation Route
    /// </summary>
    /// <param name="token">Token de validation du compte</param>
    //[AllowAnonymous]
    //[HttpPost]
    //[Route("register/validation/{discordId}")]
    //public async Task<IActionResult> ValidationRegister([FromRoute] string discordId, [FromBody] ValidationRegistrationDTO dto)
    //{
    //    if (!ModelState.IsValid) return BadRequest("qqsdqsfsf"+ModelState);

    //    if (discordId != dto.discordId) return BadRequest("DiscordIds do not match");

    //    UserApi user = await userManager.FindByEmailAsync(dto.discordId);
    //    if (user == null) return BadRequest("L'utilisateur n'existe pas");

    //    IdentityResult registrationToken = await userManager.ConfirmEmailAsync(user, dto.token);

    //    if (registrationToken.Succeeded) await userManager.AddToRoleAsync(user, Roles.User);
    //    else return BadRequest("aaaaa" + registrationToken.Errors);


    //    userValidatedOnDbDTO userValidatedOnDbDTO = new() { userId = user.Id, discordId = user.Email };

    //    string url = $"{apiToBotSettings.baseURI}userValidatedOnDB/{user.Id}";

    //    string json = JsonSerializer.Serialize(userValidatedOnDbDTO);
    //    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

    //    HttpClient client = new();
    //    await client.PostAsync(url, data);

    //    return Ok();
    //}



    /// <summary>
    /// Permet de login un user dans la DB
    /// </summary>
    /// <param name="dto">Model de login d'un user</param>
    /// <response code="400 + Message"></response>
    /// <response code="401">Erreur de mdp ou id</response>
    /// <response code="200">Token + date d'expiration</response>
    [AllowAnonymous]
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<TokenDto>), Status200OK)]
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserDto loginUserDto)
    {
        ValidationResult validationResult = _loginUserValidator.Validate(loginUserDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        UserApi? user = await _userPlatform.GetByNameAsync(loginUserDto.Login);
        user ??= await _userPlatform.GetByEmailAsync(loginUserDto.Login);

        if (user is null || !await _userPlatform.CheckPasswordAsync(user, loginUserDto.Password))
            return Unauthorized();

        TokenDto token = await _userPlatform.LoginAsync(user);

        return token;
    }


    /// <summary>
    /// Test the validity of a token
    /// </summary>
    /// <param name="token">token a check</param>
    [AllowAnonymous]
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<TokenDto>), Status200OK)]
    [HttpPost]
    [Route("TokenTest")]
    public async Task<IActionResult> TokenTest([FromBody] string token)
    {
        bool isTokenValid = _userPlatform.TestTokenValidity(token);

        if (isTokenValid)
            return Ok();
        else return Unauthorized();
    }

    //[HttpPost]
    //[Route("ConfirmDiscord")]
    //public async Task<IActionResult> ConfirmDiscord([FromBody] ConfirmDiscordDTO dto)
    //{
    //    if (!ModelState.IsValid) return BadRequest(ModelState);

    //    ApiUser user = await userManager.FindByEmailAsync(dto.DiscordId);
    //    if (user == null) return BadRequest("Nom d'utilisateur invalide");

    //    if (user.EmailConfirmed) return BadRequest("Le compte discord est déjà validé");

    //    IdentityResult? result = await userManager.ConfirmEmailAsync(user, dto.ConfirmationToken);
    //    if (!result.Succeeded) return BadRequest(result.Errors);

    //    result = await userManager.AddPasswordAsync(user, dto.Password);
    //    if (!result.Succeeded) return BadRequest(result.Errors);

    //    return Ok("Le compte discord a été validé avec succès");
    //}


    /// <summary>
    /// Check if the user already exists 
    /// </summary>
    /// <param name="DiscordId">discord id de l'utilisateur a check</param>
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult), Status200OK)]
    [HttpGet]
    [Route("UserExist/{email}")]
    public async Task<ActionResult> UserExist([FromRoute] string email)
    {
        UserApi? user = await _userPlatform.GetByEmailAsync(email);
        if (user is null) return NoContent();
        else return Ok();
    }

    /// <summary>
    /// Permet de suprimer un utilisateur
    /// </summary>
    /// <param name="email">Email of user to delete</param>
    [Authorize(Roles = Roles.Admin)]
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult), Status204NoContent)]
    [HttpDelete]
    [Route("DeleteUser/{email}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string email)
    {
        UserApi? user = await _userPlatform.GetByEmailAsync(email);
        if (user is null)
            return NotFound(EUserErrorCodes.UserNotFoundById);

        await _userPlatform.DeleteAsync(user);

        return NoContent();
    }

    /// <summary>
    /// Create a request for a password change token
    /// </summary>
    /// <param name="dto">Password change template</param>
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<TokenDto>), Status200OK)]
    [HttpPost]
    [Route("FrogotPassword")]
    public async Task<ActionResult<string>> FrogotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        ValidationResult validationResult = _forgotPasswordValidator.Validate(forgotPasswordDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        UserApi? user = await _userPlatform.GetByEmailAsync(forgotPasswordDto.Email);
        if (user is null) return BadRequest("USER_NOT_FOUND");

        string token = await _userPlatform.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    /// <summary>
    /// Reset a password with a password reset token 
    /// </summary>
    /// <param name="resetPasswordDto">ResetPasswordDTO</param>
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult), Status200OK)]
    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        ValidationResult validationResult = _resetPasswordValidator.Validate(resetPasswordDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        UserApi? user = await _userPlatform.GetByEmailAsync(resetPasswordDto.Email);
        if (user is null) return BadRequest(EUserErrorCodes.UserNotFoundById);

        IdentityResult? result = await _userPlatform.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }


}
