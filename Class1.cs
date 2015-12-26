using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApplication1
{
//TODO Fjern denne : test kommentar til at illustrere om GitHub virker
    class Datahandler
    {
        int AntalKnapper = 4;
        int maxsymptomantal;
        public XElement doc;
        public int AntalSpørgsmål=4;
        public List<String> sygdomme;
        public List<String> kandidatsymptomer;
        //public List<KeyValuePair<String,List<String>>> korrektesymptomer;
        public Dictionary<String, List<String>> korrektesymptomer; 
        public List<String> TotalListeKorrekteSymptomer;  
        private Random rnd;
        private bool ErIgangMedSession= false;
        private int sygdomNr;

        public Datahandler(int antal)
        {
            maxsymptomantal = antal;
        }

        public void indlæsdata(string stinavn)
        {
            rnd = new Random();
            doc = XElement.Load(stinavn);
        }

        public void VælgSygdomAtSpørgeOm()
        {
            if (!ErIgangMedSession)
            {
                sygdomNr = 0;

                sygdomme = new List<String>();
                korrektesymptomer= new Dictionary<string,List<string>>();
                for (int i = 0; i < AntalSpørgsmål; i++)
                {
                    var sygdom = doc.Elements("sygdom").OrderBy(elt => rnd.Next()).ToList()[0].Attribute("navn").Value;
                    if (!sygdomme.Contains(sygdom))
                    {
                        sygdomme.Add(sygdom);
                        korrektesymptomer.Add(sygdom,
                                   doc.Elements("sygdom").Where(asymp => asymp.Attribute("navn").
                                       Value == sygdomme.Last()).ToList().
                                       SelectMany(syg => syg.Elements("symptom").ToList(), (a, b) => b.Attribute("navn").Value).ToList());
                    }
                }
                korrektesymptomer.ToDictionary(a => a.Key, a => a.Value);
                TotalListeKorrekteSymptomer = korrektesymptomer.ToList().ToDictionary(a => a.Key, a => a.Value).Select(a => a.Value).SelectMany(a => a).Distinct().ToList();
                kandidatsymptomer = GenererListeAfkandidatsymptomer();
                ErIgangMedSession = true;
            }
            else
            {
                
            }
        }

        List<string> GenererListeAfkandidatsymptomer()
        {
            List<string> AlleSymptomer = doc.Elements("sygdom").ToList<XElement>().SelectMany(syg => syg.Elements("symptom").ToList(), (a, b) => b.Attribute("navn").Value).ToList();

            var SymptomerIkkeFordenneSygdom = AlleSymptomer.Except(TotalListeKorrekteSymptomer).OrderBy(sfds => rnd.Next()); ;

            var maxantalsymptomerfralister = Math.Min((decimal)SymptomerIkkeFordenneSygdom.Count(), (decimal)korrektesymptomer.Count());
            List<string> displaysymptomer = new List<string>() {TotalListeKorrekteSymptomer.ToList()[0] };

            //for (int i = 1; i < AntalKnapper; i++)// Math.Min((decimal)maxsymptomantal, (decimal)maxantalsymptomerfralister); i++)
            //{
            //    if (rnd.Next(2) == 0)
            //        displaysymptomer.Add(TotalListeKorrekteSymptomer.ToList()[i]);
            //    else
            //        displaysymptomer.Add(SymptomerIkkeFordenneSygdom.ToList()[i]);
            //}

            displaysymptomer =  displaysymptomer.Concat(TotalListeKorrekteSymptomer).ToList();
            displaysymptomer = displaysymptomer.Concat(SymptomerIkkeFordenneSygdom).ToList();

            displaysymptomer = displaysymptomer.OrderBy(ds => rnd.Next(1000)).ToList();

            return new List<string>{displaysymptomer[1], displaysymptomer[2], displaysymptomer[3], displaysymptomer[4]};//(1:4);
        }

        public string NuværendeSygdom()
        {
            return sygdomme[sygdomNr];
        }

        public void GåTilNæsteSygdom()
        {
            sygdomNr += 1;
            if (sygdomNr == AntalSpørgsmål - 1)
            {
                ErIgangMedSession = false;
            }
        }
    }
}
