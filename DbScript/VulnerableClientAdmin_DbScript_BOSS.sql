-- T1-JA-105434
-- Create VulnerableClientAdmin tables in BOSS database and create and populate tables where necessary

CREATE SCHEMA VCAdmin
GO

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[AuditObject]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.AuditObject (
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
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[VulnerabilityReason]') AND type in (N'U'))

	BEGIN 
		CREATE TABLE VCAdmin.VulnerabilityReason (
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

IF NOT EXISTS (SELECT * FROM VCAdmin.VulnerabilityReason
	WHERE Reason = 'Poor Health')

	BEGIN
		INSERT INTO VCAdmin.VulnerabilityReason
			( Reason, ReasonActive, IsCore, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Poor Health', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Physical Disability', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Mental Health', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Serious / Life Threatening Health Condition', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Suicidal', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Cognitive Disorder', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Physical Impairment', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Sensory Impairment', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Behavioural Impairment', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Financial Resilience', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Death / Bereavement', 1, 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Life Event - Coping with Illness/ Injury', 1, 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Life Event - Divorce/ Separation', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Old age / Care', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Emotional State', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Life Event - Other', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Literacy', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Digital Literacy', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Low Financial capability', 1, 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Low Capability - Non - English speaking', 1 , 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Low Capability - Other', 1, 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[PreferredContactMethod]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.PreferredContactMethod (
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

IF NOT EXISTS (SELECT * FROM VCAdmin.PreferredContactMethod
	WHERE Method = 'Email')

	BEGIN
	   INSERT INTO VCAdmin.PreferredContactMethod
			( Method, MethodActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Email', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Telephone', 1, GETDATE(), 'System', GETDATE(), 'System'),
			( 'Written', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Any', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[SpecialRequirement]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.SpecialRequirement (
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

IF NOT EXISTS (SELECT * FROM VCAdmin.SpecialRequirement
	WHERE Requirement = 'Large Print Documents')

	BEGIN
		INSERT INTO VCAdmin.SpecialRequirement
			( Requirement, RequirementActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Large Print Documents', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Braille Documents', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Audio Information', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[SourceOfAwareness]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.SourceOfAwareness (
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

IF NOT EXISTS (SELECT * FROM VCAdmin.SourceOfAwareness
	WHERE Source = 'Solicitor')

	BEGIN
		INSERT INTO VCAdmin.SourceOfAwareness
			( Source, SourceActive, DateCreated, CreatedBy, DateLastUpdated, LastUpdatedBy )
		VALUES
			( 'Solicitor', 1, GETDATE(), 'System', GETDATE(), 'System' ),
			( 'Family', 1, GETDATE(), 'System', GETDATE(), 'System' )
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[VulnerabilityInformation]') AND type in (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.VulnerabilityInformation (
			VulnerabilityInformationId INT NOT NULL IDENTITY (1, 1),
			TenantId INT NOT NULL,
			ContactId INT NOT NULL,
			IsInferred BIT NOT NULL,
			StatementAndComments NVARCHAR (MAX) NULL,
			VulnerabilitySignOffNotes NVARCHAR (MAX) NULL,
			SourceOfAwarenessId INT NOT NULL,
			ThirdPartyContact NVARCHAR (MAX) NOT NULL,
			PreferredContactMethodId INT NOT NULL,
			PreferredContactDetails NVARCHAR (100) NULL,
			VulnerableClientHasMadeComplaint BIT NOT NULL,
			ComplaintOutcome NVARCHAR (MAX) NULL,
			ClientRequirementMonitoringNeed NVARCHAR (MAX) NOT NULL,
			RequiredActionByCompany NVARCHAR (MAX) NOT NULL,
			ResponsilbilityOfCompletionOfNextAction NVARCHAR (MAX) NULL,
			CompletionDateOfAssociatedTask DATETIME NULL,
			SpecialRequirementId INT NULL,
			SpecialRequirementNotes NVARCHAR (MAX) NULL,
			DateOfNextReview DATETIME NULL,
			FeedbackProvidedToTeam NVARCHAR (MAX) NULL,
			CDOutcomeUnderstandingNeeds NVARCHAR (MAX) NULL,
			CDOutcomeStaffSkillsAndCapability NVARCHAR (MAX) NULL,
			CDOutcomeTakingPracticalActions NVARCHAR (MAX) NULL,
			CDOutcomeMonitoringAndEvaluation NVARCHAR (MAX) NULL,
			DiaryDate DATETIME NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_VulnerabilityInformation PRIMARY KEY (VulnerabilityInformationId),
			CONSTRAINT FK_VulnerabilityInformation_SourceOfAwareness FOREIGN KEY (SourceOfAwarenessId)
				REFERENCES VCAdmin.SourceOfAwareness (SourceOfAawrenessId),
			CONSTRAINT FK_VulnerabilityInformation_PreferredContactMethod FOREIGN KEY (PreferredContactMethodId)
				REFERENCES VCAdmin.PreferredContactMethod (PreferredContactMethodId),
			CONSTRAINT FK_VulnerabilityInformation_Tenant FOREIGN KEY (TenantId)
				REFERENCES config.TenantSettings (Tenant),
			CONSTRAINT FK_VulnerabilityInformation_Contacts FOREIGN KEY (ContactId)
				REFERENCES dbo.Contacts (Contacts_ID),
			CONSTRAINT FK_VulnerabilityReason_SpecialRequirement FOREIGN KEY (SpecialRequirementId)
				REFERENCES VCAdmin.SpecialRequirement (SpecialRequirementId)
		)
	END


IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[VulnerabilityNote]') AND type in (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.VulnerabilityNote (
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
				REFERENCES VCAdmin.VulnerabilityInformation (VulnerabilityInformationId)
		)
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[SavedPage]') AND type IN (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.SavedPage (
			SavedPageId INT NOT NULL IDENTITY (1, 1),
			Title NVARCHAR(50) NOT NULL,
			Url NVARCHAR(256) NOT NULL,
			Notes NVARCHAR(500) NULL,
			IsExternal BIT NOT NULL,
			CONSTRAINT PK_SavedPages PRIMARY KEY (SavedPageId)
		)
	END

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[VCAdmin].[Vulnerability]') AND type in (N'U'))

	BEGIN
		CREATE TABLE VCAdmin.Vulnerability (
			VulnerabilityId INT NOT NULL IDENTITY (1, 1),
			VulnerabilityInformationId INT NOT NULL,
			VulnerabilityReasonId INT NOT NULL,
			VulnerabilityDateAdded DATETIME NOT NULL,
			AdditionalDetails NVARCHAR (500) NOT NULL,
			IsPermanent BIT NOT NULL,
			VulnerabilityDateRemoved DATETIME NULL,
			ReasonRemoved NVARCHAR(500) NULL,
			DateCreated DATETIME NOT NULL,
			CreatedBy NVARCHAR(100) NOT NULL,
			DateLastUpdated DATETIME NOT NULL,
			LastUpdatedBy NVARCHAR(100) NOT NULL,
			CONSTRAINT PK_Vulnerability PRIMARY KEY (VulnerabilityId),
			CONSTRAINT FK_Vulnerability_VulnerabilityInformation FOREIGN KEY (VulnerabilityInformationId)
				REFERENCES VCAdmin.VulnerabilityInformation (VulnerabilityInformationId),
			CONSTRAINT FK_Vulnerability_VulnerabilityReason FOREIGN KEY (VulnerabilityReasonId)
				REFERENCES VCAdmin.VulnerabilityReason (VulnerabilityReasonId)
		)
	END