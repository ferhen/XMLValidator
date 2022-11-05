using System.Text;

namespace XMLValidator.Core
{
    internal class ElementParser
    {
        private enum State
        {
            Empty,
            OpeningBracket,
            ForwardSlash,
            Text,
            ClosingBracket,
            InnerText
        }

        private enum Command
        {
            ToForwardSlash,
            ToText,
            ToClosingBracket,
            ToOpeningBracket
        }

        private record StateTransition(State State, Command Command);

        private readonly Dictionary<StateTransition, State> transitions = new()
        {
            { new StateTransition(State.Empty, Command.ToOpeningBracket), State.OpeningBracket },
            { new StateTransition(State.OpeningBracket, Command.ToForwardSlash), State.ForwardSlash },
            { new StateTransition(State.OpeningBracket, Command.ToText), State.Text },
            { new StateTransition(State.ForwardSlash, Command.ToText), State.Text },
            { new StateTransition(State.Text, Command.ToClosingBracket), State.ClosingBracket },
            { new StateTransition(State.Text, Command.ToText), State.Text },
            { new StateTransition(State.ClosingBracket, Command.ToOpeningBracket), State.OpeningBracket },
            { new StateTransition(State.ClosingBracket, Command.ToText), State.InnerText },
            { new StateTransition(State.InnerText, Command.ToText), State.InnerText },
            { new StateTransition(State.InnerText, Command.ToOpeningBracket), State.OpeningBracket},
        };

        public IEnumerable<Element> ParseElements(string input)
        {
            var state = State.Empty;
            var nameBuffer = new StringBuilder();
            var isClosingElement = false;

            foreach (var character in input)
            {
                var command = GetCommand(character);
                state = MoveNext(state, command);

                switch (state)
                {
                    case State.OpeningBracket:
                        isClosingElement = false;
                        nameBuffer.Length = 0;
                        break;
                    case State.ForwardSlash:
                        isClosingElement = true;
                        break;
                    case State.Text:
                        nameBuffer.Append(character);
                        break;
                    case State.ClosingBracket:
                        yield return new Element(nameBuffer.ToString(), isClosingElement ? ElementType.Closing : ElementType.Opening);
                        break;
                    default:
                        break;
                }
            }

            if (state == State.InnerText)
            {
                throw new FormatException("Trailing text is not valid XML");
            }
        }

        private Command GetCommand(char character)
        {
            return character switch
            {
                '<' => Command.ToOpeningBracket,
                '/' => Command.ToForwardSlash,
                '>' => Command.ToClosingBracket,
                _ => Command.ToText
            };
        }

        private State MoveNext(State state, Command command)
        {
            var transition = new StateTransition(state, command);

            if (!transitions.TryGetValue(transition, out var nextState))
            {
                throw new FormatException("Invalid character found in XML");
            }

            return nextState;
        }
    }
}
