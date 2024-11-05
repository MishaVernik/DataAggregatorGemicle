using DataAggregatorGemicle.Utils;
using Shouldly;

namespace DAG.Tests
{
    [TestFixture]
    public class ClientCodeGeneratorTests
    {
        [Test]
        public void GenerateClientCode_ValidInputs_ShouldGenerateCorrectCode()
        {
            string firstName = "Jack";
            string lastName = "Sparrow";
            string organizationName = "Jack Sparrow Warship";
            string expectedCode = "KCA-RAP-JSW";

            string actualCode = ClientCodeGenerator.Generate(firstName, lastName, organizationName);

            actualCode.ShouldBe(expectedCode);
        }

        [Test]
        public void GenerateClientCode_ShortFirstName_ShouldGenerateCorrectCode()
        {
            string firstName = "Jo";
            string lastName = "Doe";
            string organizationName = "Pirate Company";
            string expectedCode = "O-EO-PC";

            string actualCode = ClientCodeGenerator.Generate(firstName, lastName, organizationName);

            actualCode.ShouldBe(expectedCode);
        }

        [Test]
        public void GenerateClientCode_ShortLastName_ShouldGenerateCorrectCode()
        {
            string firstName = "Elizabeth";
            string lastName = "Sw";
            string organizationName = "Pirate Alliance";
            string expectedCode = "ZIL-W-PA";

            string actualCode = ClientCodeGenerator.Generate(firstName, lastName, organizationName);

            actualCode.ShouldBe(expectedCode);
        }

        [Test]
        public void GenerateClientCode_EmptyOrganization_ShouldGenerateCorrectCode()
        {
            string firstName = "Jack";
            string lastName = "Sparrow";
            string organizationName = "";
            string expectedCode = "KCA-RAP-";

            string actualCode = ClientCodeGenerator.Generate(firstName, lastName, organizationName);

            actualCode.ShouldBe(expectedCode);
        }
    }
}
