using System;
using SbfCompiler;

namespace Esolangs.Sbf
{
    /// <summary>
    /// Compiler implements the sbf compiler.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //if (args.Length > 0)
            //{
            string fileName = /*args[0]*/ @"hello.sbf";

            Compiler compiler;
            compiler = new Compiler(fileName);

            Type myType = compiler.Compile();
            //}
        }        
    };
}