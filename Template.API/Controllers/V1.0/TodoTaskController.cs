using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;
using Template.Domain.ErrorCodes;
using Template.Platform.Interfaces;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Error = Template.Domain.Models.HttpError.Error;

namespace Template.API.Controllers.V1._0;

[Authorize]
[ApiController]
[Route("api/v1.0/[controller]")]
public class TodoTaskController : ControllerBase
{
    #region Props
    private readonly IValidator<CreateTodoTaskDto> _createTodoTaskValidator;
    private readonly IValidator<UpdateTodoTaskDto> _updateTodoTaskValidator;
    private readonly IUserPlatform _userPlatform;
    private readonly ITodoTaskPlatform _todoTaskPlatform;

    private readonly IMapper _mapper;
    #endregion

    #region Ctor
    public TodoTaskController(IValidator<CreateTodoTaskDto> createTodoTaskValidator, IUserPlatform userPlatform, ITodoTaskPlatform todoTaskPlatform, IMapper mapper, IValidator<UpdateTodoTaskDto> updateTodoTaskValidator)
    {
        _createTodoTaskValidator = createTodoTaskValidator ?? throw new ArgumentNullException(nameof(createTodoTaskValidator));
        _userPlatform = userPlatform ?? throw new ArgumentNullException(nameof(userPlatform));
        _todoTaskPlatform = todoTaskPlatform ?? throw new ArgumentNullException(nameof(todoTaskPlatform));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _updateTodoTaskValidator = updateTodoTaskValidator ?? throw new ArgumentNullException(nameof(updateTodoTaskValidator));
    }
    #endregion

    /// <summary>
    /// Add a new TodoTask
    /// </summary>
    /// <param name="createTodoTaskDto">CreateTodoTaskDto</param>
    /// <returns>A Task with TodoTask</returns>
    /// <response code="401">Unauthorized</response>
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<TodoTaskDto>), Status201Created)]
    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<TodoTaskDto>> AddTodoTask([FromBody] CreateTodoTaskDto createTodoTaskDto)
    {
        //ValidationResult validationResult = _createTodoTaskValidator.Validate(createTodoTaskDto);
        //if (!validationResult.IsValid)
        //    return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        UserApi? user = await _userPlatform.GetByIdAsync(createTodoTaskDto.IdUser);
        if (user is null) return BadRequest(EUserErrorCodes.UserNotFoundById);

        TodoTask todoTask = _mapper.Map<CreateTodoTaskDto, TodoTask>(createTodoTaskDto);

        await _todoTaskPlatform.CreateTodoTaskAsync(todoTask);

        return Created("", _mapper.Map<TodoTask, TodoTaskDto>(todoTask));
    }

    /// <summary>
    /// Get All TodoTask
    /// </summary>
    /// <param name="skip">to skip n first result</param>
    /// <param name="top">to get n max len of result</param>
    /// <returns>A Task with IQueryable of TodoTaskDto</returns>
    [OutputCache]
    [AllowAnonymous]
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<IQueryable<TodoTaskDto>>), Status200OK)]
    [HttpGet]
    [Route("All")]
    public IQueryable<TodoTaskDto> GetAll([FromQuery] int? skip, int? top)
    {
        IQueryable<TodoTask> todoTasks = _todoTaskPlatform.GetAll();
        if (skip.HasValue)
            todoTasks = todoTasks.Skip(skip.Value);
        if (top.HasValue)
            todoTasks = todoTasks.Take(top.Value);
        return _mapper.ProjectTo<TodoTaskDto>(todoTasks);
    }

    /// <summary>
    /// Get a todoTask by its Id
    /// </summary>
    /// <param name="todoTaskId">Id of wanted TodoTask</param>
    /// <returns>A Task with TodoTask Dto</returns>
    [ProducesErrorResponseType(typeof(Error))]
    [ProducesResponseType(typeof(ActionResult<TodoTaskDto>), Status200OK)]
    [HttpGet]
    [Route("{todoTaskId}")]
    public async Task<ActionResult<TodoTaskDto>> GetById([FromRoute] Guid todoTaskId)
    {
        TodoTask? todoTask = await _todoTaskPlatform.GetByIdAsync(todoTaskId);
        if (todoTask is null) return BadRequest(ETodoTaskErrorCodes.NotFoundById);

        return _mapper.Map<TodoTask, TodoTaskDto>(todoTask);
    }

    /// <summary>
    /// Update TodoTask
    /// </summary>
    /// <param name="todoTaskId">Id of updated Task</param>
    /// <param name="updateDto">Dto with new values</param>
    /// <returns>Taks with Updated TodoTask</returns>
    [ProducesErrorResponseType(typeof(ActionResult<Error>))]
    [ProducesResponseType(typeof(ActionResult<TodoTaskDto>), Status200OK)]
    [HttpPut]
    [Route("{todoTaskId}")]
    public async Task<ActionResult<TodoTaskDto>> UpdateAsync([FromRoute] Guid todoTaskId, [FromBody] UpdateTodoTaskDto updateDto)
    {
        TodoTask? todoTask = await _todoTaskPlatform.GetByIdAsync(todoTaskId);
        if (todoTask is null)
            return NotFound();

        ValidationResult? validationResult = _updateTodoTaskValidator.Validate(updateDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        todoTask = await _todoTaskPlatform.UpdateAsync(todoTask, updateDto);

        return _mapper.Map<TodoTask, TodoTaskDto>(todoTask);
    }


    /// <summary>
    /// Delete task by ID 
    /// </summary>
    /// <param name="todoTaskId">Id of delete Task</param>
    [ProducesErrorResponseType(typeof(ActionResult<Error>))]
    [ProducesResponseType(typeof(ActionResult), Status204NoContent)]
    [HttpDelete]
    [Route("{todoTaskId}")]
    public async Task<ActionResult> DeleteKeyAsync([FromRoute] Guid todoTaskId)
    {
        TodoTask? translation = await _todoTaskPlatform.GetByIdAsync(todoTaskId);
        if (translation is null)
            return NotFound();

        await _todoTaskPlatform.DeleteAsync(translation);

        return NoContent();
    }
}
