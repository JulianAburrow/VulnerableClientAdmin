using QuestPDF.Fluent;

namespace VulnerableClientAdminUI.Shared.Methods;

public static class PDFMethods
{
    public static byte[] CreateSARPDF(VulnerabilityInformationModel vulnerabilityInformationModel)
    {
        var document = new PDFStrings(vulnerabilityInformationModel);
        return document.GeneratePdf();
    }
}
