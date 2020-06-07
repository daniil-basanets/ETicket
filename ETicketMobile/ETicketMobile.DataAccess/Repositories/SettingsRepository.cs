using System;
using System.IO;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using SQLite;

namespace ETicketMobile.DataAccess.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        #region Fields

        private readonly SQLiteAsyncConnection database;

        #endregion

        public SettingsRepository()
        {
            var databasePath = GetDatabasePath();

            database = new SQLiteAsyncConnection(databasePath);
            database.CreateTableAsync<Setting>();
        }

        private static string GetDatabasePath()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(basePath, "ETicketdb.db");

            return path;
        }

        public async Task<string> GetByNameAsync(string name)
        {
            var setting = await database
                .Table<Setting>()
                .FirstOrDefaultAsync(s => s.Name == name)
                .ConfigureAwait(false);

            if (setting == null)
                return null;

            return setting.Value;
        }

        public async Task SaveAsync(string name, string value)
        {
            var currentSetting = await database
                .Table<Setting>()
                .FirstOrDefaultAsync(s => s.Name == name);

            if (currentSetting == null)
            {
                await database.InsertAsync(
                    new Setting
                    {
                        Name = name,
                        Value = value
                    }
                );

                return;
            }

            currentSetting.Value = value;

            await database.UpdateAsync(currentSetting);
        }
    }
}