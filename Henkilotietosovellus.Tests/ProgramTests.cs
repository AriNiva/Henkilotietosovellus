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
        public void Main_AsksForUsersSex_WhenExecuted() 
        {
            SetupUserResponses("Mies", "Ari");
            var expectedPrompt = "Kerro sukupuolesi. (Mies/Nainen).";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.That(outputLines[1], Is.EqualTo(expectedPrompt));
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

