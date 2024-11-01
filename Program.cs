services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<AdminManager>();
services.AddScoped<BookManager>();
services.AddScoped<BorrowManager>();
services.AddScoped<ReaderManager>();
services.AddScoped<ReturnManager>();
services.AddScoped<UserManager>(); 