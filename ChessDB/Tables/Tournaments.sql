CREATE TABLE [dbo].[Tournaments]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[guid] NVARCHAR(255) NOT NULL UNIQUE,
	[name] NVARCHAR(255) NOT NULL,
	[place] NVARCHAR(255),
	[players_min] INT NOT NULL DEFAULT 2,
	[players_max] INT NOT NULL DEFAULT 32,
	[elo_min] INT DEFAULT 0,
	[elo_max] INT DEFAULT 3000,
	[categories] NVARCHAR(255) NOT NULL,
	[status] NVARCHAR(50) NOT NULL DEFAULT 'waitingForPlayers',
	[current_round] INT NOT NULL DEFAULT 0,
	[women_only] BIT NOT NULL DEFAULT 0,
	[created_at] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
	[end_registration] DATETIME2(7) NOT NULL,
	[updated_at] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
	CONSTRAINT PK_tournaments PRIMARY KEY(id)
)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_players_min] CHECK(players_min BETWEEN 2 AND 32)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_players_max] CHECK(players_max BETWEEN 2 AND 32)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_players_minmax] CHECK(players_min <= players_max)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_elo_min] CHECK(elo_min BETWEEN 0 AND 3000)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_elo_max] CHECK(elo_max BETWEEN 0 AND 3000)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_elo_minmax] CHECK(elo_min <= elo_max)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_end_registration] CHECK(end_registration >= DATEADD(day,players_min,GETDATE()))
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_status] CHECK(status IN ('waitingForPlayers','inProgress','closed'))
GO