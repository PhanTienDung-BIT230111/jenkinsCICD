using jenkinsCICD.Models;

namespace jenkinsCICD.Services
{
    public class MockDataService
    {
        private static List<User> _users = new();
        private static List<Group> _groups = new();
        private static List<Expense> _expenses = new();
        private static List<GroupMember> _groupMembers = new();
        private static int _userIdCounter = 1;
        private static int _groupIdCounter = 1;
        private static int _expenseIdCounter = 1;

        static MockDataService()
        {
            InitializeSampleData();
        }

        private static void InitializeSampleData()
        {
            // Tạo users mẫu
            _users.AddRange(new[]
            {
                new User { Id = 1, Name = "Nguyễn Văn An", Email = "an@example.com", PhoneNumber = "0901234567", CreatedAt = DateTime.Now.AddDays(-30) },
                new User { Id = 2, Name = "Trần Thị Bình", Email = "binh@example.com", PhoneNumber = "0912345678", CreatedAt = DateTime.Now.AddDays(-25) },
                new User { Id = 3, Name = "Lê Minh Cường", Email = "cuong@example.com", PhoneNumber = "0923456789", CreatedAt = DateTime.Now.AddDays(-20) },
                new User { Id = 4, Name = "Phạm Thu Dung", Email = "dung@example.com", PhoneNumber = "0934567890", CreatedAt = DateTime.Now.AddDays(-15) }
            });

            // Tạo groups mẫu
            _groups.AddRange(new[]
            {
                new Group { Id = 1, Name = "Team Văn phòng", Description = "Nhóm mua đồ ăn trưa văn phòng", CreatedByUserId = 1, CreatedAt = DateTime.Now.AddDays(-20), IsActive = true },
                new Group { Id = 2, Name = "Nhóm bạn thân", Description = "Mua sắm cuối tuần", CreatedByUserId = 2, CreatedAt = DateTime.Now.AddDays(-15), IsActive = true },
                new Group { Id = 3, Name = "Gia đình", Description = "Chi tiêu gia đình hàng tháng", CreatedByUserId = 3, CreatedAt = DateTime.Now.AddDays(-10), IsActive = true }
            });

            // Tạo group members mẫu
            _groupMembers.AddRange(new[]
            {
                new GroupMember { Id = 1, GroupId = 1, UserId = 1, IsAdmin = true, JoinedAt = DateTime.Now.AddDays(-20), IsActive = true },
                new GroupMember { Id = 2, GroupId = 1, UserId = 2, IsAdmin = false, JoinedAt = DateTime.Now.AddDays(-18), IsActive = true },
                new GroupMember { Id = 3, GroupId = 1, UserId = 3, IsAdmin = false, JoinedAt = DateTime.Now.AddDays(-15), IsActive = true },
                new GroupMember { Id = 4, GroupId = 2, UserId = 2, IsAdmin = true, JoinedAt = DateTime.Now.AddDays(-15), IsActive = true },
                new GroupMember { Id = 5, GroupId = 2, UserId = 4, IsAdmin = false, JoinedAt = DateTime.Now.AddDays(-12), IsActive = true },
                new GroupMember { Id = 6, GroupId = 3, UserId = 3, IsAdmin = true, JoinedAt = DateTime.Now.AddDays(-10), IsActive = true },
                new GroupMember { Id = 7, GroupId = 3, UserId = 4, IsAdmin = false, JoinedAt = DateTime.Now.AddDays(-8), IsActive = true }
            });

            // Tạo expenses mẫu
            _expenses.AddRange(new[]
            {
                new Expense { Id = 1, Title = "Cơm trưa nhóm", Description = "Đặt cơm cho team", Amount = 240000, ExpenseDate = DateTime.Now.AddDays(-5), Category = "Ăn uống", PaidByUserId = 1, GroupId = 1, CreatedAt = DateTime.Now.AddDays(-5) },
                new Expense { Id = 2, Title = "Cafe buổi chiều", Description = "Gọi cafe cho văn phòng", Amount = 150000, ExpenseDate = DateTime.Now.AddDays(-3), Category = "Ăn uống", PaidByUserId = 2, GroupId = 1, CreatedAt = DateTime.Now.AddDays(-3) },
                new Expense { Id = 3, Title = "Mua sắm cuối tuần", Description = "Đi siêu thị mua đồ", Amount = 850000, ExpenseDate = DateTime.Now.AddDays(-2), Category = "Mua sắm", PaidByUserId = 2, GroupId = 2, CreatedAt = DateTime.Now.AddDays(-2) },
                new Expense { Id = 4, Title = "Xăng xe", Description = "Đổ xăng xe máy", Amount = 120000, ExpenseDate = DateTime.Now.AddDays(-1), Category = "Đi lại", PaidByUserId = 3, GroupId = null, CreatedAt = DateTime.Now.AddDays(-1) },
                new Expense { Id = 5, Title = "Hóa đơn điện", Description = "Tiền điện tháng này", Amount = 450000, ExpenseDate = DateTime.Now, Category = "Tiện ích", PaidByUserId = 3, GroupId = 3, CreatedAt = DateTime.Now }
            });

            _userIdCounter = _users.Count + 1;
            _groupIdCounter = _groups.Count + 1;
            _expenseIdCounter = _expenses.Count + 1;

            // Thiết lập navigation properties
            foreach (var user in _users)
            {
                user.Expenses = _expenses.Where(e => e.PaidByUserId == user.Id).ToList();
                user.GroupMemberships = _groupMembers.Where(gm => gm.UserId == user.Id).ToList();
            }

            foreach (var group in _groups)
            {
                group.CreatedByUser = _users.First(u => u.Id == group.CreatedByUserId);
                group.Members = _groupMembers.Where(gm => gm.GroupId == group.Id).ToList();
                group.Expenses = _expenses.Where(e => e.GroupId == group.Id).ToList();
            }

            foreach (var expense in _expenses)
            {
                expense.PaidByUser = _users.First(u => u.Id == expense.PaidByUserId);
                if (expense.GroupId.HasValue)
                    expense.Group = _groups.First(g => g.Id == expense.GroupId);
            }

            foreach (var groupMember in _groupMembers)
            {
                groupMember.User = _users.First(u => u.Id == groupMember.UserId);
                groupMember.Group = _groups.First(g => g.Id == groupMember.GroupId);
            }
        }

