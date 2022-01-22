using Password_Manager.Core.Domain;
using Password_Manager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Password_Manager.Infrastructure.Repositories
{
    public class MasterPasswordRepository : IMasterPasswordRepository
    {
        private readonly AppDbContext _appDbContext;

        public MasterPasswordRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(MasterPassword s)
        {
            try
            {
                _appDbContext.MasterPassword.Add(s);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.MasterPassword.Remove(_appDbContext.MasterPassword.FirstOrDefault(x => x.Id == id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<MasterPassword> GetAsync(string username)
        {
            return await Task.FromResult(_appDbContext.MasterPassword.FirstOrDefault(x => x.Username == username));
        }

        public async Task<MasterPassword> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.MasterPassword.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(MasterPassword s)
        {
            try
            {
                var z = _appDbContext.MasterPassword.FirstOrDefault(x => x.Id == s.Id);

                z.Username = s.Username;
                z.MasterPasswordHash = s.MasterPasswordHash;

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
