using BankSystemPrototype.Domain.BankModel;
using System;

namespace BankSystemPrototype.Domain.UserModel
{
    /// <summary>
    /// Работник банка
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Банк, в котором зарегистрирован пользователь
        /// </summary>
        public Bank Bank { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Тип сотрудника
        /// </summary>
        public UserType Type { get; set; }
        /// <summary>
        /// Может ли пользователь добавлять клиентов
        /// </summary>
        public bool IsUserCanAddClient { get => Type == UserType.Manager; }
        /// <summary>
        /// Может ли пользователь удалять клиентов
        /// </summary>
        public bool IsUserCanRemoveClient { get => Type == UserType.Manager; }
        /// <summary>
        /// Может ли пользователь добавлять счета
        /// </summary>
        public bool IsUserCanAddAccount { get => Type == UserType.Manager || Type == UserType.Employee; }
        /// <summary>
        /// Может ли пользователь удалять счета
        /// </summary>
        public bool IsUserCanRemoveAccount { get => Type == UserType.Manager || Type == UserType.Employee; }
        /// <summary>
        /// Может ли пользователь добавлять пользователя
        /// </summary>
        public bool IsUserCanAddUser { get => Type == UserType.Admin; }
        /// <summary>
        /// Может ли пользователь добавлять пользователя
        /// </summary>
        public bool IsUserCanRemoveUser { get => Type == UserType.Admin; }
        /// <summary>
        /// Может ли просматривать клиентов
        /// </summary>
        public bool IsCanReadClients { get => Type == UserType.Manager || Type == UserType.Employee; }
        /// <summary>
        /// Может ли просматривать счета
        /// </summary>
        public bool IsCanReadAccounts { get => Type == UserType.Manager || Type == UserType.Employee; }
        /// <summary>
        /// Может ли смотреть остаток на счете
        /// </summary>
        public bool IsCanReadAccountMoney { get => Type == UserType.Manager; }
        /// <summary>
        /// Может ли переводить денежные средства клиентов
        /// </summary>
        public bool IsCanDoTransaction { get => Type == UserType.Manager || Type == UserType.Employee; }
        /// <summary>
        /// Признак показывает авторизован ли пользователь
        /// </summary>
        private bool _isAuhtorize;
        /// <summary>
        /// Признак показывает авторизован ли пользователь
        /// </summary>
        public bool IsAuhtorize => _isAuhtorize;
        /// <summary>
        /// Попытка авторизоваться
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void TryAuhtorize(string login, string password)
        {
            if (login != Login) throw new Exception("Некорректный логин");
            if (password != Password) throw new Exception("Некорректный пароль");
            _isAuhtorize = true;
        }
        /// <summary>
        /// Выход из системы
        /// </summary>
        public void Exit()
        {
            _isAuhtorize = false;
        }
    }

}
