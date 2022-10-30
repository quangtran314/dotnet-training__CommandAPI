using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using CommandAPI.Controllers;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests
    {
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}
