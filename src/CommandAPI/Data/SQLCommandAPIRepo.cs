using CommandAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommandAPI.Data
{
    public class SQLCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext _context;

        public SQLCommandAPIRepo(CommandContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
                throw new System.NotImplementedException();

            _context.CommandItems.Add(cmd);
            SaveChanges();
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
                throw new System.ArgumentNullException();
            _context.CommandItems.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _context.CommandItems.FirstOrDefault(c => c.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void UpdateCommand(Command cmd)
        {
        }
    }
}
