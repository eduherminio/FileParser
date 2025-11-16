# FileParser

[![Azure DevOps][azuredevopslogo]][azuredevopslink]
[![GitHub Actions][githubactionslogo]][githubactionslink]
[![Circle CI][circlecilogo]][circlecilink]

[![Nuget][nugetlogo]][nugetlink]

[![API][apimundologo]][apimundolink]

**FileParser** is a **.NET** library designed to read text files line-by-line, saving each line's content into basic types vars (int, double, string, etc.).

- .NET Framework 4.6 was supported until v1.4.x.
- .NET Standard 2.0 and 2.1 were supported until v1.6.x.
- `Nullable` is enabled from 1.6.x.

## Purpose

This project was born with a very specific purpose: providing a tool with whom easily parse files with a known structure, ideally being as flexible and easy to use as C++ standard IO approach.

For those who don't understand what I mean, here's a simple Use Case ([also reposited](https://github.com/eduherminio/FileParser/tree/main/Examples)):

Given the following `input.txt`, which contains an integer n (>=0) followed by n doubles and a final string,

```txt
5   1.1 3.14159265 2.2265       5.5 10              fish
```

A simple `.cpp` snippet like the following one could process `input.txt`, providing that file is selected as standard input source:

`./myExecutable < input.txt > output.txt`

```cpp
#include <iostream>
#include <list>
#include <string>

int main()
{
    int _integer;
    std::string _str;
    std::list<double> _list;
    double _auxdouble;

    // Input start;
    std::cin>>_integer;
    for(int i=0; i<_integer; ++i)
    {
        std::cin>>_auxdouble;
        _list.push_back(_auxdouble);
    }
    std::cin>>_str;
    // Input end

    // Data processing

    // Output start
    std::cout<<_integer<<" ";
    for(const double& d : _list)
        std::cout<<d<<" ";
    std::cout<<_str;
    // Output end

    return 0;
}
```

Seems effortless to process these kind of simple `.txt` files using C++, right?

Well, using C# things are not so straight-forward, and that's why `FileParser` was created for:

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using FileParser;

namespace FileParserSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            List<double> listDouble = new List<double>();
            string str;

            // Input start
            IParsedFile file = new ParsedFile("SimpleInput.txt");
            IParsedLine firstLine = file.NextLine();

            int _integer = firstLine.NextElement<int>();

            for(int i=0; i<_integer; ++i)
                listDouble.Add(firstLine.NextElement<double>());

            str = firstLine.NextElement<string>();
            // Input end

            // Data Processing

            // Output start
            StreamWriter writer = new StreamWriter("..\\CSharpSimpleOutput.txt");
            using (writer)
            {
                writer.WriteLine(_integer + " " + string.Join(null, listDouble));
            }
            // Output end
        }
    }
}
```

## Documentation

I've done my best to create a [WIKI](https://github.com/eduherminio/FileParser/wiki) describing FileParser API.

Besides the WIKI, some real (own) projects where it has been used are:

- [Google #HashCode 2018](https://github.com/eduherminio/Google_HashCode_2018/blob/master/GoogleHashCode2018/Project/Manager.cs#L63).
- [Google #HashCode 2020](https://github.com/eduherminio/Google_HashCode_2020/blob/master/GoogleHashCode/BookManager.cs#L167).
- [Advent of Code 2018](https://github.com/eduherminio/advent-of-code-2018).
- [Advent of Code 2023](https://github.com/eduherminio/AoC2023).

## Contributing, issues, suggestions, doubts

If anyone else ever happens to use FileParser, I'll be happy to accept suggestions and solve any doubts.

Just open an issue :)

[githubactionslogo]: https://github.com/eduherminio/FileParser/actions/workflows/ci.yml/badge.svg
[githubactionslink]: https://github.com/eduherminio/FileParser/actions/workflows/ci.yml
[azuredevopslogo]: https://dev.azure.com/eduherminio/FileParser/_apis/build/status/eduherminio.FileParser?branchName=main
[azuredevopslink]: https://dev.azure.com/eduherminio/FileParser/_build/latest?definitionId=1&branchName=main
[circlecilogo]: https://circleci.com/gh/eduherminio/FileParser/tree/main.svg?style=svg
[circlecilink]: https://circleci.com/gh/eduherminio/FileParser/tree/main
[nugetlogo]: https://img.shields.io/nuget/v/FileParser.svg?style=flat-square&label=nuget
[nugetlink]: https://www.nuget.org/packages/FileParser
[apimundologo]: https://img.shields.io/badge/FileParser%20API-Apimundo-728199.svg
[apimundolink]: https://apimundo.com/organizations/nuget-org/nuget-feeds/public/packages/FileParser/versions/latest?tab=types
