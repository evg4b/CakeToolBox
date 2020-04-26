<h1 align="center">CakeToolBox.Parameters</h1>
<p align="center">
  <img width="150px" src="https://res.cloudinary.com/evg-abramovitch/image/upload/v1584908793/CakeToolBox/cake-tool-box.png">
</p>

## Aliases

The following aliases are currently available:

- [Aliases](#aliases)
  - [Parameter](#parameter)
  - [Choice](#choice)
  - [ConfigureRequiredParameter](#configurerequiredparameter)
  - [ConfigureParameterWithDefaultValue](#configureparameterwithdefaultvalue)
- [Not recommended for use:](#not-recommended-for-use)
  - [GetAllArguments](#getallarguments)
  - [GetAllArgumentsNames](#getallargumentsnames)

### Parameter

This alias returns a value from an argument, if it was not specified then returns value from environment variable.

Name of environment variable is a name of the parameter in `UPPER_SNAKE_CASE` notation with prefix `CAKE_` . For parameter name `DebugMode`, name of an environment variable will be `CAKE_DEBUG_MODE`. **Note**: Notation transform works only with `camelCase` and `PascalCase` notations.

When default value not provided and value in argument or environment variable not set, this alias throws exception.

``` CSharp
// common usage
var pi = Parameter<double>("Pi");

// with default value
var pi = Parameter("Pi", 3.14);

// These parameters can be specified using the argument 'Pi' or environment variable 'CAKE_PI'
```

### Choice

Returns the only one possible item from the list based on the passed parameters. This method do not provide values from argument.
In the case of passing multiple arguments, throws exception.

``` CSharp
// common usage
var parameter = Choice("case1", "case2", "case3");

// with default value
var parameter = Choice(new string[]{"case1", "case2", "case3"}, "defaultCase");
```

### ConfigureRequiredParameter

This alias returns a function to get required parameters with custom configuration. The returned function works similarly to `Parameter` alias without default parameter.

``` CSharp
var parameter = ConfigureRequiredParameter<double>("CUSTOM_PREFIX");
var pi = parameter("Pi");
// This parameters can be specified using the argument 'Pi' or environment variable 'CUSTOM_PREFIX_PI'
```

### ConfigureParameterWithDefaultValue

This alias returns a function to get parameters with default value and custom configuration. The returned function works similarly to `Parameter` alias with default parameter.

``` CSharp
var parameter = ConfigureParameterWithDefaultValue<double>("CUSTOM_PREFIX");
var pi = parameter("Pi", 3.14);
// This parameters can be specified using the argument 'Pi' or environment variable 'CUSTOM_PREFIX_PI'
```

## Not recommended for use:

**WARN:** These methods use reflection to getting data. In future versions of CakeBuild, these aliases may do not work. Use these methods only when absolutely necessary and lock cake build version.

### GetAllArguments

Returns all arguments and their values ​​passed to cake script.

``` CSharp
var arguments = GetAllArguments(); // returns IDictionary<string, string>
```

### GetAllArgumentsNames

Returns names of all arguments ​​passed to cake script.

``` CSharp
var argumentsNames = GetAllArgumentsNames(); // returns IEnumerable<string>
```