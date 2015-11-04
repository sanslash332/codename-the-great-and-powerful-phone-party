using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

using CocosSharp;
//using project.cpp.Shared;
using project.cpp.Core;



namespace project.cpp.windx
{   

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
        static void Main(string[] args)
        {
            Console.Title("Codename, cpp ");
            CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
            application.ApplicationDelegate = new AppDelegate();

            application.StartGame();
        }
    }


}

