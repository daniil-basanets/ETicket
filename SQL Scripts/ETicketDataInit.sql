BEGIN TRAN

INSERT INTO [dbo].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
VALUES
(N'957c51d3-afa1-4a83-b02c-6e7614beab41',N'SuperUser',N'SUPERUSER',N'7206b815-3a81-4f43-b444-a02354709498'),
(N'a0ea3770-fb50-4afe-80e8-a919d308f263',N'Admin',N'ADMIN',N'395409d6-b695-4b55-a200-242ea37c785a');
GO

INSERT INTO [dbo].[AspNetUsers]([Id],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount])
VALUES
(N'046aa84a-cac7-45da-a90e-2d0733b2c498',N'Super@gmail.com',N'SUPER@GMAIL.COM',N'Super@gmail.com',N'SUPER@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEHZ30SKh4ZevNQSgILFei3FzejPbNZcDaDgFhjxThzMD6Er95xyM862jWa2yUOYO3A==',N'2HESRDISOYFUVKTZNXGFOXBF6UVPAHDZ',N'50bab245-3ca7-4e56-adb5-e9f342868695',NULL,0,0,NULL,1,0),
(N'48f79a75-67e3-40cf-afba-c6759e149958',N'Admin@gmail.com',N'ADMIN@GMAIL.COM',N'Admin@gmail.com',N'ADMIN@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEArWcLkSQ8aEhrjdvOAdVhvqZtY2sy41YtvjvMP3z1CJm/6+/67G9etw6PP18366gw==',N'2GR3SPKMCT4VCNEE7VPX7SPY4IUCYLGT',N'0466e415-5fd1-4397-ac56-a8e0cfba761b',NULL,0,0,NULL,1,0);
GO

INSERT INTO [dbo].[AspNetUserRoles]([UserId],[RoleId])
VALUES
(N'046aa84a-cac7-45da-a90e-2d0733b2c498',N'957c51d3-afa1-4a83-b02c-6e7614beab41'),
(N'48f79a75-67e3-40cf-afba-c6759e149958',N'a0ea3770-fb50-4afe-80e8-a919d308f263');
GO

INSERT INTO [dbo].[DocumentTypes]([Name])
VALUES
(N'Passport'),
(N'Student card'),
(N'Pension document');
GO

INSERT INTO [dbo].[Documents]([Id],[DocumentTypeId],[Number],[ExpirationDate],[IsValid])
VALUES
('E8CBB27E-3B3F-4BFD-9C65-4DFDD6AADA52',(SELECT Id FROM [DocumentTypes] AS p WHERE p.Name = 'Student card'),N'AD000325698','2020-04-04T00:00:00',1),
('7AE35EF4-2920-4A29-8EA1-924DD521F21C',(SELECT Id FROM [DocumentTypes] AS p WHERE p.Name = 'Passport'),N'ÂÂ110022','2040-07-04T00:00:00',1),
('8A216FA4-37B5-44E4-9118-B1BA4037BCEF',(SELECT Id FROM [DocumentTypes] AS p WHERE p.Name = 'Student card'),N'AA110344022',NULL,0),
('450235E1-D9B5-4996-9EAA-64B41C88CAFB',(SELECT Id FROM [DocumentTypes] AS p WHERE p.Name = 'Student card'),N'AA123465446','2020-04-25 00:00:00',0),
('91D467D6-8C95-4535-A0A6-676D942D1096',(SELECT Id FROM [DocumentTypes] AS p WHERE p.Name = 'Student card'),N'AB133131433','2020-11-12 00:00:00',1),
('6159B9B7-4C8E-4C8F-8C53-87AC51DC726B',(SELECT Id FROM [DocumentTypes] AS p WHERE p.Name = 'Pension document'),N'ÂÀ1131313','2020-04-25 00:00:00',0);
GO

