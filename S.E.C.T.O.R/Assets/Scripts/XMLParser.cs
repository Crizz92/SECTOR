using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public static class XMLParser
{
    public static Dictionary<string,string> ParseXMLFile(string fileName, string language)
    {
        string filePath = "GameText/"+language+"/"+fileName;
        TextAsset xmlFile = (TextAsset)Resources.Load(filePath);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.SelectNodes("//Root/Text");

        Dictionary<string, string> _translations = new Dictionary<string, string>();

        foreach(XmlNode xmlNode in xmlNodeList)
        {
            _translations.Add(xmlNode.Attributes["name"].Value, xmlNode.InnerText);    
        }
        return _translations;   
    }
}
