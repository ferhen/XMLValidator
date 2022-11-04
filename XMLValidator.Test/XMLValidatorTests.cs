using FluentAssertions;
using XMLValidator.Core;

namespace XMLValidator.Test
{
    [TestClass]
    public class XMLValidatorTests
    {
        private readonly Validator validator = new();

        [TestMethod]
        public void Valid_XML()
        {
            // Test setup
            var validXML = "<Design><Code>hello world</Code></Design>";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Extra_Opening_Tag()
        {
            // Test setup
            var validXML = "<Design><Code>hello world</Code>";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Extra_Closing_Tag()
        {
            // Test setup
            var validXML = "<Code>hello world</Code></Design>";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Invalid_Tag_Closing_Order()
        {
            // Test setup
            var validXML = "<People><Design><Code>hello world</People></Code></Design>";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Malformed_Tag()
        {
            // Test setup
            var validXML = "<Design hello world</Design>";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_String()
        {
            // Test setup
            var validXML = "";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeFalse();
        }
    }
}