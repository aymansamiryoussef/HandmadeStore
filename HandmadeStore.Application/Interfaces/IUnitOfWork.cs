using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
