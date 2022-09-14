CREATE TABLE [dbo].[Members]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[role] NVARCHAR(10) NOT NULL DEFAULT 'player',
	[pseudo] NVARCHAR(50) NOT NULL UNIQUE,
	[email] NVARCHAR(150) NOT NULL UNIQUE,
	[password] NVARCHAR(150) NOT NULL,
	[birthdate] DATETIME2(7) NOT NULL,
	[gender] NVARCHAR(10) NOT NULL,
	[elo] INT NOT NULL DEFAULT 1200,
	CONSTRAINT PK_members PRIMARY KEY(id)
)
GO
ALTER TABLE [dbo].[Members]
	ADD CONSTRAINT [CK_members_role] CHECK (role IN ('admin','player'))
GO
ALTER TABLE [dbo].[Members]
	ADD CONSTRAINT [CK_members_birthdate] CHECK (birthdate BETWEEN '1920-01-01' AND GETDATE()) 
GO
ALTER TABLE [dbo].[Members]
	ADD CONSTRAINT [CK_members_gender] CHECK (gender IN ('male','female','other'))
GO
ALTER TABLE [dbo].[Members]
	ADD CONSTRAINT [CK_members_elo] CHECK (elo BETWEEN 0 AND 3000)
GO