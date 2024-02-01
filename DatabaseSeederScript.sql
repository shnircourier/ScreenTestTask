
        CREATE DATABASE ScreenTestTask
        GO
        USE ScreenTestTask
        GO
        CREATE TABLE Users (
            [Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
            [Name] NVARCHAR(255) NOT NULL,
            [Password] NVARCHAR(255) NULL DEFAULT('qwerty1234')
        );
        GO
        CREATE TABLE Goods (
            [Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
            [Name] NVARCHAR(255) NOT NULL,
            [Price] DECIMAL(8, 2) NOT NULL
        );
        GO
        CREATE TABLE UserGoods (
            [Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
            [GoodsId] BIGINT NOT NULL,
            [UserId] BIGINT NOT NULL,
            [BoughtAt] DATE NOT NULL
        );
        GO
        ALTER TABLE UserGoods ADD CONSTRAINT UserGoods_userid_foreign FOREIGN KEY([UserId]) REFERENCES Users ([Id]);
        ALTER TABLE UserGoods ADD CONSTRAINT UserGoods_goodsid_foreign FOREIGN KEY([GoodsId]) REFERENCES Goods ([Id]);
    