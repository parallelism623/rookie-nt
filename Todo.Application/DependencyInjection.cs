using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Commons;
using Todo.Application.Commons.Models;
using Todo.Domain.Entities;

namespace Todo.Application;
public static class DependencyInjection
{
    public static IServiceCollection ApplicationValidationConfig(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        return services.AddFluentValidationAutoValidation();
    }
}

