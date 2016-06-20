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
            if (args.Length < 1)
            {
                string fileName = @"hello.sbf";

                Compiler compiler;
                compiler = new Compiler(fileName);

                compiler.Compile();
            }
            else
            {
                foreach (string fileName in args)
                {
                    Compiler compiler;
                    compiler = new Compiler(fileName);

                    compiler.Compile();
                }
            }
        }        
    };
}