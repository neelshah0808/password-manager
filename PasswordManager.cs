/*
 * Program:         PasswordManager.exe
 * Module:          PasswordManager.cs
 * Date:            30th May 2022
 * Author:          Yug Shah, Dhanashri Nayi
 * Description:     Some free starting code for INFO-3138 project 1, the Password Manager
 *                  application. All it does so far is demonstrate how to obtain the system date 
 *                  and how to use the PasswordTester class provided.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
 // File class

using Newtonsoft.Json; //JsonConvert class
using Newtonsoft.Json.Schema; //JSchema class
using Newtonsoft.Json.Linq; //JObject class


namespace PasswordManager
{
    class Program
    {
        private const string DATA_FILE = "../../account-data.json";
        private const string SCHEMA_FILE = "../../accounts-schema.json";
        private const string OUTPUT_FILE = "../../account-data.json"; // not yet impelmented
      


        private static bool changes = false;

        private static string Selected;
        private static string Selected_action;

        static void Main(string[] args)
        {
            string json = File.ReadAllText(DATA_FILE);
            List<Account> account  = JsonConvert.DeserializeObject<List<Account>>(json);

            // System date demonstration
            DateTime dateNow = DateTime.Now;
            Console.Write("PASSWORD MANAGEMENT SYSTEM (STARTING CODE), " + dateNow.ToShortDateString());
            Console.WriteLine("");
            string proDesign = "+---------------------------------------------------------------------------------------------------------------------+";
            Console.WriteLine(proDesign);
            Console.WriteLine("|                                                       Account Entries                                               |");
            
            DisplayAccountName(account);
               





        }
        //code to add account pending 
        private static void addAccount(List<Account> account)
        {
            //code for adding account
            Password pass = new Password();

          
            string newPass;
            Account acc = new Account();
            Console.WriteLine("     Please key-in values for the following fields... \n");

            Console.Write("     Description:          ");
            acc.Description = Console.ReadLine();
            Console.Write("     User ID:              ");
            acc.UserId = Console.ReadLine();

            Console.Write("     Password:             ");
            newPass = Console.ReadLine();

            Console.Write("     Login URL:             ");
            acc.LoginUrl = Console.ReadLine();

            Console.Write("     Account #:            ");
            acc.AccountNum = Console.ReadLine();
           
            pass.Value = newPass;
            DateTime dateNow = DateTime.Now;
            pass.LastReset = dateNow.ToString();
            PasswordTester pw = new PasswordTester(newPass);
            pass.StrengthText = pw.StrengthLabel;
            pass.StrengthValue = pw.StrengthPercent;
            acc.Password = pass;

            Console.WriteLine("     Account added");

            string json_schema = File.ReadAllText(SCHEMA_FILE);

            if (Validate(acc, json_schema))
            {
                Console.WriteLine("ERROR: Invalid account information entered. Please try again...");
                addAccount(account);
            }
            else
            {
                account.Add(acc);
                changes = true;
                DisplayAccountName(account);
                Console.WriteLine("     Account added");
               
            }

        }

        // code to save this to file pending 
        private static void savetofile(List<Account> account)
        {
  
           
                string json = JsonConvert.SerializeObject(account);
                File.WriteAllText(OUTPUT_FILE, json);
         
                
           
        }
        private static void DisplayAccountInfo(int selection, List<Account> account)
        {
            string proDesign = "+---------------------------------------------------------------------------------------------------------------------+";
            Console.WriteLine("");
            Console.WriteLine(proDesign);
            Console.WriteLine("               " + account[selection - 1].Description.ToString());
            Console.WriteLine(proDesign);
            Console.WriteLine("     UserID            :" + account[selection - 1].UserId.ToString());
            Console.WriteLine("     Password          :" + account[selection - 1].Password.Value.ToString());
            Console.WriteLine("     Password Strength :" + account[selection - 1].Password.StrengthText.ToString());
            Console.WriteLine("     Password Reset    :" + account[selection - 1].Password.LastReset.ToString());
            Console.WriteLine("     Login URL         :" + account[selection - 1].LoginUrl.ToString());
            Console.WriteLine("     Account #         :" + account[selection - 1].AccountNum.ToString());
            
            Console.WriteLine(proDesign);
            Console.WriteLine("");
            Console.WriteLine("     Press P to change this password.");
            Console.WriteLine("     Press D to delete this entry");
            Console.WriteLine("     Press M to return to the main menu.");
            Console.Write("     Enter a command:    ");
             Selected_action = Console.ReadLine();
            Console.WriteLine("");
            

            if(Selected_action == "M" || Selected_action == "m")
            {
                DisplayAccountName(account);
            }
            else if(Selected_action == "P" || Selected_action == "p")
            {
                ChangePassword(selection , account);
            }
            else if(Selected_action == "D" || Selected_action == "d")
            {
                DeleteAccount(selection , account);
            }
            else
            {
                Console.Write("     Select options from above");
                Console.ReadLine();
            }
             
        }
        private static void DeleteAccount(int selection , List<Account> account)
        {
            account.RemoveAt(selection - 1);
            Console.WriteLine("     Account deleted");
            changes = true;
            Console.WriteLine("");
            DisplayAccountName(account);
        }
      
        private static void ChangePassword(int selection , List<Account> account)
        {
            DateTime dateNow = DateTime.Now;
            //code to change account passsword
            Console.WriteLine("     change password");
            Console.Write("     New password: ");
            string newpass = Console.ReadLine();
            account[selection -1].Password.Value = newpass;
            PasswordTester pw = new PasswordTester(newpass);
            account[selection -1 ].Password.StrengthText = pw.StrengthLabel;
            account[selection -1 ].Password.StrengthValue = pw.StrengthPercent;
            account[selection - 1].Password.LastReset = dateNow.ToString();
           
                Console.WriteLine("");
                Console.WriteLine("     Password changed");
            changes = true;
                DisplayAccountName(account);
          
       
        }
        

        private static bool Validate(Account acc, string json_schema)
        {
            string json_data = JsonConvert.SerializeObject(acc);

            // Validate the data string against the schema contained in the 
            // json_schema parameter. Also, modify or replace the following 
            // return statement to return 'true' if item is valid, or 'false' 
            // if invalid.
            JSchema schema = JSchema.Parse(json_schema);
            JObject itemObj = JObject.Parse(json_data);
            return itemObj.IsValid(schema);
        }
        private static void TestPassword()
        {
            bool done;
            do
            {
                Console.Write("\n\nEnter a password: ");
                string pwText = Console.ReadLine();

                try
                {
                    // PasswordTester class demonstration
                    PasswordTester pw = new PasswordTester(pwText);
                    Console.WriteLine("That password is " + pw.StrengthLabel);
                    Console.WriteLine("That password has a strength of " + pw.StrengthPercent + "%");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("ERROR: Invalid password format");
                }

                Console.Write("\nTest another password? (y/n): ");
                done = Console.ReadKey().KeyChar != 'y';

            } while (!done);
        }

        private static void DisplayAccountName(List<Account> account)
        {

            string proDesign = "+---------------------------------------------------------------------------------------------------------------------+";
            Console.WriteLine(proDesign);
            for (int i = 0; i < account.Count; i++)
            {
                Console.WriteLine("     " + (i+1) +". " + account[i].Description.ToString());
            }
            Console.WriteLine(proDesign);
            Console.WriteLine("");
            Console.WriteLine("     Press # to get account details");
            Console.WriteLine("     Press A to add an account");
            Console.WriteLine("     Press X to exit");
            Console.Write("     Enter a command:    ");
           //int Selected_account = int.Parse(Console.ReadLine());
            Selected = Console.ReadLine();
            //int Selected_account1 = int.Parse(Selected);
            Console.WriteLine("");
            try
            {

                if (Selected == "X" || Selected == "x")
                {
                    if (changes)
                    {
                        savetofile(account);
                        Environment.Exit(-1);
                    }
                    else
                    {
                        Environment.Exit(-1);
                    }


                }
                else if (Selected == "A" || Selected == "a")
                {
                    addAccount(account);
                }
                else
                {
                    if (int.Parse(Selected) <= account.Count && int.Parse(Selected) > 0)
                    {
                        DisplayAccountInfo(int.Parse(Selected), account);
                    }
                    else
                    {
                        Console.WriteLine("     Select options from above");
                        Console.WriteLine("");
                        DisplayAccountName(account);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("     Select options from above");
                Console.WriteLine("");
                DisplayAccountName(account);
            }

        }
       
       
    } // end class
}
