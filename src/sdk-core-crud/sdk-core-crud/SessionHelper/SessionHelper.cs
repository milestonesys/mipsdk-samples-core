using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Platform.SDK.Core;

namespace sdk_core_crud
{
    public class SessionHelper
    {
        private readonly string _defaultUrl = "http://localhost";
        private readonly UserType _defaultUserType = UserType.DefaultWindows;

        public SessionHelper()
        {
        }

        public SessionHelper(string defaultUrl, UserType defaultUserType)
        {
            _defaultUrl = defaultUrl;
            _defaultUserType = defaultUserType;
        }

        public ISession CreateSession(ServiceProvider serviceProvider)
        {
            string? serverUrl = "";
            UserType userType = _defaultUserType;
            string username = "";
            string password = "";
            string userToken = "";
            Console.WriteLine("Default settings are: ServerUrl: " + _defaultUrl + ", UserType: " + _defaultUserType);
            Console.WriteLine("Do you want to use default settings? (Y/N)");
            var info = Console.ReadKey(true);
            if (info.KeyChar == 'Y' || info.KeyChar == 'y')
            {
                serverUrl = _defaultUrl;

            }
            else
            {
                Console.WriteLine($"Input server URL (default {_defaultUrl}):");
                serverUrl = Console.ReadLine();
                if (string.IsNullOrEmpty(serverUrl))
                {
                    serverUrl = _defaultUrl;
                }
                Console.WriteLine($"Select user type:");
                Console.WriteLine("1: DefaultWindows");
                Console.WriteLine("2: Windows");
                Console.WriteLine("3: Basic");
                Console.WriteLine("4: External");
                char keyChar = ' ';
                while(keyChar < '1' || keyChar > '4')
                {
                    Console.WriteLine("Enter a number between 1 and 4:");
                    keyChar = Console.ReadKey(true).KeyChar;
                }
                switch (keyChar) 
                {
                    case '2':
                        userType = UserType.Windows;
                        break;
                    case '3':
                        userType = UserType.Basic;
                        break;
                    case '4':
                        userType = UserType.External;
                        break;
                    default:
                        userType = UserType.DefaultWindows;
                        break;
                }
                if (userType == UserType.Windows || userType == UserType.Basic)
                {
                    Console.WriteLine("Input username:");
                    username = Console.ReadLine() ?? "";
                    Console.WriteLine("Input password:");
                    password = ReadPassword();
                }
                else if (userType == UserType.External)
                {
                    Console.WriteLine("Input access token:");
                    userToken = ReadPassword();
                }
            }
            var serverUri = new Uri(serverUrl);
            var idpUri = new Uri(serverUri, "idp");
            var serverConfiguration = new ServerConfiguration(serverUri, idpUri);
            switch (userType)
            {
                case UserType.Windows:
                    return new Session(serverConfiguration, serviceProvider, new WindowsUser(username, password));
                case UserType.Basic:
                    return new Session(serverConfiguration, serviceProvider, new BasicUser(username, password));
                case UserType.External:
                    return new Session(serverConfiguration, serviceProvider, userToken);
                default:
                    return new Session(serverConfiguration, serviceProvider, new DefaultWindowsUser());
            }
        }

        /// <summary>
        /// Just a helper method to read input without showing it in the console, for password and token input. It also supports backspace to correct input.
        /// </summary>
        /// <returns>The entered string</returns>
        private string ReadPassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;
                if(key == ConsoleKey.Backspace && passwordBuilder.Length > 0)
                {
                    passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    passwordBuilder.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            } while (key != ConsoleKey.Enter);
            return passwordBuilder.ToString();
        }
    }
}
