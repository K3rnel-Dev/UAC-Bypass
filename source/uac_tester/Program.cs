﻿namespace UX
{
    internal class Program
    {
        public const string command = "/k @echo Hello world && regedit"; // your command
        static void Main(string[] args)
        {
            ClientStandalone.ux();
        }
    }
}
