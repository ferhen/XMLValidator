namespace XMLValidator.Core
{
    internal enum ElementType
    {
        Opening,
        Closing
    }

    internal record Element(string Name, ElementType Type)
    {
        public bool WasOpenedBy(Element otherElement)
        {
            return Type == ElementType.Closing
                && otherElement.Type == ElementType.Opening
                && Name == otherElement.Name;
        }

        public bool IsOpeninglement() => Type == ElementType.Opening;
    }
}
