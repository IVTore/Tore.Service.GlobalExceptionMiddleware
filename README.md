# Tore.Service.GlobalExceptionMiddleware

Language: C#.

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

Dependencies: <br/>

GlobalExceptionMiddleware v6.0.1 for net 6.0 .<br/>
&emsp; net 6.0<br/>
&emsp; Microsoft.AspNetCore.Mvc.NewtonsoftJson (6.0.1) [Please refer to note 4 below]<br/>

## GlobalExceptionMiddleware :

A standard middleware for net web API <br/>
It intercepts unhandled exceptions raised during requests<br/>
and calls a developer defined method to generate responses accordingly.<br/>

<b>WARNING</b>: <br/>
1] net 5.0 support terminated.<br/>
2] Exception Responder Type has changed into:<br/>

```C#
    public static async Task MyStaticExceptionResponder(HttpContext context, Exception exception) {
        // do it.
    }
```

For using it in net 6.0 modify program.cs:<br/>
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
```

The method should be bound as:
```C#
    GlobalExceptionMiddleWare.ExceptionResponder = MyClass.MyStaticExceptionResponder;
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
2] This setup does not handle invalid routes. <br/>
&emsp; For that, invalid routes must be re-routed to a controller endpoint, <br/>
&emsp; If that endpoint raises exception, then GlobalExceptionMiddleware is activated.<br/>
<br/>
3] Why Microsoft.AspNetCore.Mvc.NewtonsoftJson? <br/>
&emsp; Weirdly enough default http abstractions miss some methods like HttpResponse.CloseAsync().<br/>
&emsp; So it saves me from a lot of class chasings and abstractions and I use it in my API's anyway.<br/>
<br/>
4] GlobalExceptionMiddleware assignments should be done at configuration.<br/>
&emsp; After service starts, since system goes multithreading, do not change assignments.<br/>
&emsp; Turkish proverb: While crossing the river, one does not switch horses...<br/>
<br/>
