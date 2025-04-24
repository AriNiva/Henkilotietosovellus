using Moq;
using System.Text;

namespace Henkilotietosovellus.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        // Kenttien m��rittely

        StringBuilder _ConsoleOutput; // Tallentaa kaiken tekstin, jonka ohjelma kirjoittaa Console.WriteLine-kutsuilla.

        Mock<TextReader> _ConsoleInput; // Simuloidaan Console.ReadLine()-sy�tett�. T�m� on ns. mock-input.

        [SetUp]
        public void Setup()
        {
            // Luodaan StringBuilder-olio tallentamaan konsoliin kirjoitettavat tekstit. 
            // T�m� toimii ohjelman "virtuaalisena konsolipuskurina" johon Console.WriteLine-tulostus menee.
            _ConsoleOutput = new StringBuilder();

            // Luodaan StringWriter, joka kirjoittaa tekstin suoraan _ConsoleOutput-merkkijonopuskuriin.
            // T�m� mahdollistaa Console.WriteLine-kutsujen tallentamisen StringBuilderiin.
            var consoleOutputWriter = new StringWriter(_ConsoleOutput);

            // Luodaan mock-objekti TextReader-luokasta, (joka on Console.In:n tyyppi) joka simuloi k�ytt�j�n sy�tett� Console.ReadLine()-kutsuille.
            // Mocking mahdollistaa testauksen ilman, ett� k�ytt�j� oikeasti antaa sy�tett�.
            _ConsoleInput = new Mock<TextReader>();

            // Asetetaan ohjelman oletusulostulovirta Console.WriteLine-kutsuille niin, ett� ne kirjoitetaan _ConsoleOutput-muuttujaan
            // eik� suoraan konsoliin. T�m� mahdollistaa tulosteen tarkistamisen testiss�.
            Console.SetOut(consoleOutputWriter);

            // Asetetaan ohjelman oletussy�tevirta niin, ett� Console.ReadLine() lukee _ConsoleInput-mock-oliosta
            // eik� suoraan k�ytt�j�n kirjoittamasta sy�tteest�. T�m� auttaa testien suorittamisessa ilman manuaalista sy�tett�.
            Console.SetIn(_ConsoleInput.Object);
        }

        // Huom. Testeiss� t�ytyy antaa kaikki sy�tteet joita ohjelma tarvitsee.
        [Test]
        public void Main_Kysyy_K�ytt�j�n_Sukupuolen_Vastaus_Mies() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");
            var expectedPrompt = "Kerro sukupuolesi. (Mies/Nainen).";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[1], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Sukupuolen_Vastaus_Nainen()
        {
            SetupUserResponses("Nainen", "Anna", "0401234567", "anna@posti.fi", "Tie 1", "90630", "Oulu", "70");

            var expectedPrompt = "Kerro sukupuolesi. (Mies/Nainen).";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[1], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Sukupuolen_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin() 
        {
            SetupUserResponses("kissa", "Nainen", "Anna", "0401234567", "anna@posti.fi", "Tie 1", "90630", "Oulu", "70");

            var expectedPrompt = "Virhe: Sy�t� Mies tai Nainen";

            var outputLines = RunMainAndGetConsoleOutput();

            var sukupuoliPromptCount = outputLines.Count(l => l == "Kerro sukupuolesi. (Mies/Nainen).");

            Assert.Multiple(() =>
            {
                Assert.That(outputLines[2], Is.EqualTo(expectedPrompt));
                Assert.That(sukupuoliPromptCount, Is.EqualTo(2));
            });
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Nimen_Sy�te_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Teemu Sel�nne", "0401234567", "teemu.selanne@mail.com", "Tie 1", "90630", "Oulu", "16");

            var expectedPrompt = "Anna nimesi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[2], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Nimen_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin()
        {
            // HUOMIO! Vaihtoehtoinen tapa testata uudelleenkysyminen ja virheviestin n�ytt�minen
            SetupUserResponses("Mies", "4", "Teemu Sel�nne", "0401234567", "teemu.selanne@mail.com", "Tie 1", "90630", "Oulu", "16");

            var outputLines = RunMainAndGetConsoleOutput();

            var nimiPromptCount = outputLines.Count(l => l == "Anna nimesi.");

            Assert.That(nimiPromptCount, Is.EqualTo(2));
            Assert.That(outputLines, Does.Contain("Virhe: Nimi saa sis�lt�� vain kirjaimia, v�liviivoja ja v�lily�ntej�"));
        }

        [Test] 
        public void Main_Kysyy_K�ytt�j�n_Puhelinnumeron_Sy�te_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna puhelinnumerosi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[3], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Puhelinnumeron_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Virhe: Anna kelvollinen puhelinnumero";

            var outputLines = RunMainAndGetConsoleOutput();

            var puhelinnumeroPromptCount = outputLines.Count(l => l == "Anna puhelinnumerosi.");

            Assert.That(outputLines[4], Is.EqualTo(expectedPrompt));
            Assert.That(puhelinnumeroPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_S�hk�postin_Sy�te_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna s�hk�postiosoitteesi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[4], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_S�hk�postin_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "sahkoposti", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Virhe: S�hk�postiosoitteen tulee olla muodossa nimi@domain.com";

            var outputLines = RunMainAndGetConsoleOutput();

            var s�hk�postiPromptCount = outputLines.Count(l => l == "Anna s�hk�postiosoitteesi.");

            Assert.That(outputLines[5], Is.EqualTo(expectedPrompt));
            Assert.That(s�hk�postiPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Osoitteen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna osoitteesi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[5], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Postinumeron_Sy�te_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna postinumerosi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[6], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Postinumeron_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "X", "90630", "Oulu", "40");

            var expectedPrompt = "Virhe: Postinumeron tulee olla 5-numeroinen luku (esim. 00100).";

            var outputLines = RunMainAndGetConsoleOutput();

            var postinumeroPromptCount = outputLines.Count(l => l == "Anna postinumerosi.");

            Assert.That(outputLines[7], Is.EqualTo(expectedPrompt));
            Assert.That(postinumeroPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Postitoimipaikan_Sy�te_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna postitoimipaikkasi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[7], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Postitoimipaikan_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "@4a", "Oulu", "40");

            var expectedPrompt = "Virhe: Postitoimipaikan tulee sis�lt�� vain kirjaimia, v�lily�tej� tai v�liviivoja (esim. Nummi-Pusula).";

            var outputLines = RunMainAndGetConsoleOutput();

            var postitoimipaikkaPromptCount = outputLines.Count(l => l == "Anna postitoimipaikkasi.");

            Assert.That(outputLines[8], Is.EqualTo(expectedPrompt));
            Assert.That(postitoimipaikkaPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Ik��_Sy�te_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna ik�si.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[8], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_K�ytt�j�n_Ik��_Uudelleen_Jos_Sy�te_Ei_Kelvollinen_Ja_N�ytt��_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "kahdeksantoista", "40");

            var expectedPrompt = "Virhe: I�n tulee olla positiivinen kokonaisluku.";

            var outputLines = RunMainAndGetConsoleOutput();

            var ik�PromptCount = outputLines.Count(l => l == "Anna ik�si.");

            Assert.That(outputLines[9], Is.EqualTo(expectedPrompt));
            Assert.That(ik�PromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Kysymys_Tulostaa_Mit�J�b�Duunaa_Jos_Sukupuoli_On_Mies() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Mit� j�b� duunaa?";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[17], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Kysymys_Tulostaa_Mit�MimmiDuunaa_Jos_Sukupuoli_On_Nainen()
        {
            SetupUserResponses("Nainen", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Mit� mimmi duunaa?";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[17], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_KaikkiKelvollisetSy�tteet_TulostusOikein()
        {
            // Sy�tet��n vain kelvolliset arvot
            SetupUserResponses("Nainen", "Anna Virtanen", "0501234567", "anna@example.com", "Katu 5", "00100", "Helsinki", "30");

            var outputLines = RunMainAndGetConsoleOutput();

            // Tarkistetaan ett� ei tule yht��n virheilmoitusta
            Assert.That(outputLines.Any(l => l.StartsWith("Virhe")), Is.False);

            // Tarkistetaan ett� kaikki tulostukset ovat mukana
            Assert.Multiple(() =>
            {
                Assert.That(outputLines, Does.Contain("Henkil�tietosovellus, versio 1.0."));
                Assert.That(outputLines, Does.Contain("Kerro sukupuolesi. (Mies/Nainen)."));
                Assert.That(outputLines, Does.Contain("Anna nimesi."));
                Assert.That(outputLines, Does.Contain("Anna puhelinnumerosi."));
                Assert.That(outputLines, Does.Contain("Anna s�hk�postiosoitteesi."));
                Assert.That(outputLines, Does.Contain("Anna osoitteesi."));
                Assert.That(outputLines, Does.Contain("Anna postinumerosi."));
                Assert.That(outputLines, Does.Contain("Anna postitoimipaikkasi."));
                Assert.That(outputLines, Does.Contain("Anna ik�si."));
                Assert.That(outputLines, Does.Contain("Hei Anna Virtanen!. Olet 30-vuotias Nainen."));
                Assert.That(outputLines, Does.Contain("Olet t�ysi-ik�inen"));
                Assert.That(outputLines, Does.Contain("Yhteystiedot ovat..."));
                Assert.That(outputLines, Does.Contain("Mit� mimmi duunaa?"));
            });
        }

        // Ajaa p��ohjelman, ja palauttaa sen tulosteen riveitt�in.
        private string[] RunMainAndGetConsoleOutput() 
        {
            Program.Main(default);
            return _ConsoleOutput.ToString().Split("\r\n");
        }

        // Metodi asettaa simuloidut vastaukset k�ytt�j�lt�
         private MockSequence SetupUserResponses(params string[] userResponses) 
        { 
            var sequence = new MockSequence();
            foreach (var response in userResponses) 
                _ConsoleInput.InSequence(sequence).Setup(mockConsoleReader => mockConsoleReader.ReadLine()).Returns(response);
            return sequence;
            
        }
    }

    [TestFixture]
    public class IkaLuokitusTests 
    {
        StringBuilder _ConsoleOutput;
        Mock<TextReader> _ConsoleInput;

        [SetUp]
        public void Setup()
        {
            _ConsoleOutput = new StringBuilder();
            var consoleOutputWriter = new StringWriter(_ConsoleOutput);
            _ConsoleInput = new Mock<TextReader>();
            Console.SetOut(consoleOutputWriter);
            Console.SetIn(_ConsoleInput.Object);
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletVauva_Jos_Ik�_Alle_1()
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "0");

            var expectedPrompt = "Olet vauva";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletAlleKouluik�inen_Jos_Ik�_On_1()
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "1");

            var expectedPrompt = "Olet alle kouluik�inen";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletAlleKouluik�inen_Jos_Ik�_Alle_7() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "6");

            var expectedPrompt = "Olet alle kouluik�inen";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletAlaik�inen_Jos_Ik�_On_7()
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "7");

            var expectedPrompt = "Olet alaik�inen";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletAlaik�inen_Jos_Ik�_Alle_18() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "17");

            var expectedPrompt = "Olet alaik�inen";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletT�ysiIk�inen_Jos_Ik�_On_18()
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "18");

            var expectedPrompt = "Olet t�ysi-ik�inen";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletT�ysiIk�inen_Jos_Ik�_Alle_66() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "65");

            var expectedPrompt = "Olet t�ysi-ik�inen";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void MuodostaIk�Luokitus_Tulostaa_OletSeniori_Jos_Ik�_Yli_65() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "66");

            var expectedPrompt = "Olet seniori ja ehk� el�kkeell�";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[10], Is.EqualTo(expectedPrompt));
        }

        private string[] RunMainAndGetConsoleOutput()
        {
            Program.Main(default);
            return _ConsoleOutput.ToString().Split("\r\n");
        }

        private MockSequence SetupUserResponses(params string[] userResponses)
        {
            var sequence = new MockSequence();
            foreach (var response in userResponses)
                _ConsoleInput.InSequence(sequence).Setup(x => x.ReadLine()).Returns(response);
            return sequence;

        }
    }
}

