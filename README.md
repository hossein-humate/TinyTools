  # LambdaHelper C#

Overview
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
This helper class translate string filter and orderby queries to valid Lambda Expression that can easily translate and execute into the System.Linq methods.
You can clone and start LambdaHelper project and check the example code in program.cs to test and understand the basic usage of the helper class.

Porpose of this class is to make the simplest way of queryable communication between client-side application to pass filter and orderby commands into the server side application by url Query String.

These are valid conditional operators to filter:

       |---------------------------------------------------------------------|
       |Humate Conditional operators to filter                               |
       |---------------------------------------------------------------------|
       |Examples for &filter={value}    to          C# compiled syntax       |
       |---------------------------------------------------------------------|
       |&filter=prop eq 2               |     x => x.prop == 2               |
       |&filter=prop lt 2               |     x => x.prop > 2                |
       |&filter=prop gt 2               |     x => x.prop < 2                |
       |&filter=prop le 2               |     x => x.prop >= 2               |
       |&filter=prop ge 2               |     x => x.prop <= 2               |
       |&filter=prop cn Humate          |     x => x.prop.Contains("Humate") |
       |&filter=prop.nest eq 2          |     x => x.prop.nest == 2          |
       |---------------------------------------------------------------------|
     
These are valid conditional operators to orderby:
 
       |----------------------------------------------------------------------------|
       |Humate Conditional operators to orderby                                     |
       |----------------------------------------------------------------------------|
       |Examples for &orderby={value}    to          C# compiled syntax             |
       |----------------------------------------------------------------------------|
       |&orderby=prop                   |     source.OrderBy(x => x.prop)           |
       |&orderby=prop desc              |     source.OrderByDescending(x => x.prop) |
       |----------------------------------------------------------------------------|
 
 Getting Start
 ----------------------------------------------------------------------------------------------------------------------------------------------
 Install the LambdaHelper package from Nuget:

