USE Master
GO

IF EXISTS (SELECT * FROM sys.databases WHERE NAME = 'VulnerableClientAdmin')
	ALTER DATABASE [VulnerableClientAdmin] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
IF EXISTS (SELECT * FROM sys.databases WHERE NAME = 'VulnerableClientAdmin')
	DROP DATABASE VulnerableClientAdmin
GO

CREATE DATABASE VulnerableClientAdmin
GO

USE VulnerableClientAdmin
GO

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_Id = OBJECT_ID(N'[dbo].[VulnerabilityStatus]') AND type IN (N'U'))

	BEGIN CREATE TABLE VulnerabilityStatus (
			VulnerabilityStatusId INT NOT NULL IDENTITY (1, 1),
			StatusName NVARCHAR(50) NOT NULL,
			CONSTRAINT PK_VulnerabilityStatus PRIMARY KEY (VulnerabilityStatusId)
		)
	END

INSERT INTO VulnerabilityStatus
	( StatusName )
VALUES 
	( 'Vulnerability Not Assessed' ),
	( 'Previously Considered Vulnerable' ),
	( 'Currently Considered Vulnerable' )


IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_Id = OBJECT_ID(N'[dbo].[Contacts]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE Contacts (
			ContactId INT NOT NULL IDENTITY (1, 1),
			Title NVARCHAR(50) NULL,
			FirstName NVARCHAR(100) NULL,
			MiddleName NVARCHAR(100) NULL,
			Surname NVARCHAR(100) NULL,
			Gender NVARCHAR(100) NULL,
			DateOfBirth DateTime NULL,
			DateOfDeath DateTime NULL,
			VulnerabilityStatusId INT NOT NULL,
			CONSTRAINT PK_Contacts PRIMARY KEY (ContactId),
			CONSTRAINT FK_Contacts_VulnerabilityStatus FOREIGN KEY (VulnerabilityStatusId)
				REFERENCES VulnerabilityStatus (VulnerabilityStatusId)
		)
	END

INSERT INTO Contacts
	( Title, FirstName, MiddleName, Surname, Gender, DateOfBirth, VulnerabilityStatusId )
VALUES
	( 'Mr', 'Jack', NULL, 'Daniels', 'Male', '1959/12/21', 1 ),
	( 'Mr', 'Jim', NULL, 'Beam', 'Male', '1960/3/11', 1 ),
	( 'Mr', 'Johnny', NULL, 'Walker', 'Male', '1958/2/21', 1 ),
	( 'Mrs', 'Southern', NULL, 'Comfort', 'Female', '1970/12/13', 1 ),
	( 'Ms', 'Char', NULL, 'Treuse', 'Female', '1985/05/04', 1 ),
	( 'Mr', 'R', NULL, 'Magnac', 'Male', '1985/09/26', 1 )


IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'vcadminoperations')
	BEGIN
		EXEC('CREATE SCHEMA vcadminoperations')
	END
GO

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[AuditObject]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.AuditObject (
			AuditObjectId INT NOT NULL IDENTITY (1, 1),
			ObjectId INT NOT NULL,
			ObjectType NVARCHAR(100) NOT NULL,
			ColumnName NVARCHAR(100) NOT NULL,
			PreviousValue NVARCHAR(100) NOT NULL,
			NewValue NVARCHAR(100) NOT NULL,
			ChangedDate DATETIME NOT NULL,
			ChangedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_AuditObject PRIMARY KEY (AuditObjectId)
		)
	END

IF NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[VulnerabilityReason]') AND type in (N'U'))

	BEGIN 
		CREATE TABLE vcadminoperations.VulnerabilityReason (
			VulnerabilityReasonId INT NOT NULL IDENTITY(1, 1),
			Reason NVARCHAR(100) NOT NULL,
			Description NVARCHAR(500) NULL,
			ReasonActive BIT NOT NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_VulnerabilityReason PRIMARY KEY (VulnerabilityReasonId)
		)
	END

IF NOT EXISTS (SELECT * FROM vcadminoperations.VulnerabilityReason
	WHERE Reason = 'Poor Health')

	BEGIN
		INSERT INTO vcadminoperations.VulnerabilityReason
			( Reason, ReasonActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Poor Health', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Physical Disability', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Mental Health', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Serious / Life Threatening Health Condition', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Suicidal', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Cognitive Disorder', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Physical Impairment', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Sensory Impairment', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Behavioural Impairment', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Financial Resilience', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Death / Bereavement', 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Life Event - Coping with Illness/ Injury', 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Life Event - Divorce/ Separation', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Old age / Care', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Emotional State', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Other', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Literacy', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Digital Literacy', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Low Financial capability', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Non - English speaking', 1 , GETDATE(), 'System', GETDATE(), 'System'),
			( 'Low Capability - Other', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[PreferredContactMethod]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.PreferredContactMethod (
			PreferredContactMethodId INT NOT NULL IDENTITY (1, 1),
			Method NVARCHAR (20) NOT NULL,
			MethodActive BIT NOT NULL,
			Description NVARCHAR(500) NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_PreferredContactMethod PRIMARY KEY (PreferredContactMethodId)
		)
	END

IF NOT EXISTS (SELECT * FROM vcadminoperations.PreferredContactMethod
	WHERE Method = 'Email')

	BEGIN
	   INSERT INTO vcadminoperations.PreferredContactMethod
			( Method, MethodActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Email', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Telephone', 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Written', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Any', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[SpecialRequirement]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.SpecialRequirement (
			SpecialRequirementId INT NOT NULL IDENTITY (1, 1),
			Requirement NVARCHAR (50) NOT NULL,
			RequirementActive BIT NOT NULL,
			Description NVARCHAR(500) NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_SpecialRequirement PRIMARY KEY (SpecialRequirementId)
		)
	END

IF NOT EXISTS (SELECT * FROM vcadminoperations.SpecialRequirement
	WHERE Requirement = 'Large Print Documents')

	BEGIN
		INSERT INTO vcadminoperations.SpecialRequirement
			( Requirement, RequirementActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Large Print Documents', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Braille Documents', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Audio Information', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[SourceOfAwareness]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.SourceOfAwareness (
			SourceOfAwarenessId INT NOT NULL IDENTITY (1, 1),
			Source NVARCHAR(50) NOT NULL,
			SourceActive BIT NOT NULL,
			Description NVARCHAR(500) NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_SourceOfAwareness PRIMARY KEY (SourceOfAwarenessId)
		)
	END

IF NOT EXISTS (SELECT * FROM vcadminoperations.SourceOfAwareness
	WHERE Source = 'Solicitor')

	BEGIN
		INSERT INTO vcadminoperations.SourceOfAwareness
			( Source, SourceActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Solicitor', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Family', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[VulnerabilityInformation]') AND type in (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.VulnerabilityInformation (
			VulnerabilityInformationId INT NOT NULL IDENTITY (1, 1),
			ContactId INT NOT NULL,
			IsInferred BIT NOT NULL,
			StatementAndComments NVARCHAR (MAX) NULL,
			VulnerabilitySignOffNotes NVARCHAR (MAX) NULL,
			SourceOfAwarenessId INT NOT NULL,
			ThirdPartyContact NVARCHAR (MAX) NULL,
			PreferredContactMethodId INT NOT NULL,
			PreferredContactDetails NVARCHAR (100) NULL,
			VulnerableClientHasMadeComplaint BIT NOT NULL,
			ComplaintOutcome NVARCHAR (MAX) NULL,
			ClientRequirementMonitoringNeed NVARCHAR (MAX) NOT NULL,
			RequiredActionByCompany NVARCHAR (MAX) NOT NULL,
			ResponsibilityOfCompletionOfNextAction NVARCHAR (MAX) NULL,
			CompletionDateOfAssociatedTask DATETIME NULL,
			SpecialRequirementId INT NULL,
			SpecialRequirementNotes NVARCHAR (MAX) NULL,
			DateOfNextReview DATETIME NULL,
			FeedbackProvidedToTeam NVARCHAR (MAX) NULL,
			CDOutcomeUnderstandingNeedsGoodOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeUnderstandingNeedsBadOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeStaffSkillsAndCapabilityGoodOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeStaffSkillsAndCapabilityBadOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeTakingPracticalActionsGoodOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeTakingPracticalActionsBadOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeMonitoringAndEvaluationGoodOutcomes NVARCHAR (MAX) NULL,
			CDOutcomeMonitoringAndEvaluationBadOutcomes NVARCHAR (MAX) NULL,
			DiaryDate DATETIME NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_VulnerabilityInformation PRIMARY KEY (VulnerabilityInformationId),
			CONSTRAINT FK_VulnerabilityInformation_SourceOfAwareness FOREIGN KEY (SourceOfAwarenessId)
				REFERENCES vcadminoperations.SourceOfAwareness (SourceOfAwarenessId),
			CONSTRAINT FK_VulnerabilityInformation_PreferredContactMethod FOREIGN KEY (PreferredContactMethodId)
				REFERENCES vcadminoperations.PreferredContactMethod (PreferredContactMethodId),
			CONSTRAINT FK_VulnerabilityInformation_Contacts FOREIGN KEY (ContactId)
				REFERENCES dbo.Contacts (ContactId),
			CONSTRAINT FK_VulnerabilityReason_SpecialRequirement FOREIGN KEY (SpecialRequirementId)
				REFERENCES vcadminoperations.SpecialRequirement (SpecialRequirementId)
		)
	END


IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[VulnerabilityNote]') AND type in (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.VulnerabilityNote (
			VulnerabilityNoteId INT NOT NULL IDENTITY (1, 1),
			VulnerabilityInformationId INT NOT NULL,
			Note NVARCHAR (MAX) NOT NULL,
			NoteDate DATETIME NOT NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_VulnerabilityNote PRIMARY KEY (VulnerabilityNoteId),
			CONSTRAINT FK_VulnerabilityNote_VulnerabilityInformation FOREIGN KEY (VulnerabilityInformationId)
				REFERENCES vcadminoperations.VulnerabilityInformation (VulnerabilityInformationId)
		)
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[TeamFeedback]') AND type in (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.TeamFeedback (
			TeamFeedbackId INT NOT NULL IDENTITY (1, 1),
			VulnerabilityInformationId INT NOT NULL,
			Feedback NVARCHAR (MAX) NOT NULL,
			FeedbackDate DATETIME NOT NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_TeamFeedback PRIMARY KEY (TeamFeedbackId),
			CONSTRAINT FK_TeamFeedback_VulnerabilityInformation FOREIGN KEY (VulnerabilityInformationId)
				REFERENCES vcadminoperations.VulnerabilityInformation (VulnerabilityInformationId)
		)
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[SavedPage]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.SavedPage (
			SavedPageId INT NOT NULL IDENTITY (1, 1),
			Title NVARCHAR(50) NOT NULL,
			Url NVARCHAR(256) NOT NULL,
			Notes NVARCHAR(500) NULL,
			IsExternal BIT NOT NULL,
			Owner NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_SavedPages PRIMARY KEY (SavedPageId)
		)
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminoperations].[Vulnerability]') AND type in (N'U'))

	BEGIN
		CREATE TABLE vcadminoperations.Vulnerability (
			VulnerabilityId INT NOT NULL IDENTITY (1, 1),
			VulnerabilityInformationId INT NOT NULL,
			VulnerabilityReasonId INT NOT NULL,
			VulnerabilityDateAdded DATETIME NOT NULL,
			Explanation NVARCHAR (500) NOT NULL,
			IsPermanent BIT NOT NULL,
			VulnerabilityDateRemoved DATETIME NULL,
			ReasonRemoved NVARCHAR(500) NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_Vulnerability PRIMARY KEY (VulnerabilityId),
			CONSTRAINT FK_Vulnerability_VulnerabilityInformation FOREIGN KEY (VulnerabilityInformationId)
				REFERENCES vcadminoperations.VulnerabilityInformation (VulnerabilityInformationId),
			CONSTRAINT FK_Vulnerability_VulnerabilityReason FOREIGN KEY (VulnerabilityReasonId)
				REFERENCES vcadminoperations.VulnerabilityReason (VulnerabilityReasonId)
		)
	END

