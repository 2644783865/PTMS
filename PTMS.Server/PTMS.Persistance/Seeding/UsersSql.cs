using PTMS.Common;

namespace PTMS.Persistance.Seeding
{
    public static class UsersSql
    {
        public static string[] SqlStatements = new string[] {
            $@"
        INSERT INTO ""AppRole"" (""Name"", ""NormalizedName"", ""DisplayName"")
        VALUES ('{RoleNames.Administrator}', '{RoleNames.Administrator.ToUpper()}', _UTF8'Администратор');
",
             $@"
        INSERT INTO ""AppRole"" (""Name"", ""NormalizedName"", ""DisplayName"")
        VALUES ('{RoleNames.Dispatcher}', '{RoleNames.Dispatcher.ToUpper()}', _UTF8'Диспетчер');
",
             $@"
        INSERT INTO ""AppRole"" (""Name"", ""NormalizedName"", ""DisplayName"")
        VALUES ('{RoleNames.Transporter}', '{RoleNames.Transporter.ToUpper()}', _UTF8'Перевозчик');
",
             $@"
        INSERT INTO ""AppUser"" (
            ""UserName"",
            ""NormalizedUserName"",
            ""FirstName"",
            ""LastName"",
            ""MiddleName"",
            ""Email"",
            ""NormalizedEmail"",
            ""EmailConfirmed"",
            ""PasswordHash"",
            ""AccessFailedCount"",
            ""ConcurrencyStamp"",
            ""LockoutEnabled"",
            ""LockoutEnd"",
            ""PhoneNumber"",
            ""PhoneNumberConfirmed"",
            ""SecurityStamp"",
            ""TwoFactorEnabled"",
            ""Enabled"",
            ""Description"",
            ""ProjectId"")
        VALUES (
            'admin@test.com',
            'ADMIN@TEST.COM',
            _UTF8'Иван',
            _UTF8'Иванов',
            _UTF8'Иванович',
            'admin@test.com',
            'ADMIN@TEST.COM',
            1,
            'AQAAAAEAACcQAAAAECQQ5jCvhsKCrCKH5WmHU3gnzEYpV45hxmYy/jjcFBgPs32cDu/Z4BPONUA3vjFxqA==',
            0,
            'fc690367-d982-4ae9-986a-738a253552c9',
            0,
            NULL,
            NULL,
            1,
            '9f7cc9f8-6d77-4263-8114-2ec9bbea6373',
            0,
            1,
            _UTF8'Тестовый пользователь для старта системы',
            NULL);
",
             $@"
    INSERT INTO ""AppUserRole"" (""UserId"", ""RoleId"")
    VALUES (1, 1);
"
        };
    }
}
