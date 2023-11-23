using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.API.Validator.TodoTask;
using Template.Domain.Dto.TodoTask;
using Template.Domain.ErrorCodes;
using Xunit;

namespace Template.Unit.test;

public class TodoValidatorTest
{
    [Theory]
    [MemberData(nameof(UpdateTodoTaskDtoWithInvalidIdTask))]
    [MemberData(nameof(UpdateTodoTaskDtoWithInvalidTaskName))]
    [MemberData(nameof(UpdateTodoTaskDtoWithInvalidTaskDescription))]
    [MemberData(nameof(UpdateTodoTaskDtoWithInvalidIdUser))]
    public void UpdateTodoTaskValidator_WithInvalidData_ShouldHaveErrors(UpdateTodoTaskDto todoDto, ETodoTaskErrorCodes expectedErrorCode)
    {
        // Arrange
        var validator = new UpdateTodoTaskValidator();

        // Act
        ValidationResult validationResult = validator.Validate(todoDto);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(error => error.ErrorCode == expectedErrorCode.ToString());
    }

    public static TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes> UpdateTodoTaskDtoWithInvalidIdTask()
    {
        return new TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes>
            {
                {
                    // Empty IdTask
                    new UpdateTodoTaskDto { IdTask = Guid.Empty, TaskName = "Task", Start = DateTime.MinValue, End = DateTime.MaxValue, Description = "Description", IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.TaskIdNullOrEmpty
                }
            };
    }
    public static TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes> UpdateTodoTaskDtoWithInvalidTaskName()
    {
        return new TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes>
            {
                {
                    // Null TaskName
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = null, Start = DateTime.MinValue, End = DateTime.MaxValue, Description = "Description", IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.TaskNameNullOrEmpty
                },
                {
                    // Empty TaskName
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = string.Empty, Start = DateTime.MinValue, End = DateTime.MaxValue, Description = "Description", IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.TaskNameNullOrEmpty
                },
                {
                    // Whitespace TaskName
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = "       ", Start = DateTime.MinValue, End = DateTime.MaxValue, Description = "Description", IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.TaskNameNullOrEmpty
                }
            };
    }
    public static TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes> UpdateTodoTaskDtoWithInvalidTaskDescription()
    {
        return new TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes>
            {
                {
                    // Null Description
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = "Task", Start = DateTime.MinValue, End = DateTime.MaxValue, Description = null, IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.DescriptionNullOrEmpty
                },
                {
                    // Empty Description
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = "Task", Start = DateTime.MinValue, End = DateTime.MaxValue, Description = string.Empty, IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.DescriptionNullOrEmpty
                },
                {
                    // whitespace Description
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = "Task", Start = DateTime.MinValue, End = DateTime.MaxValue, Description = "        ", IdUser = Guid.NewGuid() },
                    ETodoTaskErrorCodes.DescriptionNullOrEmpty
                }
            };
    }
    public static TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes> UpdateTodoTaskDtoWithInvalidIdUser()
    {
        return new TheoryData<UpdateTodoTaskDto, ETodoTaskErrorCodes>
            {
                {
                    // Empty IdUser
                    new UpdateTodoTaskDto { IdTask = Guid.NewGuid(), TaskName = "Task", Start = DateTime.MinValue, End = DateTime.MaxValue, Description = "Description", IdUser = Guid.Empty },
                    ETodoTaskErrorCodes.IdUserNullOrEmpty
                }
            };
    }
}
