using System;

namespace Tar.Tests.Core
{
    public class InterfaceImpl: IInterface
    {
        public bool WriteLine(string message)
        {
            Console.WriteLine(message);
            return true;
        }
    }
}