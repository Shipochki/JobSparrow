namespace JobSparrow.JobsSites
{
	using System.Net;
	using System.Text.RegularExpressions;
	using System.Text;

	public class DevBg : BaseJobsSite
	{
		public override string SiteSearch()
		{
			string pattern = @"<div class=""job-list-item .*>";

			string lastCountJobsString = string.Empty;
			int lastCountJobs = 0;
			int currentCountJobs = 0;

			using (StreamReader sr = new StreamReader("DevBg.txt"))
			{
				while ((lastCountJobsString = sr.ReadLine()) != null)
				{
					lastCountJobs = int.Parse(lastCountJobsString);
				}
			}

			using (WebClient client = new WebClient { Encoding = Encoding.UTF8 })
			{
				byte[] content = client.DownloadData(@$"https://dev.bg/company/jobs/net/?_seniority=intern");
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				Encoding encoding1251 = Encoding.GetEncoding("windows-1251");
				var convertedBytes = Encoding.Convert(encoding1251, Encoding.UTF8, content);

				string htmlCode = Encoding.UTF8.GetString(convertedBytes);

				Regex regex = new Regex(pattern);
				MatchCollection matches = regex.Matches(htmlCode);

				currentCountJobs = matches.Count;
			}

			if (lastCountJobs != currentCountJobs)
			{
				using (StreamWriter sw = new StreamWriter("DevBg.txt"))
				{
					sw.WriteLine(currentCountJobs);
				}

				if (lastCountJobs < currentCountJobs)
				{
					return "New job in Dev.bg: https://dev.bg/company/jobs/net/?_seniority=intern";
				}
				else if (lastCountJobs > currentCountJobs)
				{
					return $"Less job in Dev.bg {DateTime.Now}";
				}
			}

			return $"Checked Dev.bg {DateTime.Now}";
		}
	}
}
