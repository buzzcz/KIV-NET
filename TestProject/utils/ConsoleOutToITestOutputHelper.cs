﻿using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace TestProject.Utils
{
    public class ConsoleOutToITestOutputHelper : TextWriter
    {
        ITestOutputHelper _output;
        public ConsoleOutToITestOutputHelper(ITestOutputHelper output)
        {
            _output = output;
        }
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string message)
        {
            _output.WriteLine(message);
        }
        public override void WriteLine(string format, params object[] args)
        {
            _output.WriteLine(format, args);
        }
    }
}