/*
 * Program:         PasswordManager.exe
 * Module:          PasswordManager.cs
 * Date:            30th May 2022
 * Author:          Yug Shah, Dhanashri Nayi
 * Description:     Some free starting code for INFO-3138 project 1, the Password Manager
 *                  application. All it does so far is demonstrate how to obtain the system date 
 *                  and how to use the PasswordTester class provided.
 */

namespace PasswordManager
{
    public class Password
    {
        public string Value { get; set; }
        public ushort StrengthValue { get; set; }
        public string StrengthText { get; set; }
        public string LastReset { get; set; }
    }
    public class Account
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public string LoginUrl  { get; set; }
        public string AccountNum    { get; set; }
        public Password Password { get; set; }
    }

    
}