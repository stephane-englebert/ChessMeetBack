CREATE TABLE [dbo].[Registrations]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[player_id] INT NOT NULL,
	[tournament_guid] NVARCHAR(255) NOT NULL,
	CONSTRAINT FK_registrations_members FOREIGN KEY(player_id) REFERENCES Members(id),
	CONSTRAINT FK_registrations_tournaments FOREIGN KEY(tournament_guid) REFERENCES Tournaments(guid)
)
