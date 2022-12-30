using System;
using System.IO;
using System.Net;
using System.Net.Http;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Ajax.Utilities;

namespace CrystalReportWebAPI.Utilities
{
    public static class CrystalReport
    {
        public static HttpResponseMessage RenderReport(string reportPath, string reportFileName, string exportFilename)
        {
			try
			{
                var rd = new ReportDocument();
                
                rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName));
                rd.SetParameterValue(0, 3);
                rd.SetParameterValue(1, "99428");
                //rd.Load(@"C:\\Users\Abdul Rafay\\Documents\\Projects\\.NetCore\\APIs\\CrystalReportWebAPI\\CrystalReportWebAPI\\Reports\\CustomerLedgerReport.rpt");
                MemoryStream ms = new MemoryStream();
                rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName)+"4568.pdf");
                using (var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                {
                    stream.CopyTo(ms);
                }

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = exportFilename
                    };
                result.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                return result;
            }
			catch (System.Exception ex)
			{

				throw;
			}
        }
    }
}