namespace VulnerableClientAdminUI.Shared.Methods;

public static class CSVStrings
{
    /// <summary>
    /// Returns a CSV string for every CD Outcome passed in
    /// </summary>
    /// <param name="cdOutcomes"></param>
    /// <returns></returns>
    public static string CreateCDOutcomesCSVString(List<CDOutcomeModel> cdOutcomes)
    {
        var cdOutcomesSB = new StringBuilder();
        cdOutcomesSB.Append("FirstName,LastName,ColumnName,Outcome,EvaluationDate,CompletedBy");
        cdOutcomesSB.Append(Environment.NewLine);
        foreach (var cdOutcome in cdOutcomes)
        {
            cdOutcomesSB.Append($"{RemoveCommas(cdOutcome.FirstName)},{RemoveCommas(cdOutcome.LastName)},{cdOutcome.ColumnName},{cdOutcome.Outcome},{RemoveCommas(cdOutcome.EvaluationDate.ToShortDateString())},{RemoveCommas(cdOutcome.CompletedBy)}");
            cdOutcomesSB.Append(Environment.NewLine);
        }

        return cdOutcomesSB.ToString();
    }
    /// <summary>
    /// Returns a CSV string for every vulnerability passed in
    /// </summary>
    /// <param name="vulnerabilities"></param>
    /// <returns></returns>
    public static string CreateVulnerabilitiesCSVString(List<VulnerabilityInformationModel> vulnerabilities)
    {
        var vulnerabilitiesSB = new StringBuilder();
        vulnerabilitiesSB.Append("Title,First Name,Middle Names,Last Name,Gender,Date First Considered Vulnerable,");
        vulnerabilitiesSB.Append("Vulnerabilities,Status,Date No Longer Considered Vulnerable,Complaint Made,Complaint Outcome,");
        vulnerabilitiesSB.Append("Understanding Needs: Good Outcomes,Understanding Needs: BadOutcomes,");
        vulnerabilitiesSB.Append("Staff Skills And Capability: Good Outcomes,Staff Skills And Capability: Bad Outcomes,");
        vulnerabilitiesSB.Append("Taking Practical Actions: Good Outcomes,Taking Practical Actions: Bad Outcomes,");
        vulnerabilitiesSB.Append("Monitoring And Evaluation: Good Outcomes,Monitoring And Evaluation: Bad Outcomes,");
        vulnerabilitiesSB.Append("Notes,");
        vulnerabilitiesSB.Append("Feedback");
        vulnerabilitiesSB.Append(Environment.NewLine);
        foreach (var vulnerability in vulnerabilities)
        {
            vulnerabilitiesSB.Append($"{vulnerability.Contact.Title},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.Contact.FirstName)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.Contact.MiddleName)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.Contact.Surname)},");
            vulnerabilitiesSB.Append($"{vulnerability.Contact.Gender},");
            vulnerabilitiesSB.Append($"{vulnerability.Vulnerabilities.OrderBy(v => v.VulnerabilityDateAdded).First().VulnerabilityDateAdded.ToString("dd/MM/yyyy")},");
            foreach (var vulnerabilityReason in vulnerability.Vulnerabilities)
            {
                vulnerabilitiesSB.Append($"{RemoveCommas(vulnerabilityReason.VulnerabilityReason.Reason)} ({RemoveCommas(vulnerabilityReason.Explanation)}) ");
                vulnerabilitiesSB.Append($"({vulnerabilityReason.VulnerabilityDateAdded.ToString("dd/MM/yyyyy")} - {vulnerabilityReason.VulnerabilityDateRemoved?.ToString("dd/MM/yyyy")})-");
            }
            vulnerabilitiesSB.Remove(vulnerabilitiesSB.Length - 1, 1);
            vulnerabilitiesSB.Append($",{RemoveCommas(vulnerability.Contact.VulnerabilityStatus.StatusName)},");
            vulnerabilitiesSB.Append($"{GetDateNoLongerConsideredVulnerable(vulnerability.Vulnerabilities)},");
            vulnerabilitiesSB.Append($"{vulnerability.VulnerableClientHasMadeComplaint},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.ComplaintOutcome)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeUnderstandingNeedsGoodOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeUnderstandingNeedsBadOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeStaffSkillsAndCapabilityGoodOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeStaffSkillsAndCapabilityBadOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeTakingPracticalActionsGoodOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeTakingPracticalActionsBadOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeMonitoringAndEvaluationGoodOutcomes)},");
            vulnerabilitiesSB.Append($"{RemoveCommas(vulnerability.CDOutcomeMonitoringAndEvaluationBadOutcomes)},");
            foreach (var vulnerabilityNote in vulnerability.VulnerabilityNotes)
            {
                vulnerabilitiesSB.Append($"{RemoveCommas(vulnerabilityNote.Note)} ({vulnerabilityNote.NoteDate.ToShortDateString()})-");
            }
            vulnerabilitiesSB.Remove(vulnerabilitiesSB.Length - 1, 1);
            vulnerabilitiesSB.Append(",");
            foreach (var teamFeedback in vulnerability.TeamFeedbacks)
            {
                vulnerabilitiesSB.Append($"{RemoveCommas(teamFeedback.Feedback)} ({teamFeedback.FeedbackDate.ToShortDateString()})-");
            }
            vulnerabilitiesSB.Remove(vulnerabilitiesSB.Length - 1, 1);
            vulnerabilitiesSB.Append(Environment.NewLine);
        }

        return vulnerabilitiesSB.ToString();
    }