INSERT INTO [dbo].[Privileges]([Name],[Coefficient])
VALUES
(N'Student',0.50),
(N'Pupil',0.75),
(N'Pensioner',0.00);
GO

INSERT INTO [dbo].[ETUsers]([Id],[FirstName],[LastName],[Phone],[Email],[DateOfBirth],[PrivilegeId],[DocumentId])
VALUES
('6F3E9D56-8DDA-427C-A194-1ECB73481E42',N'Daniil',N'Basanets',N'8245564855',NULL,'1995-04-01 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),'E8CBB27E-3B3F-4BFD-9C65-4DFDD6AADA52'),
('4C90236B-3D4B-4374-917F-410F52C01322',N'Diana',N'Pavlenko',N'+380998553258',NULL,'1999-03-16 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),'91D467D6-8C95-4535-A0A6-676D942D1096'),
('15AAB630-6CA4-41CA-A0F7-88147BA8708E',N'Daria',N'Nazarenko',N'+380998556754',N'daryan15646@gmail.com','2000-02-11 00:00:00',NULL,NULL),
('E76BCFED-C0A8-49BC-9B68-8B0E05A0CE88',N'Daria',N'Klimenko',N' +38099855675',N'daryan7@gmail.com','1945-05-11 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Pensioner'),'6159B9B7-4C8E-4C8F-8C53-87AC51DC726B'),
('3326A6A8-A090-4289-8A04-9F57F5BA907E',N'Aleksandr',N'Karpov',N'8805553535',NULL,'2000-01-01 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Pupil'),'7AE35EF4-2920-4A29-8EA1-924DD521F21C'),
('A0B956C9-4EEF-44A1-86E4-B5711A5EAAE0',N'Leonid',N'Verdon',N'+380998553258',NULL,'1975-06-17 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),'450235E1-D9B5-4996-9EAA-64B41C88CAFB'),
('A9746019-F436-4FE9-B421-D060505C31B5',N'Alice',N'Pavlenko',N'+380998556754',N'A_Pavlenko@gmail.com','1998-10-25 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),'8A216FA4-37B5-44E4-9118-B1BA4037BCEF'),
('498CCCCD-0608-46F7-A421-DE51A9B839F9',N'Ekaterina',N'Kosovaya',N'+380998556755',NULL,'1995-06-14 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),NULL),
('F6D19F1E-B294-4965-A86C-EA4FB993DAD1',N'Maksim',N'Popov',N'+380998696754',NULL,'1990-06-19 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),NULL),
('882DF8C3-91C9-42E2-B379-FFE91EF41DF3',N'Elena ',N'Baranenko',N'+380998556769',NULL,'1999-05-02 00:00:00',(SELECT Id FROM [Privileges] AS p WHERE p.Name = 'Student'),NULL);
GO

INSERT INTO [dbo].[TicketTypes]([TypeName],[DurationHours],[IsPersonal],[Price])
VALUES
(N'1 Hour',1,0,10.00),
(N'24 Hours',24,0,75.00),
(N'48 Hours',48,0,132.00),
(N'3 Days',72,0,175.00),
(N'7 Days',168,0,450.00),
(N'7 Days Personal',168,1,250.00),
(N'30 Days',720,1,1000.00),
(N'90 Days',2160,1,2500.00),
(N'1 Year',8760,1,7500.00);
GO

INSERT INTO [dbo].[TransactionHistory]([Id],[ReferenceNumber],[TotalPrice],[Date],[TicketTypeId],[Count])
VALUES
('C2EDF34D-E382-49FF-9F4C-CCC52276A0F0',N'0000929880810',30.00,'2020-04-13 19:59:36', (SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=1),4),
('3CF84319-F47B-4DF7-8FF5-1018250339B8',N'0000846579538',70.00,'2020-04-13 19:59:34',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=1),7),
('2A8B29E1-37E7-4145-A87F-1376911899F3',N'0000922970594',20.00,'2020-04-13 19:53:35',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=1),2),
('917801DF-EF8E-4B1A-B76A-21614DE13B6A',N'0000767943836',30.00,'2020-04-13 15:37:47',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=1),3),
('C2944EDF-8622-49DE-A299-28F37ADE930E',N'0000605105120',0.00,'2020-04-13 19:55:51',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=1),2),
('53BDBCD5-6333-4D52-B048-299453886B2D',N'0000869792983',30.00,'2020-04-13 18:43:16',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=1),3),
('372366F5-C921-416A-8B45-33B87C0F790A',N'0000860400290',105.00,'2020-04-13 15:39:50',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=24),3),
('D6D29979-93A3-4A5E-98DB-3837C1269DE6',N'0000941039601',110.00,'2020-04-13 19:51:15',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=48),2),
('3D9AE67D-FA71-45F1-AFBC-3A98F2AEEDE0',N'0000630253022',275.00,'2020-04-15 11:31:32',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=48),5),
('E5E21055-EE96-4FB4-AFFA-450583D248DB',N'0000514100777',165.00,'2020-04-13 18:53:43',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=48),3),
('25F6D2C8-2343-4000-9AD2-6ADA3067CEC1',N'0000026503734',240.00,'2020-04-13 19:04:57',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=72),3),
('2B5437AD-89B6-4B3D-96F6-7C10EBDCF546',N'0000942173556',430.00,'2020-04-14 11:59:18',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=168 AND p.IsPersonal=0),2),
('F0C0D82F-95A9-41A4-A52A-8C632E00CF73',N'0000935472310',975.00,'2020-04-15 13:25:26',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=168 AND p.IsPersonal=1),5),
('059174D9-0738-4184-8104-80DCC8F85BF7',N'0000798626364',5670.00,'2020-04-13 20:01:07',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=720),7),
('C92A06B8-F526-4C7E-93BB-9E43F1560586',N'0000798626364',4800.00,'2020-04-13 20:01:07',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=2160),2),
('77CF129E-32EB-439B-A237-114A8BBC7450',N'0000798626364',19000.00,'2020-04-13 20:01:07',(SELECT Id FROM [TicketTypes] AS p WHERE p.DurationHours=8760),2);
GO

