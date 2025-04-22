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

