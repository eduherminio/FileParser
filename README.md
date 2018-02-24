# FileParser

[![Build Status](https://travis-ci.org/eduherminio/TextParser.svg?branch=master)](https://travis-ci.org/eduherminio/TextParser)

**File parser** designed to read text files line-by-line, saving each line's content into basic types vars (int, double, string, etc.).

**.NET Standard 2.0** project

This project was born with a very specific purpose: providing a tool with whom easily parse files with a known structure, ideally being  as flexible and easy to use as C++ standard IO approach.

For those who don't understand what I mean, here's a simple Use Case:

Given the following `input.txt`,
```txt
5   1.1 3.14159265 2.2265       5.5 10              fish
```

A simple `.cpp` snippet like the following one could process `input.txt`, providing that file is selected as standard input source:

 `./myExecutable < input.txt > output.txt`

```cpp
#include <iostream>
#include <list>

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

Well, using C# things are not that straight-forward, and that's why FileParser was created for.