    /// <summary>
    /// Returns a CSV string for every vulnerable client passed in
    /// </summary>
    /// <param name="vulnerableClients"></param>
    /// <returns></returns>
    public static string CreateVulnerableClientsCSVString(List<VulnerableClientModel> vulnerableClients)
    {
        var vulnerableClientsSB = new StringBuilder();
        vulnerableClientsSB.Append("Title,First Name,Middle Names,Last Name,Gender,Date First Considered Vulnerable,");
        vulnerableClientsSB.Append("Vulnerabilities,Status,Date No Longer Considered Vulnerable,Complaint Made,Complaint Outcome,");
        vulnerableClientsSB.Append("Understanding Needs: Good Outcomes,Understanding Needs: BadOutcomes,");
        vulnerableClientsSB.Append("Staff Skills And Capability: Good Outcomes,Staff Skills And Capability: Bad Outcomes,");
        vulnerableClientsSB.Append("Taking Practical Actions: Good Outcomes,Taking Practical Actions: Bad Outcomes,");
        vulnerableClientsSB.Append("Monitoring And Evaluation: Good Outcomes,Monitoring And Evaluation: Bad Outcomes,");
        vulnerableClientsSB.Append("Notes,");
        vulnerableClientsSB.Append("Feedback");
        vulnerableClientsSB.Append(Environment.NewLine);
        foreach (var vulnerableClient in vulnerableClients)
        {
            vulnerableClientsSB.Append($"{vulnerableClient.Title},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.FirstName)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.MiddleName)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.Surname)},");
            vulnerableClientsSB.Append($"{vulnerableClient.Gender},");
            vulnerableClientsSB.Append($"{vulnerableClient.VulnerabilityInformation.Vulnerabilities.OrderBy(v => v.VulnerabilityDateAdded).First().VulnerabilityDateAdded.ToString("dd/MM/yyyy")},");
            foreach (var vulnerabilityReason in vulnerableClient.VulnerabilityInformation.Vulnerabilities)
            {
                vulnerableClientsSB.Append($"{RemoveCommas(vulnerabilityReason.VulnerabilityReason.Reason)} ({RemoveCommas(vulnerabilityReason.Explanation)}) ");
                vulnerableClientsSB.Append($"({vulnerabilityReason.VulnerabilityDateAdded.ToString("dd/MM/yyyyy")} - {vulnerabilityReason.VulnerabilityDateRemoved?.ToString("dd/MM/yyyy")})-");
            }
            vulnerableClientsSB.Remove(vulnerableClientsSB.Length - 1, 1);
            vulnerableClientsSB.Append($",{RemoveCommas(vulnerableClient.VulnerabilityStatus.StatusName)},");
            vulnerableClientsSB.Append($"{GetDateNoLongerConsideredVulnerable(vulnerableClient.VulnerabilityInformation.Vulnerabilities)},");
            vulnerableClientsSB.Append($"{vulnerableClient.VulnerabilityInformation.VulnerableClientHasMadeComplaint},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.ComplaintOutcome)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeUnderstandingNeedsGoodOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeUnderstandingNeedsBadOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeStaffSkillsAndCapabilityGoodOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeStaffSkillsAndCapabilityBadOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeTakingPracticalActionsGoodOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeTakingPracticalActionsBadOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeMonitoringAndEvaluationGoodOutcomes)},");
            vulnerableClientsSB.Append($"{RemoveCommas(vulnerableClient.VulnerabilityInformation.CDOutcomeMonitoringAndEvaluationBadOutcomes)},");
            if (vulnerableClient.VulnerabilityInformation.VulnerabilityNotes is not null)
            {
                foreach (var vulnerabilityNote in vulnerableClient.VulnerabilityInformation.VulnerabilityNotes)
                {
                    vulnerableClientsSB.Append($"{RemoveCommas(vulnerabilityNote.Note)} ({vulnerabilityNote.NoteDate.ToShortDateString()})-");
                }
                vulnerableClientsSB.Remove(vulnerableClientsSB.Length - 1, 1);
            }            
            vulnerableClientsSB.Append(",");
            if (vulnerableClient.VulnerabilityInformation.TeamFeedbacks is not null)
            {
                foreach (var teamFeedback in vulnerableClient.VulnerabilityInformation.TeamFeedbacks)
                {
                    vulnerableClientsSB.Append($"{RemoveCommas(teamFeedback.Feedback)} ({teamFeedback.FeedbackDate.ToShortDateString()})-");
                }
                vulnerableClientsSB.Remove(vulnerableClientsSB.Length - 1, 1);
            }            
            vulnerableClientsSB.Append(Environment.NewLine);
        }

        return vulnerableClientsSB.ToString();
    }

