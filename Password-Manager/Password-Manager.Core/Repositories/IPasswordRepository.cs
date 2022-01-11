using Password_Manager.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.Core.Repositories
{
    public interface IPasswordRepository
    {
        public Task<IEnumerable<Password>> BrowseAllAsync();
        public Task<Password> GetAsync(int id);
        public Task AddAsync(Password s);
        public Task DelAsync(int id);
        public Task UpdateAsync(Password s);
    }
}
