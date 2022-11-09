# XMLValidator
A CLI tool that checks whether or not a given XML is valid. Supports strings and files.

# Run
## Using dotnet CLI
Change directory to XMLValidator.CLI
```bash
cd XMLValidator.CLI
```
To test strings:
```bash
dotnet run -i "<Design><Code>hello world</Code></Design>"
```
To test files:
```bash
dotnet run -f input.xml
```

## Using MSBuild
After building the solution with XMLValidator.CLI as the Startup Project, change directory to:
```bash
cd XMLValidator.CLI\bin\Release\net6.0
```
To test strings:
```bash
XMLValidator.CLI.exe -i "<Design><Code>hello world</Code></Design>"
```
To test files:
```bash
XMLValidator.CLI.exe -f input.xml
```

# How it works
XMLValidator is divided in two main parts, the Parser and Validator.
The Parser reads the input and create a list of Element objects, which is consumed by the Validator to decide if what was received is a valid XML or not.

## Parser
The parser reads the input string and uses a Finite State Machine to create Element objects. Elements are records with a name and a type (Opening or Closing).

The FSM is defined as follows.

![StateMachine](/images/element-parser-state-machine.png)

## Validator
The validator receives the list of Elements and uses a stack to decide if the input is valid. Elements are stored in the stack if they have the type Opening. When a matching element with type Closing is received, that element is removed from the stack, which guarantees the nesting order of the elements. If a new element that doesn't match the element at the top of the stack, the XML is invalid. After all elements are processed, if there are no elements left in the stack, the XML is valid.
```javascript
input: "<Design><Code>hello world</Code></Design>"

<Design>   <Code>       </Code>       </Design>
   [] -> [<Design>] -> [ <Code> ] -> [<Design>] -> []
                       [<Design>]

output: true
explanation: stack is empty
```

```javascript
input: "<Design><Code>hello world</Code></Design><People>"

<Design>   <Code>       </Code>       </Design>     <People>
   [] -> [<Design>] -> [ <Code> ] -> [<Design>] -> [<People>]
                       [<Design>]

output: false
explanation: stack is not empty
```

```javascript
input: "<People><Design><Code>hello world</People></Code></Design>"

<People>  <Design>       <Code>       </People>     
   [] -> [<People>] -> [<Design>] -> [ <Code> ]
                       [<People>]    [<Design>] 
                                     [<People>] 

output: false
explanation: </People> closing Element did not match the Opening Element <Code> at the top of the stack
```
