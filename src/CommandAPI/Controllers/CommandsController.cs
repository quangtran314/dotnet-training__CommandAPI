using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase {
        private readonly ICommandAPIRepo _commandAPIRepo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo commandAPIRepo, IMapper mapper)
        {
            _commandAPIRepo = commandAPIRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands(){
            return Ok(_mapper.Map<IEnumerable<Command>>(_commandAPIRepo.GetAllCommands()));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var command = _commandAPIRepo.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto command)
        {
            var commandModel = _mapper.Map<Command>(command);
            _commandAPIRepo.CreateCommand(commandModel);

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            return CreatedAtRoute(nameof(GetCommandById), new
            {
                Id = commandReadDto.Id
            }, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto command)
        {
            var commandModelFromRepo = _commandAPIRepo.GetCommandById(id);
            if (commandModelFromRepo == null)
                return NotFound();

            _mapper.Map(command, commandModelFromRepo);
            _commandAPIRepo.UpdateCommand(commandModelFromRepo);

            _commandAPIRepo.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _commandAPIRepo.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _commandAPIRepo.UpdateCommand(commandModelFromRepo);

            _commandAPIRepo.SaveChanges();

            return NoContent() ;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _commandAPIRepo.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _commandAPIRepo.DeleteCommand(commandModelFromRepo);
            _commandAPIRepo.SaveChanges();
            return NoContent();
        }
    }
}