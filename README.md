# Tore.Service.GlobalExceptionMiddleware

Language: C#. (.Net 5)

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

Dependancies: None.

## GlobaleExceptionMiddleWare :

A standard middleware for .Net web API intercepting unhandled exceptions raised during requests.

```C#
// Add this to your program.cs usings.

using Tore.Service;
...
// Add at service configure method before any other app.Use... commands :      
  
  app.UseMiddleware<GlobalExceptionMiddleware>()                                       

```  
