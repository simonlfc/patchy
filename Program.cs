using Binarysharp.MemoryManagement;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Patchy
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
            try
            {
                var sharp = new MemorySharp(Process.GetProcessesByName("iw3mp").First());
                foreach (var patch in Directory.EnumerateFiles("../../patches", "*.cs", SearchOption.AllDirectories))
                {
                    Console.WriteLine("Compiling {0}...", patch.ToString());
                    CompilerOptions.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                    CompilerOptions.ReferencedAssemblies.Add("System.dll");
                    CompilerOptions.ReferencedAssemblies.Add("System.Core.dll");
                    CompilerOptions.ReferencedAssemblies.Add("E:\\Sources\\patchy\\packages\\MemorySharp.1.2.0\\lib\\MemorySharp.dll"); // i know
                    var compiler = provider.CompileAssemblyFromSource(CompilerOptions, File.ReadAllText(patch));

                    foreach (var error in compiler.Errors)
                    {
                        Console.WriteLine(error.ToString());
                    }

                    if (!compiler.Errors.HasErrors)
                    {
                        var module = compiler.CompiledAssembly.GetModules()[0];
                        try
                        {
                            module.GetType("Patchy." + Path.GetFileNameWithoutExtension(patch)).GetMethod("Main").Invoke(null, new object[] { sharp });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.InnerException);
                        }
                    }
                }

                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("no process, press the any key");
                Console.ReadLine();
            }
        }
    }
}
