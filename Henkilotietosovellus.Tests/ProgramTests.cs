using Moq;
using System.Text;

namespace Henkilotietosovellus.Tests
{
    public class ProgramTests
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

        // Huom. Testeissä täytyy antaa kaikki syötteet joita ohjelma tarvitsee.
        [Test]
        public void Main_Kysyy_Käyttäjän_Sukupuolen_Vastaus_Mies() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");
            var expectedPrompt = "Kerro sukupuolesi. (Mies/Nainen).";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[1], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Sukupuolen_Vastaus_Nainen()
        {
            SetupUserResponses("Nainen", "Anna", "0401234567", "anna@posti.fi", "Tie 1", "90630", "Oulu", "70");

            var expectedPrompt = "Kerro sukupuolesi. (Mies/Nainen).";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[1], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Sukupuolen_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin() 
        {
            SetupUserResponses("kissa", "Nainen", "Anna", "0401234567", "anna@posti.fi", "Tie 1", "90630", "Oulu", "70");

            var expectedPrompt = "Virhe: Syötä Mies tai Nainen";

            var outputLines = RunMainAndGetConsoleOutput();

            var sukupuoliPromptCount = outputLines.Count(l => l == "Kerro sukupuolesi. (Mies/Nainen).");

            Assert.Multiple(() =>
            {
                Assert.That(outputLines[2], Is.EqualTo(expectedPrompt));
                Assert.That(sukupuoliPromptCount, Is.EqualTo(2));
            });
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Nimen_Syöte_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Teemu Selänne", "0401234567", "teemu.selanne@mail.com", "Tie 1", "90630", "Oulu", "16");

            var expectedPrompt = "Anna nimesi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[2], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Nimen_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin()
        {
            // HUOMIO! Vaihtoehtoinen tapa testata uudelleenkysyminen ja virheviestin näyttäminen
            SetupUserResponses("Mies", "4", "Teemu Selänne", "0401234567", "teemu.selanne@mail.com", "Tie 1", "90630", "Oulu", "16");

            var outputLines = RunMainAndGetConsoleOutput();

            var nimiPromptCount = outputLines.Count(l => l == "Anna nimesi.");

            Assert.That(nimiPromptCount, Is.EqualTo(2));
            Assert.That(outputLines, Does.Contain("Virhe: Nimi saa sisältää vain kirjaimia, väliviivoja ja välilyöntejä"));
        }

        [Test] 
        public void Main_Kysyy_Käyttäjän_Puhelinnumeron_Syöte_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna puhelinnumerosi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[3], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Puhelinnumeron_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Virhe: Anna kelvollinen puhelinnumero";

            var outputLines = RunMainAndGetConsoleOutput();

            var puhelinnumeroPromptCount = outputLines.Count(l => l == "Anna puhelinnumerosi.");

            Assert.That(outputLines[4], Is.EqualTo(expectedPrompt));
            Assert.That(puhelinnumeroPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Sähköpostin_Syöte_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna sähköpostiosoitteesi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[4], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Sähköpostin_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "sahkoposti", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Virhe: Sähköpostiosoitteen tulee olla muodossa nimi@domain.com";

            var outputLines = RunMainAndGetConsoleOutput();

            var sähköpostiPromptCount = outputLines.Count(l => l == "Anna sähköpostiosoitteesi.");

            Assert.That(outputLines[5], Is.EqualTo(expectedPrompt));
            Assert.That(sähköpostiPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Osoitteen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna osoitteesi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[5], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Postinumeron_Syöte_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna postinumerosi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[6], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Postinumeron_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "X", "90630", "Oulu", "40");

            var expectedPrompt = "Virhe: Postinumeron tulee olla 5-numeroinen luku (esim. 00100).";

            var outputLines = RunMainAndGetConsoleOutput();

            var postinumeroPromptCount = outputLines.Count(l => l == "Anna postinumerosi.");

            Assert.That(outputLines[7], Is.EqualTo(expectedPrompt));
            Assert.That(postinumeroPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Postitoimipaikan_Syöte_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna postitoimipaikkasi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[7], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Postitoimipaikan_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "@4a", "Oulu", "40");

            var expectedPrompt = "Virhe: Postitoimipaikan tulee sisältää vain kirjaimia, välilyötejä tai väliviivoja (esim. Nummi-Pusula).";

            var outputLines = RunMainAndGetConsoleOutput();

            var postitoimipaikkaPromptCount = outputLines.Count(l => l == "Anna postitoimipaikkasi.");

            Assert.That(outputLines[8], Is.EqualTo(expectedPrompt));
            Assert.That(postitoimipaikkaPromptCount, Is.EqualTo(2));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Ikää_Syöte_Kelvollinen() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "40");

            var expectedPrompt = "Anna ikäsi.";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[8], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Main_Kysyy_Käyttäjän_Ikää_Uudelleen_Jos_Syöte_Ei_Kelvollinen_Ja_Näyttää_Virheviestin() 
        {
            SetupUserResponses("Mies", "Ari", "0401234567", "ari@posti.fi", "Tie 1", "90630", "Oulu", "kahdeksantoista", "40");

            var expectedPrompt = "Virhe: Iän tulee olla positiivinen kokonaisluku.";

            var outputLines = RunMainAndGetConsoleOutput();

            var ikäPromptCount = outputLines.Count(l => l == "Anna ikäsi.");

            Assert.That(outputLines[9], Is.EqualTo(expectedPrompt));
            Assert.That(ikäPromptCount, Is.EqualTo(2));
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

