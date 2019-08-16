using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

class OopsMiddleware {
  public OopsMiddleware(RequestDelegate next) {
  }

  public async Task InvokeAsync(HttpContext context,
    OutputFormatterSelector selector,
    IHttpResponseStreamWriterFactory writerFactory
  ) {
    context.Response.StatusCode = 400;
    var response = new ErrorResponse {
      Errors = new Error[] {
        new Error {
          Code = "test",
          Description = "test",
          Meta = new Dictionary<string, object> {
            ["test"] = new Uri("http://go-fish")
          }
        },
      }
    };

    var formatterContext = new OutputFormatterWriteContext(
      context, writerFactory.CreateWriter, typeof(ErrorResponse), response);

    var selectedFormatter = selector.SelectFormatter(formatterContext, Array.Empty<IOutputFormatter>(), new MediaTypeCollection());
    await selectedFormatter.WriteAsync(formatterContext);
  }
}

public class ErrorResponse {
  public IEnumerable<Error>? Errors { get; set; } = null;
}

public class Error {
  public string Code { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public IEnumerable<Error>? InnerErrors { get; set; } = null;
  public Dictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();
}