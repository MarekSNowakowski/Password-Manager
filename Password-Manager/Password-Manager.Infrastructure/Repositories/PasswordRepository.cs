using Password_Manager.Core.Domain;
using Password_Manager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Password_Manager.Infrastructure.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly AppDbContext _appDbContext;

        public PasswordRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Password s)
        {
            try
            {
                _appDbContext.Password.Add(s);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<IEnumerable<Password>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Password);
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.Password.Remove(_appDbContext.Password.FirstOrDefault(x => x.Id == id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<Password> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Password.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(Password s)
        {
            try
            {
                var z = _appDbContext.Password.FirstOrDefault(x => x.Id == s.Id);

                z.Service = s.Service;
                z.Username = s.Username;
                z.PassEncrypted = s.PassEncrypted;
                z.Salt = s.Salt;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    }
}
