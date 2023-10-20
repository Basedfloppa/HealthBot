CREATE TABLE [dbo].[Diary Entry's]
(
	[UUID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Data] DATETIME NOT NULL, 
    [Intake Item's] UNIQUEIDENTIFIER NULL, 
    [Author] UNIQUEIDENTIFIER NOT NULL, 
    [Type] TEXT NOT NULL, 
    [Heart Rate] INT NULL, 
    [Blood o2 saturation] INT NULL, 
    [Blood Preassute] TEXT NULL, 
    [Creation Date] DATETIME NOT NULL, 
    [Change Date] DATETIME NOT NULL, 
    [Destruction Date] DATETIME NOT NULL,
    FOREIGN KEY ([Author]) REFERENCES [Users]([UUID]),
    FOREIGN KEY ([Intake Item's]) REFERENCES [Intake Item's]([UUID])
)