    public static string CreateAuditObjectCSVString(List<AuditObjectModel> auditObjects)
    {
        var auditObjectsSB = new StringBuilder();
        auditObjectsSB.Append("ColumnName,PreviousValue,CurrentValue,ChangedDate,ChangedBy");
        auditObjectsSB.Append(Environment.NewLine);
        foreach (var auditObject in auditObjects)
        {
            auditObjectsSB.Append($"{RemoveCommas(auditObject.ColumnName)},");
            auditObjectsSB.Append($"{RemoveCommas(auditObject.PreviousValue)},");
            auditObjectsSB.Append($"{RemoveCommas(auditObject.NewValue)},");
            auditObjectsSB.Append($"{RemoveCommas(auditObject.ChangedDate.ToString())},");
            auditObjectsSB.Append($"{RemoveCommas(auditObject.ChangedBy)}");
            auditObjectsSB.Append(Environment.NewLine);
        }               

        return auditObjectsSB.ToString();
    }

    private static string RemoveCommas(string? stringToCheck)
    {
        if (stringToCheck is null)
            return string.Empty;
        return stringToCheck.Replace(",", "-");
    }

    private static string GetDateNoLongerConsideredVulnerable(List<VulnerabilityModel> vulnerabilities)
    {
        vulnerabilities = vulnerabilities.OrderBy(v => v.VulnerabilityDateRemoved).ToList();
        return vulnerabilities.Last().VulnerabilityDateRemoved is not null
            ? vulnerabilities.Last().VulnerabilityDateRemoved.Value.ToString("dd/MM/yyyy")
            : string.Empty;
    }

