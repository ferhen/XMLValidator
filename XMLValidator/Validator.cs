namespace XMLValidator.Core
{
    public class Validator
    {
        public bool DetermineSxml(string sxml)
        {
            var parser = new ElementParser();

            try
            {
                var elements = parser.ParseElements(sxml).ToList();

                if (!elements.Any())
                {
                    return false;
                }

                return AreElementsBalanced(elements);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool AreElementsBalanced(IEnumerable<Element> elements)
        {
            var stack = new Stack<Element>();

            foreach (var element in elements)
            {
                _ = stack.TryPeek(out var elementInStack);

                if (elementInStack is null || element.IsOpeninglement())
                {
                    stack.Push(element);
                    continue;
                }

                if (!element.WasOpenedBy(elementInStack))
                {
                    return false;
                }

                stack.Pop();
            }

            return stack.Count == 0;
        }
    }
}
