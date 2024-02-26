using Tutorial.Authentification.MiniGame.Models;

namespace Tutorial.Authentification.MiniGame.Storage
{
    public class Accounts
    {
        private List<Account> _accounts;

        public Accounts()
        {
            _accounts = new List<Account>();
        }

        /// <summary>
        /// Если пользователь существует, то возращает его, если пользователь не найден, возвращает false
        /// </summary>
        /// <param name="login"></param>
        /// <returns>null - такого пользователя не существует</returns>
        public Account? GetAccount(string login)
        {
            return _accounts.FirstOrDefault(x => x.Login == login);
        }

        /// <summary>
        /// Добавляет пользователя в хранилище
        /// </summary>
        /// <param name="account"></param>
        /// <returns>false - невозможно добавить пользователя, так как он уже существует. true - пользователь успешно добавлен</returns>
        public bool AddAccount(string login, string password)
        {
            if (GetAccount(login) == null)
            {
                _accounts.Add(new Account() { Login = login, Password = password });
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавляет пользователя в хранилище
        /// </summary>
        /// <param name="account"></param>
        /// <returns>false - невозможно добавить пользователя, так как он уже существует. true - пользователь успешно добавлен</returns>
        public bool AddAccount(Account account)
        {
            if (GetAccount(account.Login) == null)
            {
                _accounts.Add(account);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удаляет аккаунт
        /// </summary>
        /// <param name="account"></param>
        /// <returns>false - пользователь не найден в хранилище и не был удалён, true - пользователь удалён</returns>
        public bool RemoveAccount(Account account)
        {
            return _accounts.Remove(account);
        }
    }
}
