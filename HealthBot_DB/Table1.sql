CREATE TABLE [dbo].[Users]
(
	[UUID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] TEXT NULL, 
    [Alias] TEXT NOT NULL, 
    [Chat ID] BIGINT NOT NULL, 
    [Age] INT NULL, 
    [Sex] TEXT NULL ,
    CHECK (SEX IN ('Male','Female')),
    [Weight] INT NULL, 
    [Doctor ID's] UNIQUEIDENTIFIER NULL, 
    [Diary Entry's] UNIQUEIDENTIFIER NULL, 
    [Subscription End] DATETIME NULL, 
    [Subscription Start] DATETIME NULL, 
    [Creation Date] DATETIME NOT NULL, 
    [Change Date] DATETIME NOT NULL, 
    [Destruction Date] DATETIME NOT NULL, 
    FOREIGN KEY ([Doctor ID's]) REFERENCES [Users]([UUID]),
    FOREIGN KEY ([Diary Entry's]) REFERENCES [Diary Entry's]([UUID])
)
