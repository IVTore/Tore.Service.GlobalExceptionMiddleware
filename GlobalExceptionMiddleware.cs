﻿/*————————————————————————————————————————————————————————————————————————————
    ———————————————————————————————————————————
    |      Global Exception Middleware.       |
    ———————————————————————————————————————————

    © Copyright 2021 İhsan Volkan Töre.

Author              : IVT.  (İhsan Volkan Töre)
Version             : 202112221242
License             : MIT.

History             :
202112221242: IVT   : Added.
————————————————————————————————————————————————————————————————————————————*/
using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace Tore.Service {

    /**———————————————————————————————————————————————————————————————————————————
        CLASS:  GlobalExceptionMiddleware.                              <summary>
        USAGE:                                                          <br/>
                builder.UseMiddleware&lt;GlobalExceptionMiddleware&gt;()<br/>
                                                                        <br/>
                Before any other builder.Use... commands.               <br/>
                Don't forget to assign ExceptionResponder.              </summary>
    ————————————————————————————————————————————————————————————————————————————*/
    public class GlobalExceptionMiddleware {

        /**———————————————————————————————————————————————————————————————————————————
          TYPE: ExceptionResponderDelegate                                  <summary>
          TASK:                                                             <br/>
                Method delegate type for exception responder.               <para/>
          ARGS:                                                             <br/>
                context: HttpContext: Current request http context.         <br/>
                exception: Exception: Exception to respond.                 </summary>
        ————————————————————————————————————————————————————————————————————————————*/
        public delegate Task ExceptionResponderDelegate(
            HttpContext context, 
            Exception exception
        );

        private readonly RequestDelegate next;

        /**———————————————————————————————————————————————————————————————————————————
          VAR:  ExceptionResponder.                                     <summary>
          USE:                                                          <br/>
                Method delegate for responding an exception.            <br/>
                Method delegate type must be ExceptionResponderDelegate.<para/>
                The method can modify context.response object, and also <br/>
                optionally write the response and return.               <para/>
                Whether the method writes the response or not,          <br/>
                the middleware issues a                                 <br/>
                <c>context.response.CompleteAsync();</c>                <br/>
                flushing the response.                                  </summary>
        ————————————————————————————————————————————————————————————————————————————*/
        public static ExceptionResponderDelegate? ExceptionResponder;

        /**——————————————————————————————————————————————————————————————————————————
          CTOR: GlobalExceptionMiddleware                                   <summary>
          TASK:                                                             <br/>
                Constructs a GlobalExceptionMiddleware object.              <br/>
                This is called per request by the .Net Web API.             <para/>
          ARGS:                                                             <br/>
                nextMiddleWare: RequestDelegate:
                Next middleware delegate to invoke.                         </summary>
        ————————————————————————————————————————————————————————————————————————————*/
        public GlobalExceptionMiddleware(RequestDelegate nextMiddleWare) {
            next = nextMiddleWare;
        }

        /**——————————————————————————————————————————————————————————————————————————
          FUNC: InvokeAsync                                                 <summary>
          TASK:                                                             <br/>
                Middleware chain call, do not use, .NET Web API calls this. <para/>
          ARGS:                                                             <br/>
                context : HttpContext: Current http context.                </summary>
        ————————————————————————————————————————————————————————————————————————————*/
        public async Task InvokeAsync(HttpContext context) {
            try {
                await next(context);
            } catch (Exception e) {
                await ExceptionAsync(context, e);
            }
        }
 
        private async Task ExceptionAsync(HttpContext ctx, Exception exc) {
            HttpResponse res = ctx.Response;

            res.StatusCode = (int)HttpStatusCode.BadRequest; // Default
            try { 
                if (ExceptionResponder != null) 
                    await ExceptionResponder.Invoke(ctx, exc);
            } catch (Exception e) {
                BadCode("ExceptionResponder caused exception.", e);
                return;
            }
            try {
                await res.CompleteAsync();
            } catch (Exception e) {
                BadCode("Mulformed exception response.", e); 
            }
        }

        private void BadCode(string s, Exception e){
            string src = "[Tore.Service.GlobalExceptionMiddleware] ";
            Console.WriteLine(src + "Dev. Exception:"); 
            Console.WriteLine(src + s);
            Console.WriteLine(src + e.Message);
        }
    }
}
