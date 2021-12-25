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
    }

}
