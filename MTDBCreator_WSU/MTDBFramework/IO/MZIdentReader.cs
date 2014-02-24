//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.IO;
//using MTDBFramework.Data;

//namespace MTDBFramework.IO
//{
//    public class MZIdentReader
//    {
//        private static Dictionary<string, DatabaseSequence> database = new Dictionary<string,DatabaseSequence>();
//        private static Dictionary<string, PeptideRef> peptides = new Dictionary<string,PeptideRef>();
//        private static Dictionary<string, PeptideEvidence> evidences = new Dictionary<string,PeptideEvidence>();

//        private class DatabaseSequence
//        {
//            string m_accession;
//            int m_length;
//            string m_proteinDescription;

//            public DatabaseSequence()
//            {
//            }

//            public string Accession
//            {
//                get { return m_accession; }
//                set { m_accession = value; }
//            }

//            public int Length
//            {
//                get { return m_length; }
//                set { m_length = value; }
//            }

//            public string ProteinDescription
//            {
//                get { return m_proteinDescription; }
//                set { m_proteinDescription = value; }
//            }
//        }

//        private class PeptideRef
//        {
//            string sequence;

//        }

//        private class PeptideEvidence
//        {
//            bool isDecoy;
//            char post;
//            char pre;
//            int end;
//            int start;          
//        }

//        static void TestReader()
//        {
//            string id;
//            DatabaseSequence dbSeq;

//            XmlReaderSettings XSettings = new XmlReaderSettings();
//            //XSettings.IgnoreWhitespace = true;
//            StreamReader sr = new StreamReader(@"C:\UnitTestFolder\MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid");
//            using (XmlReader reader = XmlReader.Create(sr, XSettings))
//            {
//                reader.Read();
//                while (reader.Read())
//                {
//                    switch (reader.NodeType)
//                    {
//                        case XmlNodeType.Element:
//                            Console.Write("<{0}>", reader.Name);
//                            // Based off of what is set as 
//                            if (reader.Name == "MzIdentML")
//                            {
//                                LcmsDataSet Set = new LcmsDataSet();
//                                Console.Write("id=\"{0}\"", reader.GetAttribute("id"));
//                            }
//                            else if (reader.Name == "DBSequence")
//                            {
//                                dbSeq = new DatabaseSequence();
                                
//                                dbSeq.Accession = reader.GetAttribute("accession");
//                                dbSeq.Length = Convert.ToInt32(reader.GetAttribute("length"));
//                                id = reader.GetAttribute("id");
//                                reader.ReadToDescendant("cvParam");
//                                dbSeq.ProteinDescription = reader.GetAttribute("value");
//                                database.Add(id, dbSeq);
//                                Console.WriteLine("{0} = {1}", reader.GetAttribute("id"), reader.GetAttribute("value"));
//                            }
//                            break;
//                        case XmlNodeType.Text:
//                            Console.Write(reader.Value);
//                            break;
//                        case XmlNodeType.CDATA:
//                            Console.Write("<![CDATA[{0}]]>", reader.Value);
//                            break;
//                        case XmlNodeType.ProcessingInstruction:
//                            Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
//                            break;
//                        case XmlNodeType.Comment:
//                            Console.Write("<!--{0}-->", reader.Value);
//                            break;
//                        case XmlNodeType.XmlDeclaration:
//                            Console.Write("<?xml version='1.0'?>");
//                            break;
//                        case XmlNodeType.Document:
//                            break;
//                        case XmlNodeType.DocumentType:
//                            Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
//                            break;
//                        case XmlNodeType.EntityReference:
//                            Console.Write(reader.Name);
//                            break;
//                        case XmlNodeType.EndElement:
//                            Console.Write("</{0}>", reader.Name);
//                            break;

//                        //case XmlNodeType.Whitespace:
//                        //    reader.ReadOuterXml();
//                        //    break;

//                        //case XmlNodeType.EndElement:
//                        //    Console.Write
//                        //    break;

//                        ////case: XmlNodeType.Element:


//                        //default:
//                        //    XmlNodeType type = reader.NodeType;
//                        //    buffer = reader.Value;
//                        //    Console.WriteLine("Node type{0} with value {1}", reader.NodeType, reader.Name);
//                        //    reader.Read();
//                        //    break;
//                    }
//                }

//            }
//        }
//    }
//}
