using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace VulnerableClientAdminUI.Shared.Methods;

public class PDFStrings(VulnerabilityInformationModel vulnerabilityInformationModel) : IDocument
{
    public readonly VulnerabilityInformationModel _vulnerabilityInformationModel = vulnerabilityInformationModel;

    public DocumentMetadata GetMetadata() => new()
    {
        Title = "SAR Export"
    };

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);

            page.Header().Text("Subject Access Request Export")
                .FontSize(20)
                .Bold();

            page.Content().Column(col =>
            {
                // ============================================================
                // SECTION 1 — CONTACT SUMMARY
                // ============================================================
                col.Item().PaddingBottom(10).Text("CONTACT SUMMARY").FontSize(14).Bold();

                col.Item().Text($"First Name: {_vulnerabilityInformationModel.Contact.FirstName}");
                col.Item().Text($"Last Name: {_vulnerabilityInformationModel.Contact.Surname}");
                col.Item().Text($"Date of Birth: {_vulnerabilityInformationModel.Contact.DateOfBirth?.ToString("dd/MM/yyyy")}");
                col.Item().Text($"Gender: {_vulnerabilityInformationModel.Contact.Gender}");
                col.Item().Text($"Is Inferred: {_vulnerabilityInformationModel.IsInferred}");
                col.Item().Text($"Date No Longer Considered Vulnerable: {_vulnerabilityInformationModel.DateNoLongerConsideredVulnerable?.ToString("dd/MM/yyyy") ?? "Case still open"}");

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 2 — VULNERABILITY INFORMATION
                // ============================================================
                col.Item().Text("VULNERABILITY INFORMATION").FontSize(14).Bold();

                col.Item().Text($"Statement and Comments: {_vulnerabilityInformationModel.StatementAndComments}");
                col.Item().Text($"Sign-Off Notes: {_vulnerabilityInformationModel.VulnerabilitySignOffNotes}");
                col.Item().Text($"Third Party Contact: {_vulnerabilityInformationModel.ThirdPartyContact}");
                col.Item().Text($"Preferred Contact Details: {_vulnerabilityInformationModel.PreferredContactDetails}");
                col.Item().Text($"Complaint Made?: {_vulnerabilityInformationModel.VulnerableClientHasMadeComplaint}");
                col.Item().Text($"Complaint Outcome: {_vulnerabilityInformationModel.ComplaintOutcome}");
                col.Item().Text($"Monitoring Need: {_vulnerabilityInformationModel.ClientRequirementMonitoringNeed}");
                col.Item().Text($"Required Action By Company: {_vulnerabilityInformationModel.RequiredActionByCompany}");
                col.Item().Text($"Next Action Responsibility: {_vulnerabilityInformationModel.ResponsibilityOfCompletionOfNextAction}");
                col.Item().Text($"Completion Date Of Associated Task: {_vulnerabilityInformationModel.CompletionDateOfAssociatedTask?.ToString("dd/MM/yyyy")}");
                col.Item().Text($"Date Of Next Review: {_vulnerabilityInformationModel.DateOfNextReview?.ToString("dd/MM/yyyy")}");
                col.Item().Text($"Diary Date: {_vulnerabilityInformationModel.DiaryDate?.ToString("dd/MM/yyyy")}");

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 3 — NAVIGATION OBJECTS
                // ============================================================
                col.Item().Text("NAVIGATION OBJECTS").FontSize(14).Bold();

                col.Item().Text($"Source Of Awareness: {_vulnerabilityInformationModel.SourceOfAwareness?.Description}");
                col.Item().Text($"Preferred Contact Method: {_vulnerabilityInformationModel.PreferredContactMethod?.Description}");
                col.Item().Text($"Special Requirement: {_vulnerabilityInformationModel.SpecialRequirement?.Description}");
                col.Item().Text($"Special Requirement Notes: {_vulnerabilityInformationModel.SpecialRequirementNotes}");

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 4 — CONSUMER DUTY OUTCOMES
                // ============================================================
                col.Item().Text("CONSUMER DUTY OUTCOMES").FontSize(14).Bold();

                col.Item().Text($"Understanding Needs (Good): {_vulnerabilityInformationModel.CDOutcomeUnderstandingNeedsGoodOutcomes}");
                col.Item().Text($"Understanding Needs (Bad): {_vulnerabilityInformationModel.CDOutcomeUnderstandingNeedsBadOutcomes}");
                col.Item().Text($"Staff Skills (Good): {_vulnerabilityInformationModel.CDOutcomeStaffSkillsAndCapabilityGoodOutcomes}");
                col.Item().Text($"Staff Skills (Bad): {_vulnerabilityInformationModel.CDOutcomeStaffSkillsAndCapabilityBadOutcomes}");
                col.Item().Text($"Practical Actions (Good): {_vulnerabilityInformationModel.CDOutcomeTakingPracticalActionsGoodOutcomes}");
                col.Item().Text($"Practical Actions (Bad): {_vulnerabilityInformationModel.CDOutcomeTakingPracticalActionsBadOutcomes}");
                col.Item().Text($"Monitoring & Evaluation (Good): {_vulnerabilityInformationModel.CDOutcomeMonitoringAndEvaluationGoodOutcomes}");
                col.Item().Text($"Monitoring & Evaluation (Bad): {_vulnerabilityInformationModel.CDOutcomeMonitoringAndEvaluationBadOutcomes}");

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 5 — VULNERABILITIES TABLE
                // ============================================================
                col.Item().Text("VULNERABILITIES").FontSize(14).Bold();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                    });

                    // Header row
                    table.Header(header =>
                    {
                        header.Cell().Text("Reason").Bold();
                        header.Cell().Text("Explanation").Bold();
                        header.Cell().Text("Date Added").Bold();
                        header.Cell().Text("Permanent").Bold();
                        header.Cell().Text("Date Removed").Bold();
                    });

                    // Data rows
                    foreach (var item in _vulnerabilityInformationModel.Vulnerabilities)
                    {
                        table.Cell().Text(item.VulnerabilityReason?.Description);
                        table.Cell().Text(item.Explanation);
                        table.Cell().Text(item.VulnerabilityDateAdded.ToString("dd/MM/yyyy"));
                        table.Cell().Text(item.IsPermanent.ToString());
                        table.Cell().Text(item.VulnerabilityDateRemoved?.ToString("dd/MM/yyyy"));
                    }
                });

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 6 — NOTES TABLE
                // ============================================================
                col.Item().Text("NOTES").FontSize(14).Bold();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Note").Bold();
                        header.Cell().Text("Date").Bold();
                    });

                    foreach (var note in _vulnerabilityInformationModel.VulnerabilityNotes)
                    {
                        table.Cell().Text(note.Note);
                        table.Cell().Text(note.NoteDate.ToString("dd/MM/yyyy"));
                    }
                });

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 7 — TEAM FEEDBACK TABLE
                // ============================================================
                col.Item().Text("TEAM FEEDBACK").FontSize(14).Bold();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Feedback").Bold();
                        header.Cell().Text("Date").Bold();
                    });

                    foreach (var fb in _vulnerabilityInformationModel.TeamFeedbacks)
                    {
                        table.Cell().Text(fb.Feedback);
                        table.Cell().Text(fb.FeedbackDate.ToString("dd/MM/yyyy"));
                    }
                });

                col.Item().PaddingVertical(10);


                // ============================================================
                // SECTION 8 — AUDIT FIELDS
                // ============================================================
                col.Item().Text("AUDIT").FontSize(14).Bold();

                col.Item().Text($"Date Created: {_vulnerabilityInformationModel.DateCreated:dd/MM/yyyy HH:mm}");
                col.Item().Text($"Date Last Updated: {_vulnerabilityInformationModel.DateLastUpdated:dd/MM/yyyy HH:mm}");
            });

            page.Footer().AlignCenter().Text($"Generated on {DateTime.Now:dd/MM/yyyy HH:mm}");
        });
    }

}
