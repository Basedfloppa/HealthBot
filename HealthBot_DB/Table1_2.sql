CREATE TABLE [dbo].[Intake Item's]
(
	[UUID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] TEXT NOT NULL, 
    [Calory Amount] INT NULL, 
    [Tags] TEXT NULL, 
    [State of matter] TEXT NOT NULL DEFAULT 'Solid', 
    [Weight] INT NULL,
    [Fats] INT NULL, 
    [Protein] INT NULL, 
    [Carbohydrates] INT NULL, 
    [Diary Entry] UNIQUEIDENTIFIER NOT NULL, 
    [Creation Date] DATETIME NOT NULL, 
    [Change Date] DATETIME NOT NULL, 
    [Destruction Date] DATETIME NOT NULL,
    CHECK ([State of matter] IN ('Solid','Liquid')),
    FOREIGN KEY ([Diary Entry]) REFERENCES [Diary Entry's]([UUID]) 
)
