<h1 align="center">CakeToolBox.Parameters</h1>
<p align="center">
  <img width="150px" src="https://res.cloudinary.com/evg-abramovitch/image/upload/v1584908793/CakeToolBox/cake-tool-box.png">
</p>

## Aliases

### Switch

``` CSharp
// common usage
var value = Switch("case1", "case2", "case3");

// with default value
var value = Switch(new string[]{"case1", "case2", "case3"}, "defaultCase");
```