UPDATE TransactionHistory
SET
	TotalPrice = (SELECT tt.Price*[Count] FROM TicketTypes AS tt WHERE tt.Id = TicketTypeId)
GO

/*******************************************
 * Ticket initialilize
 *******************************************/
DECLARE @TransactionID UNIQUEIDENTIFIER
DECLARE @TicketTypeID int
DECLARE @TicketCount INT
DECLARE @Datet DATETIME

DECLARE TransactionHistory_Cursor CURSOR FAST_FORWARD 
FOR
    SELECT th.Id,
           th.TicketTypeId,
           th.[Count],
           th.[Date]
    FROM   [TransactionHistory] th    

OPEN TransactionHistory_Cursor  
FETCH NEXT FROM TransactionHistory_Cursor INTO @TransactionID, @TicketTypeID, @TicketCount, @Datet   

IF @@FETCH_STATUS <> 0
    PRINT 'No transactions'       

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @counter INT = 0;

	WHILE @counter < @TicketCount
	BEGIN
		INSERT INTO Tickets
			(
			Id,
			TicketTypeId,
			CreatedUTCDate,
			UserID,
			TransactionHistoryId
			)
		VALUES
			(
			NEWID(),
			@TicketTypeID,
			@Datet,
			(SELECT TOP 1 Id FROM ETUsers ORDER BY NEWID()),
			@TransactionID
			)	   
	   
	   SET @counter = @counter + 1;
	END;

    FETCH NEXT FROM TransactionHistory_Cursor INTO @TransactionID, @TicketTypeID, @TicketCount, @Datet
END  

CLOSE TransactionHistory_Cursor  
DEALLOCATE TransactionHistory_Cursor  
 
COMMIT TRAN