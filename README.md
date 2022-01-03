# Tore.Service.GlobalExceptionMiddleware

Language: C#. (.Net 5)

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

Dependancies: None.

## GlobalExceptionMiddleware :

A standard middleware for .Net web API intercepting unhandled exceptions raised during requests and generate responses accordingly.


For using it, modifications must be done in startup.cs:
```C#
// Add this to your startup.cs usings.

using Tore.Service;

```

Add at service configure method before any other app.Use... commands :

```C#
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      
      // Bind the exception response builder method :
      GlobalExceptionMiddleWare.exceptionResponseBuilder = SomeClass.aStaticMethodToBuildExceptionResponse;
      // Bind global exception middleware.
      app.UseMiddleware<GlobalExceptionMiddleware>();
      
      app.UseRouting();
 
      app.UseAuthorization();

      app.UseEndpoints(endpoints => {endpoints.MapControllers();});
  }
```

An exception response builder method must be defined to generate the responses.
It must be a delegate of type:

```C#
public delegate void ExceptionResponseBuilder(HttpContext context, Exception exception);
```

The delegated method has full view of current request's http context and exception.<br/>
The method can modify context.response object, and also optionally write the response and return. <br/>
Whether the method writes the response or not, the middleware issues a 
```C#
context.response.CompleteAsync()
```
flushing the response.

---

*Notes:*<br/>
<br/>
1. If developer exception page is required during development: <br/>
   Add <br/>
```C#
    app.UseMiddleware<GlobalExceptionMiddleware>();
```
   Before <br/>

```C#
    app.UseDeveloperExceptionPage();
```

That way developer exception page overrides the global exception middleware.<br/>
    <br/>
2. This setup does not handle invalid routes. <br/>
For that, invalid routes must be re-routed to a controller endpoint, <br/>
If that endpoint raises exception, then GlobalExceptionMiddleware is activated.
