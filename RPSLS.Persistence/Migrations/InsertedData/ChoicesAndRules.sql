INSERT INTO [dbo].[Choices] ([Id],[Name]) VALUES
	 (1,'Rock'),
	 (2,'Paper'),
	 (3,'Scissors'),
	 (4,'Lizard'),
	 (5,'Spock');
INSERT INTO [dbo].[Rules]
           ([Id],[WinnerChoiceId]
           ,[LooserChoiceId])
     VALUES
           (1,1,3),
		   (2,1,4),
		   (3,2,1),
		   (4,2,5),
		   (5,3,2),
		   (6,3,4),
		   (7,4,2),
		   (8,4,5),
		   (9,5,1),
		   (10,5,3);
