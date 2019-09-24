using SDmS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SDmS.Mvc.Infrastructure.Services
{
    public class CommandFactory
    {
        private readonly Dictionary<string, ICommand> _commandImplementations;
        private static volatile CommandFactory _commandFactoryInstance;
        private static object _syncRoot = new object();

        private CommandFactory()
        {
            _commandImplementations = new Dictionary<string, ICommand>();
            Initialize();
        }

        public static CommandFactory Instance
        {
            get
            {
                if (_commandFactoryInstance == null)
                {
                    lock (_syncRoot)
                    {
                        _commandFactoryInstance = new CommandFactory();
                    }
                }
                return _commandFactoryInstance;
            }
        }

        public ICommand GetCommand(string commandName)
        {
            return _commandImplementations[commandName.ToLower()];
        }

        private void Initialize()
        {
            // 1. get assembly
            Assembly asm = Assembly.GetCallingAssembly();

            // 2. get the list of all types in this assembly and iterate
            foreach (Type type in asm.GetTypes())
            {
                // we want the non-abstract implementations of ICommand
                if (type.IsClass && !type.IsAbstract)
                {
                    Type iCommand = type.GetInterface("SDmS.Infrastructure.Interfaces.ICommand");
                    if (iCommand != null)
                    {
                        // create an instance
                        object inst = asm.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, null, null, null);
                        if (inst != null)
                        {
                            ICommand commandInst = (ICommand)inst;
                            // make it case insensitive
                            string key = commandInst.CommandName.ToLower();
                            _commandImplementations.Add(key, commandInst);
                        }
                        else
                        {
                            string errMsg =
                                "CommandFactory.Initialize(): Unable " +
                                "to properly initialize " +
                                "CommandFactory - there was a " +
                                "problem instantiating the class " +
                                type.FullName;
                            throw new Exception(errMsg);
                        }
                    }
                }
            }
        }
    }
}