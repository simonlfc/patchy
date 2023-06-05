using System;
using Binarysharp.MemoryManagement;

namespace Patchy
{
    public class HelloWorld
    {
        /*
        static private void Cbuf_AddText(MemorySharp handle, string text, int localClientNum)
        {
            var address = (IntPtr)0x4F8D90u;

            using (var t = handle.Assembly.BeginTransaction())
            {
                t.AddLine("pushad");
                t.AddLine("mov eax, {0}", text);
                t.AddLine("mov ecx, {0}", localClientNum);
                t.AddLine("call {0}", address);
                t.AddLine("popad");
            }
        }
        */

        static private void LiveStorage_StatSetCmd(MemorySharp handle, int stat, int value)
        {
            var address = (IntPtr)0x579CE0;

            using (var t = handle.Assembly.BeginTransaction())
            {
                t.AddLine("pushad");
                t.AddLine("mov eax, {0}", stat);
                t.AddLine("mov edx, {0}", value);
                t.AddLine("call {0}", address);
                t.AddLine("popad");
            }
        }

        static public void Main(MemorySharp handle)
        {
            Console.WriteLine("HelloWorld");

            // remove statset protection
            handle.Write<byte>((IntPtr)0x579DF9, 0xEB);

            LiveStorage_StatSetCmd(handle, 2301, 120280);
            //Cbuf_AddText(handle, "uploadStats\n", 0);
        }
    }
}