﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IdentityServer.Persistence.EF
{
    public class PersistenceContext : IPersistenceContext
    {

        private DbContext _context;


        public IUserRepository UserRepository { get; set; }
        public IUserRepository TeachersRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void InitializeDbContext(IServiceProvider provider)
        {
            if (_context == null)
            {
                _context = provider.GetRequiredService<DbContext>();
            }
            UserRepository = new UserRepository(_context);
        }

        public PersistenceContext() { }


        public int Complete()
        {
            int retVal = 0;
            if (_context != null)
            {
                retVal = _context.SaveChanges();
            }

            return retVal;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void InitializeContext(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<DbContext>(options =>
            options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("StudentsManagementConnection"),
                b => b.MigrationsAssembly("StudentsManagement.Persistence.EF")));

            InitializeDbContext(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder builder)
        {
            throw new NotImplementedException();
        }

        public void InitializeData(IServiceProvider serviceProvider)
        {
        }
    }
}
