CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);
GO

INSERT INTO Users (Username, Password, Email) 
VALUES ('caoquang284', '123456', 'rinquanq05@gmail.com');
GO

select * from Users;