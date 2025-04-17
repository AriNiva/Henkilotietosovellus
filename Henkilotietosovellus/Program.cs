using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Henkilotietosovellus.Tests")]
namespace Henkilotietosovellus
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Henkilötietosovellus, versio 1.0.");
            var sukupuoli = KysyKäyttäjänSukupuoli();
            var nimi = KysyKäyttäjänNimi();
            Tervehdys(nimi);
            Kysymys(sukupuoli);
        }

        private static string KysyKäyttäjänSukupuoli() 
        {
            string sukupuoli = string.Empty;

            while (string.IsNullOrWhiteSpace(sukupuoli))
            {
                Console.Write("Kerro sukupuolesi. (Mies/Nainen). ");
                sukupuoli = Console.ReadLine() ?? string.Empty;
            }
            return sukupuoli;
        }

        private static void Kysymys(string sukupuoli) 
        {
            string kysymys;

            if (sukupuoli == "Mies")
            {
                kysymys = "Mitä jäbä duunaa?";
            }
            else 
            {
                kysymys = "Mitä mimmi duunaa?";
            }

            Console.WriteLine(kysymys);
        }

        private static string KysyKäyttäjänNimi() 
        { 
            string nimi = string.Empty;

            while (string.IsNullOrWhiteSpace(nimi)) 
            {
                Console.Write("Anna nimesi ");
                nimi = Console.ReadLine() ?? string.Empty;
            }

            return nimi;
        }

        private static void Tervehdys(string nimi) 
        { 
            Console.WriteLine($"Hei {nimi}");
        }
    }
}



            
                    
                    


            


