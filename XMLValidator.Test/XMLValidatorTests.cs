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
        public void Valid_XML_With_Repeated_Elements()
        {
            // Test setup
            var validXML = "<People><Design><Code>hello world</Code><Code></Code></Design></People>";

            // Method under test
            var result = validator.DetermineSxml(validXML);

            // Assertions
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Extra_Opening_Element()
        {
            // Test setup
            var invalidXML = "<Design><Code>hello world</Code>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Extra_Closing_Element()
        {
            // Test setup
            var invalidXML = "<Code>hello world</Code></Design>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Invalid_Element_Closing_Order()
        {
            // Test setup
            var invalidXML = "<People><Design><Code>hello world</People></Code></Design>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Reversed_Order()
        {
            // Test setup
            var invalidXML = "</People>hello world<People>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Element_Left_Open()
        {
            // Test setup
            var invalidXML = "<Design><Code>hello world</Code></Design><People>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Closing_Bracket_Missing()
        {
            // Test setup
            var invalidXML = "<Design hello world</Design>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Malformed_Element()
        {
            // Test setup
            var invalidXML = "<<<> hello world</Design>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Leading_Text()
        {
            // Test setup
            var invalidXML = "leading text<Design>hello world</Design>";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Trailing_Text()
        {
            // Test setup
            var invalidXML = "<Design>hello world</Design>trailing text";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_String()
        {
            // Test setup
            var invalidXML = "";

            // Method under test
            var result = validator.DetermineSxml(invalidXML);

            // Assertions
            result.Should().BeFalse();
        }
    }
}