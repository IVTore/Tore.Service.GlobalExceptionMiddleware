# Tore.Service.GlobalExceptionMiddleware

Language: C#.

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Dependencies: <br/>
&emsp; net 7.0<br/>
&emsp; Microsoft.AspNetCore.Mvc.NewtonsoftJson (7.0.4+)<br/>

## GlobalExceptionMiddleware :

A standard global exception middleware for net web API <br/>
It intercepts unhandled exceptions raised during requests<br/>
and calls a developer defined method to generate responses accordingly.<br/>


For using it, modify program.cs:<br/>
```C#
using Tore.Service;     // Add this to your usings.

...

var app = builder.Build();
      
// Bind the exception responder method:
GlobalExceptionMiddleWare.ExceptionResponder = MyClass.MyStaticExceptionResponder;
      
// Bind global exception middleware.
app.UseMiddleware<GlobalExceptionMiddleware>();

...
  
```

An exception responder method must be defined to generate the responses.
It should be a delegate of type:

```C#
public delegate Task ExceptionResponderDelegate(HttpContext context, Exception exception);

// Which is a method like:

public static async Task MyStaticExceptionResponder(HttpContext context, Exception exception) {
    // do it.
}

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
    GlobalExceptionMiddleWare.ExceptionResponder = MyClass.MyStaticExceptionResponder;
    app.UseMiddleware<GlobalExceptionMiddleware>();
```
&emsp; Before <br/>

```C#
    app.UseDeveloperExceptionPage();
```

&emsp; That way developer exception page overrides the global exception middleware.<br/>
    <br/>
2] Why Microsoft.AspNetCore.Mvc.NewtonsoftJson? <br/>
&emsp; Weirdly enough default http abstractions miss some methods like HttpResponse.CompleteAsync().<br/>
&emsp; So it saves me from a lot of package hunting and it is used in web API's anyway.<br/>
<br/>
3] GlobalExceptionMiddleware assignments should be done at configuration.<br/>
&emsp; After service starts, since system goes multithreading, do not change assignments.<br/>
<br/>