-- Tables for AspNet Identity. These are in their own schema

IF NOT EXISTS (SELECT name FROM sys.schemas
	WHERE NAME = N'vcadminsecurity')

	BEGIN
		EXEC ('CREATE SCHEMA [vcadminsecurity]')
	END

/****** Object:  Table [vcadminsecurity].[AspNetRoles] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetRoles]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetRoles](
			[Id] [nvarchar](450) NOT NULL,
			[Name] [nvarchar](256) NULL,
			[NormalizedName] [nvarchar](256) NULL,
			[ConcurrencyStamp] [nvarchar](max) NULL,
		 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END

/****** Object:  Table [vcadminsecurity].[AspNetRoleClaims] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetRoleClaims]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetRoleClaims](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[RoleId] [nvarchar](450) NOT NULL,
			[ClaimType] [nvarchar](max) NULL,
			[ClaimValue] [nvarchar](max) NULL,
		 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
	WHERE CONSTRAINT_NAME ='FK_AspNetRoleClaims_AspNetRoles_RoleId')

	BEGIN
		ALTER TABLE [vcadminsecurity].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
		REFERENCES [vcadminsecurity].[AspNetRoles] ([Id])
		ON DELETE CASCADE
	END


ALTER TABLE [vcadminsecurity].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO

/****** Object:  Table [vcadminsecurity].[AspNetUsers] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetUsers]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetUsers](
			[Id] [nvarchar](450) NOT NULL,
			[UserName] [nvarchar](256) NULL,
			[NormalizedUserName] [nvarchar](256) NULL,
			[Email] [nvarchar](256) NULL,
			[NormalizedEmail] [nvarchar](256) NULL,
			[EmailConfirmed] [bit] NOT NULL,
			[PasswordHash] [nvarchar](max) NULL,
			[SecurityStamp] [nvarchar](max) NULL,
			[ConcurrencyStamp] [nvarchar](max) NULL,
			[PhoneNumber] [nvarchar](max) NULL,
			[PhoneNumberConfirmed] [bit] NOT NULL,
			[TwoFactorEnabled] [bit] NOT NULL,
			[LockoutEnd] [datetimeoffset](7) NULL,
			[LockoutEnabled] [bit] NOT NULL,
			[AccessFailedCount] [int] NOT NULL,
		 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END

/****** Object:  Table [vcadminsecurity].[AspNetUserClaims] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetUserClaims]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetUserClaims](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[UserId] [nvarchar](450) NOT NULL,
			[ClaimType] [nvarchar](max) NULL,
			[ClaimValue] [nvarchar](max) NULL,
		 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
	WHERE CONSTRAINT_NAME ='FK_AspNetUserClaims_AspNetUsers_UserId')

	BEGIN
		ALTER TABLE [vcadminsecurity].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
		REFERENCES [vcadminsecurity].[AspNetUsers] ([Id])
		ON DELETE CASCADE
	END

ALTER TABLE [vcadminsecurity].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO

/****** Object:  Table [vcadminsecurity].[AspNetUserTokens] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetUserTokens]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetUserTokens](
			[UserId] [nvarchar](450) NOT NULL,
			[LoginProvider] [nvarchar](128) NOT NULL,
			[Name] [nvarchar](128) NOT NULL,
			[Value] [nvarchar](max) NULL,
		 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
		(
			[UserId] ASC,
			[LoginProvider] ASC,
			[Name] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
	WHERE CONSTRAINT_NAME ='FK_AspNetUserTokens_AspNetUsers_UserId')

	BEGIN
	   ALTER TABLE [vcadminsecurity].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
		REFERENCES [vcadminsecurity].[AspNetUsers] ([Id])
		ON DELETE CASCADE
	END

ALTER TABLE [vcadminsecurity].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO


/****** Object:  Table [vcadminsecurity].[AspNetUserLogins] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetUserLogins]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetUserLogins](
			[LoginProvider] [nvarchar](128) NOT NULL,
			[ProviderKey] [nvarchar](128) NOT NULL,
			[ProviderDisplayName] [nvarchar](max) NULL,
			[UserId] [nvarchar](450) NOT NULL,
		 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
		(
			[LoginProvider] ASC,
			[ProviderKey] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
	WHERE CONSTRAINT_NAME ='FK_AspNetUserLogins_AspNetUsers_UserId')

	BEGIN
		ALTER TABLE [vcadminsecurity].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
		REFERENCES [vcadminsecurity].[AspNetUsers] ([Id])
		ON DELETE CASCADE
	END

ALTER TABLE [vcadminsecurity].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO


/****** Object:  Table [vcadminsecurity].[AspNetUserRoles] ******/
IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[vcadminsecurity].[AspNetUserRoles]') AND type in (N'U'))

	BEGIN
		CREATE TABLE [vcadminsecurity].[AspNetUserRoles](
			[UserId] [nvarchar](450) NOT NULL,
			[RoleId] [nvarchar](450) NOT NULL,
		 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
		(
			[UserId] ASC,
			[RoleId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
	WHERE CONSTRAINT_NAME ='FK_AspNetUserRoles_AspNetRoles_RoleId')

	BEGIN
		ALTER TABLE [vcadminsecurity].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
		REFERENCES [vcadminsecurity].[AspNetRoles] ([Id])
		ON DELETE CASCADE
	END
GO

ALTER TABLE [vcadminsecurity].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
	WHERE CONSTRAINT_NAME ='FK_AspNetUserRoles_AspNetUsers_UserId')

	BEGIN
		ALTER TABLE [vcadminsecurity].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
		REFERENCES [vcadminsecurity].[AspNetUsers] ([Id])
		ON DELETE CASCADE
	END

ALTER TABLE [vcadminsecurity].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]