    public static string CreateVulnerabilityStatusComparisonCSVString(List<VulnerabilityInformationModel> vulnerabilities)
    {
        var vulnerabilityStatusComparisonSB = new StringBuilder();
        vulnerabilityStatusComparisonSB.Append("Contact ID,First Name,Middle Name,Surname,Contact Table Vulnerability Status,VCA Application Vulnerability Status,Attention Required");
        vulnerabilityStatusComparisonSB.Append(Environment.NewLine);
        foreach (var vulnerability in vulnerabilities)
        {
            vulnerabilityStatusComparisonSB.Append($"{vulnerability.ContactId},");
            vulnerabilityStatusComparisonSB.Append($"{vulnerability.Contact.FirstName},");
            vulnerabilityStatusComparisonSB.Append($"{vulnerability.Contact.MiddleName},");
            vulnerabilityStatusComparisonSB.Append($"{vulnerability.Contact.Surname},");
            vulnerabilityStatusComparisonSB.Append($"{vulnerability.Contact.VulnerabilityStatus.StatusName},");
            var currentlyConsideredVulnerable = vulnerability.Vulnerabilities.Any(v => v.VulnerabilityDateRemoved is null);
            var stringToOutput = currentlyConsideredVulnerable
                ? "Currently considered vulnerable"
                : "Previously considered vulnerable";
            vulnerabilityStatusComparisonSB.Append($"{stringToOutput},");
            vulnerabilityStatusComparisonSB.Append(
                vulnerability.Contact.VulnerabilityStatus.StatusName != stringToOutput
                    ? "Yes"
                    : "No");
            vulnerabilityStatusComparisonSB.Append(Environment.NewLine);
        }

        return vulnerabilityStatusComparisonSB.ToString();
    }

