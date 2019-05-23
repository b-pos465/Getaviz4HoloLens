using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Import
{
    public class HtmlImporter
    {
        private static readonly string ATTRIBUTE_ID = "id";
        private static readonly string ATTRIBUTE_POSITION = "position";
        private static readonly string ATTRIBUTE_WIDTH = "width";
        private static readonly string ATTRIBUTE_HEIGHT = "height";
        private static readonly string ATTRIBUTE_DEPTH = "depth";
        private static readonly string ATTRIBUTE_COLOR = "color";

        private string path;

        public HtmlImporter(string path)
        {
            this.path = path;
        }

        public Dictionary<ID, TransformAndColorInformation> Import()
        {
            string modelAsHTML = File.ReadAllText(this.path, Encoding.UTF8);
            string modelAsXML = this.CutHTMLBoilerplate(modelAsHTML);

            return this.ExtractTransformAndColorInformationFromXMLModel(modelAsXML);
        }

        private string CutHTMLBoilerplate(string modelAsHTML)
        {
            string result = modelAsHTML.Substring(modelAsHTML.IndexOf("</a-entity>") + 11);
            result = result.Substring(0, result.IndexOf("</a-entity>") + 11);

            return result;
        }

        private Dictionary<ID, TransformAndColorInformation> ExtractTransformAndColorInformationFromXMLModel(string modelAsXML)
        {
            Dictionary<ID, TransformAndColorInformation> resultDictionary = new Dictionary<ID, TransformAndColorInformation>();

            XElement xmlRoot = XElement.Parse(modelAsXML);

            foreach (XElement xElement in xmlRoot.Elements())
            {

                ID id = ID.From(xElement.Attribute(ATTRIBUTE_ID).Value);
                Vector3 position = this.ParsePosition(xElement);
                Vector3 scale = this.ParseScale(xElement);
                Color color = this.ParseColor(xElement);

                TransformAndColorInformation transformAndColorInformation = new TransformAndColorInformation(position, scale, color);
                resultDictionary.Add(id, transformAndColorInformation);
            }
            return resultDictionary;
        }

        private Vector3 ParsePosition(XElement xElement)
        {
            XAttribute xAttribute = xElement.Attribute(ATTRIBUTE_POSITION);
            string[] coordinates = xAttribute.Value.Split(' ');

            float x = (float)Convert.ToDouble(coordinates[0]);
            float y = (float)Convert.ToDouble(coordinates[1]);
            float z = (float)Convert.ToDouble(coordinates[2]);
            return new Vector3(x, y, z);
        }

        private Vector3 ParseScale(XElement xElement)
        {
            XAttribute xAttribute = xElement.Attribute(ATTRIBUTE_WIDTH);
            float x = (float)Convert.ToDouble(xAttribute.Value);

            xAttribute = xElement.Attribute(ATTRIBUTE_HEIGHT);
            float y = (float)Convert.ToDouble(xAttribute.Value);

            xAttribute = xElement.Attribute(ATTRIBUTE_DEPTH);
            float z = (float)Convert.ToDouble(xAttribute.Value);

            return new Vector3(x, y, z);
        }

        private Color ParseColor(XElement xElement)
        {
            XAttribute xAttribute = xElement.Attribute(ATTRIBUTE_COLOR);
            Color result;
            ColorUtility.TryParseHtmlString(xAttribute.Value, out result);
            return result;
        }
    }
}