-- Create a User Role and a SuperUser Role

IF NOT EXISTS (SELECT * FROM vcadminsecurity.AspNetRoles
	WHERE Name = 'User')

	BEGIN
		INSERT INTO [vcadminsecurity].[AspNetRoles]
			( Id, Name, NormalizedName )
		VALUES
			( NEWID(), 'User', 'USER' )
	END

IF NOT EXISTS (SELECT * FROM [vcadminsecurity].[AspNetRoles]
	WHERE Name = 'SuperUser')

	BEGIN
		INSERT INTO [vcadminsecurity].[AspNetRoles]
			( Id, Name, NormalizedName )
		VALUES
			( NEWID(), 'SuperUser', 'SUPERUSER' )
	END
GO

IF NOT EXISTS (SELECT * FROM [vcadminsecurity].[AspNetUsers]
	WHERE UserName = 'user@jaburrow.co.uk')

	BEGIN
		INSERT INTO [vcadminsecurity].[AspNetUsers]
			( Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount )
		VALUES
			( '57ae6823-9dec-4b91-950f-ef7e32d97490', 'user@jaburrow.co.uk', 'USER@JABURROW.CO.UK', 'user@jaburrow.co.uk', 'USER@JABURROW.CO.UK', 0, 'AQAAAAIAAYagAAAAEDmsbFHy66iVLxNQjdEbAZA3L+ByQ6aIBCa6LvO9+BfQ9yxjGO1//A8xpmjfcakjJQ==', 'PAJXVXKPVRFTTN66LW6H63WH6BJSBBJI', '0174a8da-6131-439a-a142-9096220b29c3', 0, 0, 1, 0 )
	END
