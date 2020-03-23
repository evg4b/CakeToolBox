<h1 align="center">CakeToolBox.Parameters</h1>
<p align="center">
  <img width="150px" src="https://res.cloudinary.com/evg-abramovitch/image/upload/v1584908793/CakeToolBox/cake-tool-box.png">
</p>

## Aliases

### Choice

Returns the only one possible item from the list based on the passed parameters. This method do not provide values from argument.
In the case of passing multiple arguments, throws `MoreThanOneCaseSpecifiedException` exception.

``` CSharp
// common usage
var parameter = Choice("case1", "case2", "case3");

// with default value
var parameter = Choice(new string[]{"case1", "case2", "case3"}, "defaultCase");
```