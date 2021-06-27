using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Net.Mail;
using System.Text.RegularExpressions;

// Author: Zackery Jaouni
// Last updated: 6/26/2021
// Description: Attempts to parse an XML file for email address and
// seperates the valid and invalid email addresses into seperate lists
namespace XML_Technical_Assignment
{
    public class EmailValidator
    {
        private XmlDocument recordsDoc = new XmlDocument(); // Represents a XML file to parse for email adresses
        private ArrayList validEmails = new ArrayList(); // Array list of valid emails in a XML file
        private ArrayList invalidEmails = new ArrayList(); // Array list of invalid email in a XML file

        // Constructor that takes a name of a XML file as a parameter.
        // Attempts to find the XML file in the directory and parse it.
        // If the file can be parsed the method createListsOfEmails is
        // called to create a lists of valid and invalid emails.
        // If the file cannot be found or could not be parsed an error 
        // message is displayed.
        public EmailValidator(String fileName)
        {
            if(isFileInDirectory(fileName))
            {
                try
                {
                    recordsDoc.Load(fileName);
                    createListsOfEmails();
                }
                catch(Exception)
                {
                    Console.WriteLine("Error: " + fileName + " could not be parsed as an xml file"); 
                }
            }
        }

        // Determine if a file can be located in the current directory and return true.
        // If the file could not be located returns false and an error message is displayed.
        // The parameter fileName is the name of the file to locate.
        public Boolean isFileInDirectory(String fileName)
        {
            if (File.Exists(fileName))
            {
                Console.WriteLine(fileName + " was successfully found in the directory.");
                return true;
            }
            Console.WriteLine("Error: "+ fileName + " could not be located in the directory!");
            return false;
        }

        // Iterates through each record of a XML file for an email address.
        // For each email address the method isValidEmail is used to determine if the   
        // email address should be added to validEmails or invalidEmails.
        // (Note: Only email addresses contained in a data elements named, 
        // emailAddress, are considered for the lists)
        public void createListsOfEmails()
        {
            XmlNodeList emailList = recordsDoc.GetElementsByTagName("emailAddress");
            for (int i=0; i < emailList.Count; i++)
            {
                String email = emailList[i].InnerXml;
                if(isValidEmail(email))
                {
                    validEmails.Add(email);
                }
                else
                {
                    invalidEmails.Add(email);
                }
            }
        }

        // Determines if an email address is in valid syntax.
        // The parameter email is the name of the email address to validate.
        public Boolean isValidEmail(String email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                Regex repeatedSpecial = new Regex(@"\.{2,}");  // Pattern for repeated dot . 
                //Regex repeatedSpecial = new Regex(@"\W{2,}"); 
                if(repeatedSpecial.IsMatch(email)) //Check for repeated dot . as it indicates invalid email address
                //if(repeatedSpecial.IsMatch(email) || !endOfEmailValidator(email))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        // Determines if an email address has the ends in any two letters.
        // The parameter email is the name of the email address to validate.
        public Boolean endOfEmailValidator(String email)
        {
            MailAddress address = new MailAddress(email);
                Regex endDomain = new Regex(@"[a-zA-Z]{2}$"); // Pattern for ending in any two letters. 
                if(endDomain.IsMatch(email)) //Check that the last two characters of an email address is two letters
                {
                    return true;
                }

            return false;
        }

        // Outputs the list of valid Emails
        public void displayValidEmails()
        {
            Console.WriteLine("There are "+ validEmails.Count + " valid emails");
            Console.WriteLine("The list of valid emails:");
            foreach(String email in validEmails)
            {
                Console.WriteLine(email);
            }  
        }

        // Outputs the list of invalid Emails
        public void displayInvalidEmails()
        {
            Console.WriteLine("There are "+ invalidEmails.Count + " invalid emails");
            Console.WriteLine("The list of invalid emails:");
            foreach(String email in invalidEmails)
            {
                Console.WriteLine(email);
            }
        }

        // Prompts the user to enter an XML file to search for in the directory.
        // If the XML file can be succussefully parsed then the lists of
        // valid and invalid emails are displayed. 
        // If the file could not be located or could not be parsed an appropriate
        // error message is displayed
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the name of an XML file to read:");
            String fileName = Console.ReadLine();           
            EmailValidator eV = new EmailValidator(fileName);
            Console.WriteLine();
            eV.displayValidEmails();
            Console.WriteLine();
            eV.displayInvalidEmails();
        }
    }    
}