GO

IF NOT EXISTS (SELECT * FROM [vcadminsecurity].[AspNetUserRoles]
	WHERE
		UserId = (SELECT Id FROM [vcadminsecurity].[AspNetUsers] WHERE UserName = 'user@jaburrow.co.uk')
	AND
		RoleId = (SELECT Id FROM [vcadminsecurity].[AspNetRoles] WHERE Name = 'User'))

	BEGIN
		INSERT INTO [vcadminsecurity].[AspNetUserRoles]
			( UserId, RoleId )
		VALUES
		( (SELECT Id FROM [vcadminsecurity].[AspNetUsers] WHERE UserName = 'user@jaburrow.co.uk'), (SELECT Id FROM [vcadminsecurity].[AspNetRoles] WHERE Name = 'User' ))
	END
GO

IF NOT EXISTS (SELECT * FROM [vcadminsecurity].[AspNetUsers]
	WHERE UserName = 'superuser@jaburrow.co.uk')

	BEGIN
		INSERT INTO [vcadminsecurity].[AspNetUsers]
			( Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount )
		VALUES
			( '7defd3d0-27d4-407d-aa7c-43a6b26b5a5f', 'superuser@jaburrow.co.uk', 'SUPERUSER@JABURROW.CO.UK', 'superuser@jaburrow.co.uk', 'SUPERUSER@JABURROW.CO.UK', 0, 'AQAAAAIAAYagAAAAEEV/wehqH6MYPBP1R32qFXrBBxKSt8W7AzKvTIV9HwXHmbA6MfSN1S31Mdd5gggHBw==', 'SCH4RKNSBBSABZERGKBBVESATLNPUJ6W', 'a83f043f-ade1-4287-9c24-74d88e35f7e9', 0, 0, 1, 0 )
	END
GO

IF NOT EXISTS (SELECT * FROM [vcadminsecurity].[AspNetUserRoles]
	WHERE
		UserId = (SELECT Id FROM [vcadminsecurity].[AspNetUsers] WHERE UserName = 'superuser@jaburrow.co.uk')
	AND
		RoleId = (SELECT Id FROM [vcadminsecurity].[AspNetRoles] WHERE Name = 'SuperUser'))

	BEGIN
		INSERT INTO [vcadminsecurity].[AspNetUserRoles]
			( UserId, RoleId )
		VALUES
		( (SELECT Id FROM [vcadminsecurity].[AspNetUsers] WHERE UserName = 'superuser@jaburrow.co.uk'), (SELECT Id FROM [vcadminsecurity].[AspNetRoles] WHERE Name = 'SuperUser' ))
	END
GO
