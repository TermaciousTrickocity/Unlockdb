using System;
using System.Collections.Generic;
using System.Xml;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide the XML file location as a command line argument.");
            return;
        }

        string xmlFilePath = args[0];
        string attributeName = "state";
        string attributeValue = "eUnlockState_Unlocked";

        try
        {
            // Load the XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            // Get all elements with the specified attribute
            XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes($"//*[@{attributeName}]");

            // Track the IDs that have been processed
            HashSet<string> processedIds = new HashSet<string>();

            // Update the attribute value for each matching element and list the associated id
            bool changesMade = false;

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes != null && node.Attributes[attributeName] != null)
                {
                    string currentStateValue = node.Attributes[attributeName].Value;

                    if (currentStateValue != attributeValue)
                    {
                        string idValue = node.Attributes["id"]?.Value;
                        if (!string.IsNullOrEmpty(idValue))
                        {
                            if (!processedIds.Contains(idValue))
                            {
                                processedIds.Add(idValue);

                                Console.WriteLine($"Changing state to '{attributeValue}' for id: {idValue}");
                                node.Attributes[attributeName].Value = attributeValue;
                                changesMade = true;
                            }
                        }
                    }
                }
            }

            if (changesMade)
            {
                // Save the modified XML document
                xmlDoc.Save(xmlFilePath);
                Console.WriteLine("XML file updated successfully.");
            }
            else
            {
                Console.WriteLine("No changes were made to the XML file as there was nothing to change.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
