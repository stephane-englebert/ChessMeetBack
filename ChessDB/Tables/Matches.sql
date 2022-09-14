CREATE TABLE [dbo].[Matches]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[tournament_guid] NVARCHAR(255) NOT NULL,
	[white_id] INT NOT NULL,
	[black_id] INT NOT NULL,
	[round] INT NOT NULL,
	[result] NVARCHAR(10) NOT NULL DEFAULT 'notPlayed',
	CONSTRAINT FK_matches_members_white FOREIGN KEY(white_id) REFERENCES Members(id),
	CONSTRAINT FK_matches_members_black FOREIGN KEY(black_id) REFERENCES Members(id),
	CONSTRAINT FK_matches_tournaments FOREIGN KEY(tournament_guid) REFERENCES Tournaments(guid)
)
GO
ALTER TABLE [dbo].[Matches]
	ADD CONSTRAINT [CK_matches_result] CHECK(result IN('notPlayed','whiteWin','blackWin','draw'))
GO
