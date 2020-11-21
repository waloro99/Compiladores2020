using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_2
{
    public class LR1
    {
        public List<Production> Grammar { get; set; }
        public List<Production> CanonicalCollection { get; set; }
        public List<Case> AnalysisTable{ get; set; }

        private string PathConfig { get; set; }

        private string PathTable { get; set; }

        public LR1()
        {
            Grammar = new List<Production>();
            AnalysisTable = new List<Case>();
            PathConfig = Environment.CurrentDirectory + "\\config.txt";
            PathTable = Environment.CurrentDirectory + "\\Tabla.csv";

            if (File.Exists(PathConfig))
            {
                setGrammar(PathConfig);

                if (File.Exists(PathTable))
                {
                    setAnalysisTable(PathTable);

                    if (!VerifyTable())
                    {
                        throw new Exception("Error to read 'Analysis Table' file");
                    }
                }
                else
                {
                    throw new Exception("Analysis Table file not found");
                }
            }
            else
            {
                throw new Exception("Configuration file not found");
            }
        }

        private void setGrammar(string path)
        {
            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(file))
                {
                    var count = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        var parent = line.Split('¬')[0];
                        var production = new Production(count, parent);

                        foreach (var elementString in line.Split('¬')[1].Split(' '))
                        {
                            if (!elementString.Equals(string.Empty))
                            {
                                Element elementObject;

                                if (elementString.Contains("_"))
                                {
                                    elementObject = new Element(elementString.Remove(0, 1));
                                    elementObject.Terminal = true;
                                }
                                else
                                {
                                    elementObject = new Element(elementString);
                                }

                                if (elementString == "''")
                                {
                                    elementObject.Caracter = "";
                                }

                                production.Lista_elementos.Add(elementObject);
                            }
                        }

                        Grammar.Add(production);
                        count++;
                    }
                }
            }
        }

        private void verifyNumberState(ref int num_state, int new_num_state)
        {
            if (num_state < new_num_state)
            {
                num_state = new_num_state;
            }
        }

        private bool searchExisting(ref Production production)
        {
            var found = false;

            if (CanonicalCollection.Count == 0)
            {
                return found;
            }

            foreach (var state in CanonicalCollection)
            {
                if ((production.Padre == state.Padre) && (production.Caracter_analizar == state.Caracter_analizar))
                {
                    for (int i = 0; i < state.Lista_elementos.Count; i++)
                    {
                        if (state.Lista_elementos[i].Actual != production.Lista_elementos[i].Actual)
                        {
                            if (state.Lista_elementos[i] == production.Lista_elementos[i])
                            {
                                found = true;
                            }
                            else
                            {
                                found = false;
                                i = state.Lista_elementos.Count;
                            }
                        }
                    }

                    if (found)
                    {
                        production.Num_estado_nuevo = state.Num_estado;
                        return found;
                    }
                }
            }


            return found;
        }

        private void setAnalysisTable(string path)
        {
            var lines = new List<string>();

            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }
                }
            }

            var firstLineContent = lines[0].Split('|').ToList();
            
            foreach (var line in firstLineContent)
            {
                if (line != "state")
                {
                    AnalysisTable.Add(new Case(line));
                }
            }

            lines.RemoveAt(0);

            for (int i = 0; i < AnalysisTable.Count; i++)
            {
                foreach (var item in lines)
                {
                    var action = item.Split('|')[i];

                    if (action == "")
                    {
                        AnalysisTable[i].Actions.Add("-1");
                    }
                    else
                    {
                        AnalysisTable[i].Actions.Add(action);
                    }
                }
            }
        }

        private bool VerifyTable()
        {
            var len = AnalysisTable[0].Actions.Count;

            foreach (var row in AnalysisTable)
            {
                if (row.Actions.Count != len)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetAction(int state, Type type, ref List<string> listaOpciones)
        {
            var itemtoSearch = "";
            var action = "-1";

            switch (type.description)
            {
                case "T_Identifier":
                    itemtoSearch = "ident";
                    break;

                case "T_Operator":
                    itemtoSearch = type.cadena;
                    break;

                case "$":
                    itemtoSearch = type.cadena;
                    break;

                //case "T_interface":
                //    itemtoSearch = "InterfaceDecl";
                //    break;

                default:
                    if (type.description.Contains("T_"))
                    {
                        itemtoSearch = type.description.Replace("T_", "");
                    }
                    else
                    {
                        itemtoSearch = type.description;
                    }
                    break;
            }

            foreach (var row in AnalysisTable)
            {
                if (row.Match.Equals(itemtoSearch))
                {
                    var result = row.Actions[state];

                    if (result.Contains("/"))
                    {
                        listaOpciones.Clear(); //por si se quedo un dato metido anteriormente
                        listaOpciones = result.Split('/').ToList();
                        return "-2";
                    }

                    return result;
                   
                }   
            }

            return action; //d39
        }

        public Production GetState(int index)
        {
            return Grammar[index];
        }
    }
}
