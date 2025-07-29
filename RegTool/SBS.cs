using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RegTool
{
    class BasicSBS
    {
        //public string Pre_cmd;
        //public string sub_cmd;
        public string cmd;
        public string name;
        public string size;
        public string value;
        public string unit;
        public string RWbit;
        public string Format;
        public string Description;
    }
    class SBS
    {
        public List<BasicSBS> SBSlist = new List<BasicSBS>();
        public SBS(string path)
        {
            if (!File.Exists(path)) //沒File就return
            {
                MessageBox.Show("No SBS Setting Exist!");
                return;
            }
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(sr, settings);
            BasicSBS bs = null;
            bool bCmd = false;
            bool bName = false;
            bool bSize = false;
            bool bValue = false;
            bool bUnit = false;
            bool bRWbit = false;
            bool bFormat = false;
            bool bDescription = false;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        //Console.WriteLine();
                        //Console.Write("{0} : ", reader.Name);
                        switch (reader.Name)
                        {
                            case "element":
                                //create a new element and put it in Map
                                bs = new BasicSBS(); //initialize element object
                                break;
                            case "cmd":
                                //set boolean values for fields, will be used in setting SBS variables
                                bCmd = true;
                                break;
                            case "name":
                                bName = true;
                                break;
                            case "size":
                                bSize = true;
                                break;
                            case "value":
                                bValue = true;
                                break;
                            case "unit":
                                bUnit = true;
                                break;
                            case "RWbit":
                                bRWbit = true;
                                break;
                            case "Format":
                                bFormat = true;
                                break;
                            case "Description":
                                bDescription = true;
                                break;
                            default:
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        //Console.Write(reader.Value);
                        if (bCmd)
                        {
                            //bs.cmd = Convert.ToByte(reader.Value, 16);
                            bs.cmd = reader.Value;
                            bCmd = false;
                        }
                        else if (bName)
                        {
                            bs.name = reader.Value;
                            bName = false;
                        }
                        else if (bSize)
                        {
                            //bs.size = Convert.ToByte(reader.Value);
                            bs.size = reader.Value;
                            bSize = false;
                        }
                        else if (bValue)
                        {
                            bs.value = reader.Value;
                            bValue = false;
                        }
                        else if (bUnit)
                        {
                            bs.unit = reader.Value;
                            bUnit = false;
                        }
                        else if (bRWbit)
                        {
                            bs.RWbit = reader.Value;
                            bRWbit = false;
                        }
                        else if (bFormat)
                        {
                            bs.Format = reader.Value;
                            bFormat = false;
                        }
                        else if (bDescription)
                        {
                            bs.Description = reader.Value;
                            bDescription = false;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        //Console.Write(", ");
                        if (reader.Name == "element")
                            SBSlist.Add(bs); //add BasicSBS object to list
                        break;
                }
            }
            //Console.WriteLine();
        }
    }
}
