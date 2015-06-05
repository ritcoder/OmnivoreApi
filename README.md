# Omnivore.Net

This is an implementation of the [Omnivore](http://omnivore.io) api. 

It is still work in progress.


## How to Use
- Import the library into your project. At the moment, you'll have to clone and build. Nuget support will be added later.
- Input the right namespaces. The root namespace is `OmnivoreApi`.
- Set the parameters

All methods and properties are on the instances. For each of use, a default instance (singleton) is created on the `Omnivore` class.
Loaders for the various resources. The functions to make the direct calls are also exposed.

### Samples
#### Initialize
```c#
Omnivore.io.apiKey = new SecureString().AppendText("--your api key here--");
Omnivore.io.version = "0.1"; //this is the default value on the singleton
Omnivore.io.baseUrl = "https://api.omnivore.io"; //this is the default on the singleton
```

#### Accessing the resources
Use `ListAsync` to get the array, `RetrieveAsync` to get a single item.
The others include `OpenAsync`, 'VoidAsync' and `AddAsync`.

```c#
var allLocations = await io.locations.ListAsync();
var oneLocation = await io.locations.RetrieveAsync("--locationId--");
var menu = await io.menu.GetAsync(oneLocation.id);
```

The same applies to all the other resources

### Runner Tester
```c#
var tester = new OmnivoreApi.Tests.ResourceTester(Omnivore.io);
var result = await tester.RunAll();
foreach (var kvp in result) WriteLine("{0} >> {1}", kvp.Key, kvp.Value);
```
 
## Completed
- All resources
- Full API


## To do
- Document each of the components. At the moment only the resources are documented.
- Add all tests
- Add Nuget support
- Complete and host documentation