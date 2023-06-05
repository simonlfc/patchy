using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
using Binarysharp.MemoryManagement;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace patchy
{
    internal class Program
    {
        static readonly CompilerParameters CompilerOptions = new CompilerParameters
        {
            GenerateExecutable = false,
            GenerateInMemory = true,
            TreatWarningsAsErrors = false
        };

        static readonly CSharpCodeProvider provider = new CSharpCodeProvider();
        
        static void Main()
        {
            var sharp = new MemorySharp(Process.GetProcessesByName("iw3mp").First());
            foreach (var patch in Directory.EnumerateFiles("../../patches", "*.pt", SearchOption.AllDirectories))
            {
                Console.WriteLine("Compiling {0}...", patch.ToString());
                CompilerOptions.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                CompilerOptions.ReferencedAssemblies.Add("System.dll");
                CompilerOptions.ReferencedAssemblies.Add("System.Core.dll");
                CompilerOptions.ReferencedAssemblies.Add("E:\\Sources\\patchy\\packages\\MemorySharp.1.2.0\\lib\\MemorySharp.dll"); // i know
                var compiler = provider.CompileAssemblyFromSource(CompilerOptions, File.ReadAllText(patch));

                foreach (var error in compiler.Errors)
                    Console.WriteLine(error.ToString());

                if (!compiler.Errors.HasErrors)
                {
                    var module = compiler.CompiledAssembly.GetModules()[0];
                    try
                    {
                        module.GetType("patchy." + Path.GetFileNameWithoutExtension(patch)).GetMethod("Main").Invoke(null, new object[] { sharp });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.InnerException.Message);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
