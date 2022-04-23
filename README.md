# Tore.Service.GlobalExceptionMiddleware

Language: C#.

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

Dependencies: <br/>
&emsp; net 6.0<br/>
&emsp; Microsoft.AspNetCore.Mvc.NewtonsoftJson (6.0.1) [Please refer to note 3 below]<br/>

## GlobalExceptionMiddleware :

A standard middleware for net web API <br/>
It intercepts unhandled exceptions raised during requests<br/>
and calls a developer defined method to generate responses accordingly.<br/>

<b>WARNING</b>: <br/>
1] net 5.0 is no more supported.<br/>
2] Exception Responder Type has changed. See below.<br/>

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
&emsp; Weirdly enough default http abstractions miss some methods like HttpResponse.CloseAsync().<br/>
&emsp; So it saves me from a lot of class chasings and abstractions and I use it in my API's anyway.<br/>
<br/>
3] GlobalExceptionMiddleware assignments should be done at configuration.<br/>
&emsp; After service starts, since system goes multithreading, do not change assignments.<br/>
&emsp; Turkish proverb: While crossing the river, one does not switch horses...<br/>
<br/>