    public static string CreateSARCSVString(VulnerabilityInformationModel v)
    {
        var sarSb = new StringBuilder();

        // ============================================================
        // SECTION 1 — CONTACT SUMMARY
        // ============================================================
        sarSb.AppendLine("CONTACT SUMMARY");
        sarSb.AppendLine($"First Name,{v.Contact.FirstName}");
        sarSb.AppendLine($"Last Name,{v.Contact.Surname}");
        sarSb.AppendLine($"Date of Birth,{v.Contact.DateOfBirth?.ToString("dd/MM/yyyy")}");
        sarSb.AppendLine($"Gender,{v.Contact.Gender}");
        sarSb.AppendLine($"Is Inferred,{v.IsInferred}");
        sarSb.AppendLine($"Date No Longer Considered Vulnerable,{v.DateNoLongerConsideredVulnerable?.ToString("dd/MM/yyyy") ?? "Case still open"}");
        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 2 — VULNERABILITY INFORMATION (scalar fields)
        // ============================================================
        sarSb.AppendLine("VULNERABILITY INFORMATION");
        sarSb.AppendLine($"Statement and Comments,{v.StatementAndComments}");
        sarSb.AppendLine($"Sign-Off Notes,{v.VulnerabilitySignOffNotes}");
        sarSb.AppendLine($"Third Party Contact,{v.ThirdPartyContact}");
        sarSb.AppendLine($"Preferred Contact Details,{v.PreferredContactDetails}");
        sarSb.AppendLine($"Complaint Made?,{v.VulnerableClientHasMadeComplaint}");
        sarSb.AppendLine($"Complaint Outcome,{v.ComplaintOutcome}");
        sarSb.AppendLine($"Monitoring Need,{v.ClientRequirementMonitoringNeed}");
        sarSb.AppendLine($"Required Action By Company,{v.RequiredActionByCompany}");
        sarSb.AppendLine($"Next Action Responsibility,{v.ResponsibilityOfCompletionOfNextAction}");
        sarSb.AppendLine($"Completion Date Of Associated Task,{v.CompletionDateOfAssociatedTask?.ToString("dd/MM/yyyy")}");
        sarSb.AppendLine($"Date Of Next Review,{v.DateOfNextReview?.ToString("dd/MM/yyyy")}");
        sarSb.AppendLine($"Diary Date,{v.DiaryDate?.ToString("dd/MM/yyyy")}");
        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 3 — NAVIGATION OBJECTS
        // ============================================================
        sarSb.AppendLine("NAVIGATION OBJECTS");
        sarSb.AppendLine($"Source Of Awareness,{v.SourceOfAwareness?.Description}");
        sarSb.AppendLine($"Preferred Contact Method,{v.PreferredContactMethod?.Description}");
        sarSb.AppendLine($"Special Requirement,{v.SpecialRequirement?.Description}");
        sarSb.AppendLine($"Special Requirement Notes,{v.SpecialRequirementNotes}");
        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 4 — CONSUMER DUTY OUTCOMES
        // ============================================================
        sarSb.AppendLine("CONSUMER DUTY OUTCOMES");
        sarSb.AppendLine($"Understanding Needs (Good),{v.CDOutcomeUnderstandingNeedsGoodOutcomes}");
        sarSb.AppendLine($"Understanding Needs (Bad),{v.CDOutcomeUnderstandingNeedsBadOutcomes}");
        sarSb.AppendLine($"Staff Skills (Good),{v.CDOutcomeStaffSkillsAndCapabilityGoodOutcomes}");
        sarSb.AppendLine($"Staff Skills (Bad),{v.CDOutcomeStaffSkillsAndCapabilityBadOutcomes}");
        sarSb.AppendLine($"Practical Actions (Good),{v.CDOutcomeTakingPracticalActionsGoodOutcomes}");
        sarSb.AppendLine($"Practical Actions (Bad),{v.CDOutcomeTakingPracticalActionsBadOutcomes}");
        sarSb.AppendLine($"Monitoring & Evaluation (Good),{v.CDOutcomeMonitoringAndEvaluationGoodOutcomes}");
        sarSb.AppendLine($"Monitoring & Evaluation (Bad),{v.CDOutcomeMonitoringAndEvaluationBadOutcomes}");
        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 5 — VULNERABILITIES TABLE
        // ============================================================
        sarSb.AppendLine("VULNERABILITIES");
        sarSb.AppendLine("Reason,Explanation,Date Added,Permanent,Date Removed");

        foreach (var item in v.Vulnerabilities)
        {
            sarSb.AppendLine(
                $"{item.VulnerabilityReason?.Description}," +
                $"{item.Explanation}," +
                $"{item.VulnerabilityDateAdded.ToString("dd/MM/yyyy")}," +
                $"{item.IsPermanent}," +
                $"{item.VulnerabilityDateRemoved?.ToString("dd/MM/yyyy")}"
            );
        }

        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 6 — NOTES TABLE
        // ============================================================
        sarSb.AppendLine("NOTES");
        sarSb.AppendLine("Note,Date");

        foreach (var note in v.VulnerabilityNotes)
        {
            sarSb.AppendLine(
                $"{note.Note}," +
                $"{note.NoteDate.ToString("dd/MM/yyyy")}"
            );
        }

        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 7 — TEAM FEEDBACK TABLE
        // ============================================================
        sarSb.AppendLine("TEAM FEEDBACK");
        sarSb.AppendLine("Feedback,Date");

        foreach (var fb in v.TeamFeedbacks)
        {
            sarSb.AppendLine(
                $"{fb.Feedback}," +
                $"{fb.FeedbackDate.ToString("dd/MM/yyyy")}"
            );
        }

        sarSb.AppendLine();
        sarSb.AppendLine();

        // ============================================================
        // SECTION 8 — AUDIT FIELDS
        // ============================================================
        sarSb.AppendLine("AUDIT");
        sarSb.AppendLine($"Date Created,{v.DateCreated.ToString("dd/MM/yyyy HH:mm")}");
        sarSb.AppendLine($"Date Last Updated,{v.DateLastUpdated.ToString("dd/MM/yyyy HH:mm")}");

        return sarSb.ToString();
    }
}
