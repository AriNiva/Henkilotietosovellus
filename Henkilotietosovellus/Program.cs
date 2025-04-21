using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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
            Console.WriteLine("Yhteystiedot ovat...");
            TulostaOsoite(osoite);
            TulostaPostinumero(postinumero);
            TulostaPostitoimipaikka(postitoimipaikka);
            TulostaPuhelinnumero(puhelinnumero);
            TulostaSähköposti(sähköposti);
            Kysymys(sukupuoli);
        }

        /* syöte =  Console.ReadLine() ?? string.Empty;
         
           Luetaan käyttäjän syöte
           * Console.ReadLine() voi joskus palauttaa null, joten varmistetaan että tyhjää merkkijonoa käytetään sen sijaan.
           * Tämä estää ohjelmaa kaatumasta, jos syöte jostain syystä on null.*/

        private static string KysyKäyttäjänSukupuoli() 
        {
            string sukupuoli;

            do
            {
                Console.WriteLine("Kerro sukupuolesi. (Mies/Nainen).");
                sukupuoli = Console.ReadLine()?.Trim() ?? string.Empty;

                if (sukupuoli != "Mies" &&  sukupuoli != "Nainen") 
                {
                    Console.WriteLine("Virhe: Syötä Mies tai Nainen");
                }
            }
            while (sukupuoli != "Mies" && sukupuoli != "Nainen");
            
            return sukupuoli;
                
            

            /* ReadLine()?.Trim().ToLower()
               * ReadLine() lukee rivin.
               * ?.Trim() poistaa tyhjät merkit alusta ja lopusta, mutta ei kaadu jos null.
               * ToLower() muuntaa kaiken pieniksi kirjaimiksi. */

            /* return char.ToUpper(sukupuoli[0]) + sukupuoli.Substring(1);
               *palauttaa muotoillun version: "mies" -> "Mies" ja "nainen" -> "Nainen" */

        }

        private static string KysyKäyttäjänNimi()
        {
            string nimi;

            do
            {
                Console.WriteLine("Anna nimesi.");
                nimi = Console.ReadLine() ?? string.Empty;

                if (!OnKelvollinenNimi(nimi)) 
                {
                    Console.WriteLine("Virhe: Nimi saa sisältää vain kirjaimia ja välilyöntejä");
                }
            }
            while (!OnKelvollinenNimi(nimi));

            return nimi;
        }

        private static bool OnKelvollinenNimi(string nimi) 
        { 
            /* Tarkistaa, että nimi ei ole tyhjä ja sisältää vain kirjaimia tai välejä
               Nimi ei voi olla numero tai sekavaa syötettä esim. "!@#"
               char.IsLetter(c) varmistaa, että merkki on kirjain
               c == ' ' sallii välilyönnin (esim. Matti Meikäläinen)

            Jos kaikki merkit ovat hyväksyttäviä ja syöte ei ole tyhjä, palautetaan true
            */
            return !string.IsNullOrWhiteSpace(nimi) && nimi.All(c => char.IsLetter(c) || c == ' ');

        }

        private static string KysyKäyttäjänPuhelinnumero()
        { 
            string puhelinnumero;

            do
            {
                Console.WriteLine("Anna puhelinnumerosi.");
                puhelinnumero = Console.ReadLine() ?? string.Empty;

                if (!OnKelvollinenPuhelinnumero(puhelinnumero)) 
                {
                    Console.WriteLine("Virhe: Anna kelvollinen puhelinnumero");
                }
            }
            while (!OnKelvollinenPuhelinnumero(puhelinnumero));

            return puhelinnumero;
        }

        private static bool OnKelvollinenPuhelinnumero(string puhelinnumero) 
        {
            // Puhelinnumeron validointi Regexillä. Sallii kansainväliset ja suomalaiset muodot.
            var puhelinRegex = new Regex(@"^\+?[0-9\s\-]{6,20}$");
            return puhelinRegex.IsMatch(puhelinnumero);

            /* Selitys Regexistä:
               ^ ja $ määrittelevät alku- ja loppupisteet
               \+? sallii yhden + -merkin alussa(vapaaehtoinen)
               [0-9\s\-]{6,20} sallii numerot, välilyönnit ja viivat, 6-20 merkkiä pitkänä*/
        }

        private static string KysyKäyttäjänSähköposti() 
        { 
            string sähköposti;

            do
            {
                Console.WriteLine("Anna sähköpostiosoitteesi.");
                sähköposti = Console.ReadLine() ?? string.Empty;

                if (!OnKelvollinenSähköposti(sähköposti)) 
                {
                    Console.WriteLine("Virhe: Sähköpostiosoitteen tulee olla muodossa nimi@domain.com");
                }
            }
            while (!OnKelvollinenSähköposti(sähköposti));

            return sähköposti;
        }

        private static bool OnKelvollinenSähköposti(string sähköposti) 
        {
            var sähköpostiRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return sähköpostiRegex.IsMatch(sähköposti);

            /* Selitys Regexistä:
               ^[^@\s]+ -> alkaa yhdellä tai useammalla merkillä, joka ei ole @ tai välilyönti
               @ -> pakollinen @-merkki
               [^@\s] -> taas merkkejä, ei @ tai välilyönti
               \. -> piste
               [^@\s]+$ -> lopussa vielä merkkejä, ei @ tai välilyönti*/
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
            string postinumero;

            do
            {
                Console.WriteLine("Anna postinumerosi.");
                postinumero = Console.ReadLine() ?? string.Empty;

                if (!OnKelvollinenPostinumero(postinumero))
                {
                    Console.WriteLine("Virhe: Postinumeron tulee olla 5-numeroinen luku (esim. 00100).");
                }
            }
            while (!OnKelvollinenPostinumero(postinumero));

            return postinumero;
        }

        private static bool OnKelvollinenPostinumero(string postinumero) 
        {
            // Tarkistaa että postinumero on viisi numeroa
            var postinumeroRegex = new Regex(@"^\d{5}$");
            return postinumeroRegex.IsMatch(postinumero);
            
        }

        private static string KysyKäyttäjänPostitoimipaikka()
        {
            string postitoimipaikka;

            do
            {
                Console.WriteLine("Anna postitoimipaikkasi.");
                postitoimipaikka = Console.ReadLine() ?? string.Empty;

                if (!OnKelvollinenPostitoimipaikka(postitoimipaikka))
                {
                    Console.WriteLine("Virhe: Postitoimipaikan tulee sisältää vain kirjaimia, välilyötejä tai väliviivoja (esim. Nummi-Pusula).");
                }
            }
            while (!OnKelvollinenPostitoimipaikka(postitoimipaikka));

            return postitoimipaikka;
        }

        private static bool OnKelvollinenPostitoimipaikka(string postitoimipaikka) 
        {
            var postitoimipaikkaRegex = new Regex(@"^[A-Za-zÅÄÖåäö\s\-]+$");
            return postitoimipaikkaRegex.IsMatch (postitoimipaikka);

            /* Regex selitys:
               ^ ja $ = alku ja loppu

               [A-Za-zÅÄÖåäö\s\-]+ = sallitaan:
               isot ja pienet kirjaimet (myös suomen kielen ääkköset)
               \s = whitespace (välilyönti)
               \- = väliviiva
               + = yksi tai useampi sallittu merkki*/
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

                if (!int.TryParse(syöte, out ikä) || ikä < 0)
                {
                    Console.WriteLine("Virhe: Iän tulee olla positiivinen kokonaisluku.");
                }
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

        private static void TulostaOsoite(string osoite) 
        {
            Console.WriteLine($"Osoite: {osoite}");
        }

        private static void TulostaPostinumero(string postinumero)
        {
            Console.WriteLine($"Postinumero: {postinumero}");
        }

        private static void TulostaPostitoimipaikka(string postitoimipaikka)
        {
            Console.WriteLine($"Postitoimipaikka: {postitoimipaikka}");
        }

        private static void TulostaPuhelinnumero(string puhelinnumero)
        {
            Console.WriteLine($"Puhelinnumero: {puhelinnumero}");
        }

        private static void TulostaSähköposti(string sähköposti)
        {
            Console.WriteLine($"Sähköposti: {sähköposti}");
        }
    }
}






            
                    
                    


            


