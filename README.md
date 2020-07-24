# Sanji
A sandbox for smoking testing

```csharp
var httpClient = Sanji.GetService("SUT").CreateHttpClient();
var response = await httpClient.GetAsync("/books");
```

## What?
Sanji is a sandbox for smoking test. It will run your servies in processes and provide a friendly API to access in test cases.

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
            "Executable": "..\\..\\..\\..\\SampleASP1\\bin\\Debug\\netcoreapp3.1\\SampleASP1.exe",
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

### Step4: Write test!
We can easily send HTTP request to our services under test with Sanji.
```csharp
var httpClient = Sanji.GetService("SUT").CreateHttpClient();
var response = await httpClient.GetAsync("/books");
```
