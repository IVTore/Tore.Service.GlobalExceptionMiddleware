# Tore.Service.GlobalExceptionMiddleware

Language: C#. (.Net 5)

Nuget package: [Tore.Service.GlobalExceptionMiddleware](https://www.nuget.org/packages/Tore.Service.GlobalExceptionMiddleware/)

Dependancies: None.

## GlobaleExceptionMiddleWare :

A standard middleware for .Net web API intercepting unhandled exceptions raised during requests.

```C#
// Add this to your startup.cs usings.

using Tore.Service;
...
// Add at service configure method before any other app.Use... commands :

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      
      // If an exception response builder method defined bind it:
      GlobalExceptionMiddleWare.exceptionResponseBuilder = aStaticMethodToBuildExceptionResponse;
      // Bind global exception middleware.
      app.UseMiddleware<GlobalExceptionMiddleware>();
      
      app.UseRouting();
 
      app.UseAuthorization();

      app.UseEndpoints(endpoints => {endpoints.MapControllers();});
  }

// If developer exception page is needed during development : 

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if(env.IsDevelopment()) {
          app.UseDeveloperExceptionPage();
          // Maybe there is swagger too.
          app.UseSwagger();
          app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test v1"));
      
      } else {
          // Non development only...
          // If an exception response builder method defined, bind it :
          GlobalExceptionMiddleWare.exceptionResponseBuilder = aStaticMethodToBuildExceptionResponse;
          // Bind global exception middleware.  
          app.UseMiddleware<GlobalExceptionMiddleware>();
      }
      
      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers();});
}
                                        

```  

Note that this does not handle invalid routes. 
For that invalid routes must be rerouted to an endpoint,
If that endpoint raises exception, then GlobalExceptionMiddleware is activated.
