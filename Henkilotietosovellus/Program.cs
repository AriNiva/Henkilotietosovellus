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
            var puhelinnumero = KysyKäyttäjänPuhelinnumero();
            var sähköposti = KysyKäyttäjänSähköposti();
            var osoite = KysyKäyttäjänOsoite();
            var postinumero = KysyKäyttäjänPostinumero();
            var postitoimipaikka = KysyKäyttäjänPostitoimipaikka();
            var ikä = KysyKäyttäjänIkä();
            Tervehdys(nimi, ikä, sukupuoli);
            MuodostaIkäLuokitus(ikä);
            Kysymys(sukupuoli);
        }

        /* syöte =  Console.ReadLine() ?? string.Empty;
         
           Luetaan käyttäjän syöte
           * Console.ReadLine() voi joskus palauttaa null, joten varmistetaan että tyhjää merkkijonoa käytetään sen sijaan.
           * Tämä estää ohjelmaa kaatumasta, jos syöte jostain syystä on null.*/

        private static string KysyKäyttäjänSukupuoli() 
        {
            string sukupuoli = string.Empty;

            while (string.IsNullOrWhiteSpace(sukupuoli))
            {
                Console.WriteLine("Kerro sukupuolesi. (Mies/Nainen).");
                sukupuoli = Console.ReadLine() ?? string.Empty;
            }
            return sukupuoli;
        }

        private static string KysyKäyttäjänNimi()
        {
            string nimi = string.Empty;

            while (string.IsNullOrWhiteSpace(nimi))
            {
                Console.WriteLine("Anna nimesi.");
                nimi = Console.ReadLine() ?? string.Empty;
            }

            return nimi;
        }

        private static string KysyKäyttäjänPuhelinnumero()
        { 
            string puhelinnumero = string.Empty;

            while (string.IsNullOrWhiteSpace(puhelinnumero)) 
            {
                Console.WriteLine("Anna puhelinnumerosi.");
                puhelinnumero = Console.ReadLine() ?? string.Empty;
            }

            return puhelinnumero;
        }

        private static string KysyKäyttäjänSähköposti() 
        { 
            string sähköposti = string.Empty;

            while (string.IsNullOrWhiteSpace(sähköposti)) 
            {
                Console.WriteLine("Anna sähköpostiosoitteesi.");
                sähköposti = Console.ReadLine() ?? string.Empty;
            }

            return sähköposti;
        }

        private static string KysyKäyttäjänOsoite() 
        { 
            string osoite = string.Empty;

            while (string.IsNullOrWhiteSpace(osoite)) 
            {
                Console.WriteLine("Anna osoitteesi.");
                osoite = Console.ReadLine() ?? string.Empty;
            }

            return osoite;
        }

        private static string KysyKäyttäjänPostinumero() 
        { 
            string postinumero = string.Empty;

            while (string.IsNullOrWhiteSpace(postinumero)) 
            {
                Console.WriteLine("Anna postinumerosi.");
                postinumero = Console.ReadLine() ?? string.Empty;
            }

            return postinumero;
        }

        private static string KysyKäyttäjänPostitoimipaikka()
        {
            string postitoimipaikka = string.Empty;

            while (string.IsNullOrWhiteSpace (postitoimipaikka)) 
            {
                Console.WriteLine("Anna postitoimipaikkasi.");
                postitoimipaikka = Console.ReadLine() ?? string.Empty;
            }

            return postitoimipaikka;
        }

        private static int KysyKäyttäjänIkä() 
        {
            int ikä;
            string syöte;

            /* do/while loop is a variant of the while loop. This loop will execute the code block once, before checking if the condition is true,
               then it will repeat the loop as long as the condition is true.*/

            /* Silmikka jatkuu niin kauan kuin:
               * (!int.TryParse(syöte, out ikä) epäonnistuu (eli käyttäjä kirjoitti esim.kirjaimia)
               * tai iän arvo on alle 0 (esim -5)
             
             * int.TryParse yrittää muuntaa syöte-muuttujan kokonaisluvuksi, ja tallentaa sen ikä-muuttujaan.
               Jos muunnos ei onnistu, metodi palauttaa false.*/

            do
            {
                Console.WriteLine("Anna ikäsi.");
                syöte = Console.ReadLine() ?? string.Empty;
            }
            while (!int.TryParse(syöte, out ikä) || ikä < 0);

            return ikä;
        }

        private static void MuodostaIkäLuokitus(int ikä) 
        {
            string ikäluokitus;
            
            if (ikä < 1)
            {
                ikäluokitus = "Olet vauva";
            }
            else if (ikä < 7)
            {
                ikäluokitus = "Olet alle kouluikäinen";
            }
            else if (ikä < 18)
            {
                ikäluokitus = "Olet alaikäinen";  
            }
            else if (ikä < 66)
            {
                ikäluokitus = "Olet täysi-ikäinen";
            }
            else 
            {
                ikäluokitus = "Olet seniori ja ehkä eläkkeellä";
            }

            Console.WriteLine(ikäluokitus);
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

        private static void Tervehdys(string nimi, int ikä, string sukupuoli) 
        { 
            Console.WriteLine($"Hei {nimi}!. Olet {ikä}-vuotias {sukupuoli}.");
        }
    }
}






            
                    
                    


            


