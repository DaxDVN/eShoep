﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Requets} - Response={Response}: {request}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();
            timer.Stop();

            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}", typeof(TRequest).Name,
                    timeTaken.Seconds);
            }

            logger.LogInformation("[END] Handle {Requets} with {Response}", typeof(TRequest).Name,
                typeof(TResponse).Name);

            return response;
        }
    }
}