        // User operations
        public List<User> GetAllUsers() => _users.ToList();

        public User? GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User CreateUser(User user)
        {
            user.Id = _userIdCounter++;
            user.CreatedAt = DateTime.Now;
            user.Expenses = new List<Expense>();
            user.GroupMemberships = new List<GroupMember>();
            user.ExpenseShares = new List<ExpenseShare>();
            _users.Add(user);
            return user;
        }

        public bool UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            _users.Remove(user);
            return true;
        }

        // Group operations
        public List<Group> GetAllActiveGroups() => _groups.Where(g => g.IsActive).ToList();

        public Group? GetGroupById(int id) => _groups.FirstOrDefault(g => g.Id == id);

        public Group CreateGroup(Group group)
        {
            group.Id = _groupIdCounter++;
            group.CreatedAt = DateTime.Now;
            group.IsActive = true;
            group.CreatedByUser = _users.First(u => u.Id == group.CreatedByUserId);
            group.Members = new List<GroupMember>();
            group.Expenses = new List<Expense>();
            _groups.Add(group);

            // Add creator as admin member
            var membership = new GroupMember
            {
                Id = _groupMembers.Count + 1,
                GroupId = group.Id,
                UserId = group.CreatedByUserId,
                User = group.CreatedByUser,
                Group = group,
                IsAdmin = true,
                JoinedAt = DateTime.Now,
                IsActive = true
            };
            _groupMembers.Add(membership);
            group.Members.Add(membership);

            return group;
        }

        public bool UpdateGroup(Group group)
        {
            var existingGroup = _groups.FirstOrDefault(g => g.Id == group.Id);
            if (existingGroup == null) return false;

            existingGroup.Name = group.Name;
            existingGroup.Description = group.Description;
            existingGroup.IsActive = group.IsActive;
            return true;
        }

        // Expense operations
        public List<Expense> GetAllExpenses() => _expenses.OrderByDescending(e => e.ExpenseDate).ToList();

        public List<Expense> GetRecentExpenses(int count = 5) => 
            _expenses.OrderByDescending(e => e.CreatedAt).Take(count).ToList();

        public Expense? GetExpenseById(int id) => _expenses.FirstOrDefault(e => e.Id == id);

        public Expense CreateExpense(Expense expense)
        {
            expense.Id = _expenseIdCounter++;
            expense.CreatedAt = DateTime.Now;
            expense.PaidByUser = _users.First(u => u.Id == expense.PaidByUserId);
            if (expense.GroupId.HasValue)
                expense.Group = _groups.FirstOrDefault(g => g.Id == expense.GroupId);
            expense.ExpenseShares = new List<ExpenseShare>();
            _expenses.Add(expense);
            return expense;
        }

        public bool UpdateExpense(Expense expense)
        {
            var existingExpense = _expenses.FirstOrDefault(e => e.Id == expense.Id);
            if (existingExpense == null) return false;

            existingExpense.Title = expense.Title;
            existingExpense.Description = expense.Description;
            existingExpense.Amount = expense.Amount;
            existingExpense.ExpenseDate = expense.ExpenseDate;
            existingExpense.Category = expense.Category;
            existingExpense.PaidByUserId = expense.PaidByUserId;
            existingExpense.GroupId = expense.GroupId;
            existingExpense.PaidByUser = _users.First(u => u.Id == expense.PaidByUserId);
            if (expense.GroupId.HasValue)
                existingExpense.Group = _groups.FirstOrDefault(g => g.Id == expense.GroupId);
            else
                existingExpense.Group = null;

            return true;
        }

        public bool DeleteExpense(int id)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null) return false;

            _expenses.Remove(expense);
            return true;
        }

        // Statistics
        public decimal GetTotalExpenses() => _expenses.Sum(e => e.Amount);

        public int GetActiveGroupsCount() => _groups.Count(g => g.IsActive);

        public List<User> GetUsersNotInGroup(int groupId)
        {
            var usersInGroup = _groupMembers
                .Where(gm => gm.GroupId == groupId && gm.IsActive)
                .Select(gm => gm.UserId)
                .ToList();

            return _users.Where(u => !usersInGroup.Contains(u.Id)).ToList();
        }
    }
}
