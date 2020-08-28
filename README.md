# Sanji
<img align="right" width=180 height=260 src=".github/images/Sanji's_Wanted_Poster.png">

[![Build](https://github.com/callmewhy/sanji/workflows/Build/badge.svg?branch=develop)](https://github.com/callmewhy/sanji/actions?query=branch%3Adevelop+workflow%3ABuild)
[![NuGet](https://img.shields.io/nuget/v/sanji)](https://www.nuget.org/packages/Sanji/)

A sandbox for smoke testing

```csharp
var httpClient = Sanji.GetService("SUT").CreateHttpClient();
var response = await httpClient.GetAsync("/books");
```

## What?
Sanji is a sandbox for smoke testing. It will run your servies in processes and provide a friendly API to access in test cases.

## How?

### Step1: Create your MSTest project
As sanji is only a sandbox for testing, we still need test runner like MSTest to run our test cases.

### Step2: Add appsettings.json
We need a appsettings.json file to specify servies we want to test.
```json
{
    "Services": [
        {
            "Name": "SampleASP1",
            "Executable": "SampleASP1.exe",
            "Port": 5000
        }
    ]
}
```

### Step3: Add AssemblyInit.cs
We have to start all services before test starts.
```csharp
[TestClass]
public class AssemblyInit
{
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext context)
    {
        Sanji.Start();
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        Sanji.Stop();
    }
}
```

### Step4: Write tests!
We can easily send HTTP request to our services under test with Sanji.
```csharp
var httpClient = Sanji.GetService("SUT").CreateHttpClient();
var response = await httpClient.GetAsync("/books");
```
