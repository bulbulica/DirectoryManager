
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsManagement.Shared.Abstractions
{
    public interface IInitializer
    {
        void InitializeContext(IServiceCollection collection, IConfiguration configuration);
        void InitializeData(IServiceProvider serviceProvider);
        void Configure(IApplicationBuilder builder);
    }
}
