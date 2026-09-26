# RapidTDD - Rapid TDD application

> 🇷🇸 [Srpska verzija / Serbian version](./README-SR.md)

Compile, run C# code in memory and execute tests with code coverage feature - without refactoring, without asserts, mocks, or testability layers.

**Source code:** https://github.com/Darko-Bondjerovic/RapidTDD

You can find more information on the youtube RapidTDD channel:
https://www.youtube.com/@rapidtdd

The latest version of RapidTDD is in releases:
https://github.com/Darko-Bondjerovic/RapidTDD/releases/

![alt text](RapidTDD.png?raw=true)

---

### Core Concept

With the `[TEST]` marker in the text output, since `Console.WriteLine` commands can be called anywhere in the code, RapidTDD can test both **private** and **void** methods (which don't return a result) - without refactoring the code, without asserts, testability layers, mocks, etc. (which are necessary when using classic tools such as NUnit, xUnit...)

**Actual result (actual result)** – the application extracts the text from the first occurrence of the `[TEST]` marker to the next occurrence of the `[TEST]` marker.

**Expected result (expected result)** can be created in the following ways:
1. The user manually enters the expected value as arbitrary text within the application (in a text edit box)
2. It is possible to directly copy the actual result into the expected result inside the application
3. By using the `[EXPC]` marker in the C# code and writing the value directly into the code (as in NUnit)

> **Note:** Text defined with `[EXPC]` has higher priority and replaces the previously stored expected result. During execution, RapidTDD captures the output, parses it, creates a new list of tests, writes new actual values, compares them with stored expected values, and marks status – pass/fail.

### Example

```csharp
public class Program
{
    static void Main(string[] args)
    {
        FindPrimesTest(6);
        FindPrimesTest(15);
        FindPrimesTest(20);
    }

    static void FindPrimesTest(int input)
    {
        Console.WriteLine($"[TEST] Find primes {input}");
        Console.WriteLine(FindPrimes(input).ToString());
        // hard-coded expected value:
        if (input == 15) Console.WriteLine("[EXPC]3\n5");
    }
}
```

Output in RapidTDD:
```
[TEST] Find primes 6
2
3

[TEST] Find primes 15
3
5  <- actual == expected from [EXPC]

[TEST] Find primes 20
2
2
5
```

### Workflow

1. We write `Console.WriteLine("[TEST] <name>")` where we want to see what happened, without changing the rest of the code
2. We run the program through RapidTDD - we get actual results for all tests
3. We click `Copy actual to expected` - for all tests at once (or individually)
4. The app now has all expected values and compares them with actual ones
5. Now we have a green baseline – all tests pass
6. We refactor the code – every time the code runs, RapidTDD automatically compares again and marks tests as fail/pass wherever the actual result has changed.

### Advantages over classic test frameworks

**We don't have to refactor the code right away**
- Existing code can stay 100% as it is
- We can test code inside **private** methods - no need to change them to public
- We can test intermediate results, the method can be **void** - we just print values
- No testability layer, dependency injection, interface, Mock, Assert needed

**Simplicity**
- Test names are arbitrary - they are not method names
- We don't have to define expected results in advance
- We can sort test execution order directly in code
- Creating N tests in a loop, without duplication:
```csharp
for(int i=0; i<100; i++) { 
    Console.WriteLine($"[TEST] test {i}"); 
    ExecuteSomeMethod(i); 
}
```
- If test name is the same, e.g. MyTest, RapidTDD auto-numbers: 1.MyTest, 2.MyTest...

**Fast execution**
- Build and execution happens in RAM, without writing to disk
- Complex outputs (lists, matrices, trees) are checked instantly - text is compared

### Additional features

- Internal Code Coverage (Main menu UI: Views → Coverage)
- List of tests and their actual/expected values can be saved/loaded via menu Tests → Save test file / Load test file

Code Coverage feature is added inside RapidTDD:

![alt text](CodeCover.png?raw=true)

#### Defining expected with [EXPC]

Expected result is all text after first `[EXPC]` up to next `[TEST]`. The `[EXPC]` marker itself is ignored.

```csharp
void AddTest(int a, int b, int exp)
{
    Console.WriteLine($"[TEST] add: {a}+{b}");
    Console.WriteLine($"{a+b}");
    Console.WriteLine($"[EXPC]{exp}");
}
```

For multi-line, write `[EXPC]` only once:
```csharp
Console.WriteLine("[EXPC]first row");
Console.WriteLine("second row");
```

> For a test to pass, number of lines and text must be completely identical, including newline `\n`.

---

### Contributing

If you want to contribute, create an issue or look for an open issue and provide me with a pull request. As always: if you're intending to make a big change, let's discuss it first to avoid unnecessary work.

My email: rapidtdd@gmail.com

### For Developers
For technical stack and future migration plans, see [DEVELOPMENT.md](./DEVELOPMENT.md) and [ROADMAP.md](./ROADMAP.md).