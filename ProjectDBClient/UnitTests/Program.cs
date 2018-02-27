using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManuTh.Tasks;

namespace ManuTh.ProjectDBClient.UnitTests
{
    /// <summary>
    /// The Unit-Tests for <see cref="ProjectDBClient"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Runs the Unit-Tests for <see cref="Patchization"/>.
        /// </summary>
        /// <param name="args">The specified arguments.</param>
        internal static void Main(string[] args)
        {
            /*====================
             * 1. Connecting to the Project-Database.
             *====================
             */
            APIConnection connection;
            {
                string username;
                string password = "";

                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");

                for (ConsoleKeyInfo keyInfo = Console.ReadKey(true); keyInfo.Key != ConsoleKey.Enter; keyInfo = Console.ReadKey(true))
                {
                    if (keyInfo.Key != ConsoleKey.Backspace)
                    {
                        password += keyInfo.KeyChar;
                        Console.Write('*');
                    }
                    else if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                Console.WriteLine();

                connection = new APIConnection
                {
                    DataSource = new Uri("https://rhrpatcher.romresources.net/"),
                    Username = username,
                    Password = password
                };
            }

            try
            {
                Console.WriteLine(connection.CurrentUser.Name);
                Console.WriteLine(connection.CurrentUser.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            connection.GetAccessRequest<Project>().StartAsync().RunSync();
            Console.ReadLine();

            /*====================
             * 2. Downloading projects from the Project-Database.
             *====================
             */
            List<Project> projects = connection.GetAccessRequest<Project>().Start();

            /*====================
             * 3. Downloading a specified project from the Project-Database.
             *====================
             */
            {
                AccessRequest<List<Project>> request = connection.GetAccessRequest<Project>();
                request.Parameters = new Dictionary<string, object> { { "ID", 1 } };
                Project project = request.Start()[0];
            }

            /*====================
             * 4. Modifying a project.
             *====================
             */
            {
                projects[0].Name += " - Test";

                /*--------------------
                 * 4.1 Method 1: Creating the request at first, setting the input data and starting the request.
                 *--------------------
                 */
                UpdateRequest<Project> request = connection.GetUpdateRequest<Project>();
                request.Data = projects[0];
                request.Start();

                /*--------------------
                 * 4.2 Method 2: Creating and starting the request at once.
                 *--------------------
                 */
                connection.GetUpdateRequest(projects[0]).Start();
            }

            /*====================
             * 5. Creating a project.
             *====================
             */
            {
                Project project = new Project
                {
                    Name = "Chrono Trigger",
                    Author = new User { ID = 1 }
                };

                /*--------------------
                 * 5.1 Method 1: Creating the request at first, setting the input data and starting the request.
                 *--------------------
                 */
                {
                    CreationRequest<Project> request = connection.GetCreationRequest<Project>();
                    request.Data = project;
                    request.Start();
                }

                /*--------------------
                 * 5.2 Method 2: Doing everything at once.
                 *--------------------
                 */
                {
                    connection.GetCreationRequest(project).Start();
                }
            }

            /*====================
             * 6. Removing a project.
             *====================
             */
            {
                Project project = projects.First(p => p.Name.Contains("Sovereign"));
                connection.GetDeletionRequest(project).Start();
            }

            /*=====================
             * 7. Async execution
             *====================
             */
            {
                /*--------------------
                 * 7.1 Penetration-Test
                 *--------------------
                 */
                {
                    int taskCount = 32;

                    List<Task> tasks = new List<Task>();

                    for (int i = 0; i < taskCount; i++)
                    {
                        tasks.Add(connection.GetDeletionRequest<Project>().StartAsync());
                    }

                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }
                }
            }
        }
    }
}
