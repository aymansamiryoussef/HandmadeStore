using HandmadeStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public  Task<int> SaveChangesAsync()
        {
            return   _context.SaveChangesAsync();
        }
    }
}
