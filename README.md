# Tore.Service.GlobalExceptionMiddleware

Language: C#.

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

Dependancies: 
&emsp; net5.0
&emsp; Microsoft.AspNetCore.Mvc.Core (>= 2.2.5)
&emsp; Microsoft.AspNetCore.Mvc.NewtonsoftJson (>= 5.0.10)

## GlobalExceptionMiddleware :

A standard middleware for .Net 5 web API <br/>
It intercepts unhandled exceptions raised during requests<br/>
and calls a user defined method to generate responses accordingly.<br/>


For using it, modifications must be done in startup.cs:
```C#
// Add this to your startup.cs usings.

using Tore.Service;

```

Add bindings at service configure method before any other app.Use... commands:

```C#
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      
      // Bind the exception responder method :
      GlobalExceptionMiddleWare.ExceptionResponder = SomeClass.AStaticMethodToRespondException;
      // Bind global exception middleware.
      app.UseMiddleware<GlobalExceptionMiddleware>();
      
      app.UseRouting();
 
      app.UseAuthorization();

      app.UseEndpoints(endpoints => {endpoints.MapControllers();});
  }
```

An exception responder method must be defined to generate the responses.
It should be a delegate of type:
```C#
public delegate void ExceptionResponderDelegate(HttpContext context, Exception exception);
```

The method should be bound as:
```C#
    GlobalExceptionMiddleWare.ExceptionResponder = SomeClass.AStaticMethodToRespondException;
```

The delegated method has full view of current request's http context and exception.<br/>
The method can modify context.response object, and also optionally write the response and return. <br/>
Whether the method writes the response or not, the middleware issues a 
```C#
context.response.CompleteAsync()
```
flushing the response.

---

**Notes:**<br/>
<br/>
1] If developer exception page is required during development: <br/>
&emsp; Add <br/>
```C#
    GlobalExceptionMiddleWare.ExceptionResponder = SomeClass.AStaticMethodToRespondException;
    app.UseMiddleware<GlobalExceptionMiddleware>();
```
&emsp; Before <br/>

```C#
    app.UseDeveloperExceptionPage();
```

&emsp; That way developer exception page overrides the global exception middleware.<br/>
    <br/>
2] This setup does not handle invalid routes. <br/>
&emsp; For that, invalid routes must be re-routed to a controller endpoint, <br/>
&emsp; If that endpoint raises exception, then GlobalExceptionMiddleware is activated.
