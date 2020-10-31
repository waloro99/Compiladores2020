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

        private string Path { get; set; }

        public LR1()
        {
            Grammar = new List<Production>();
            Path = Environment.CurrentDirectory + "\\config.txt";

            if (File.Exists(Path))
            {
                setGrammar(Path);
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
                    var count = 1;

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
    }
}
