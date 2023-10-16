namespace JobSparrow.JobsSites
{
	using System.Net;
	using System.Text.RegularExpressions;
	using System.Text;

	public class JobsBg : BaseJobsSite
	{
		public override string SiteSearch()
		{
			string pattern = @"<li class=""[0-9]*"".*>";
			string milstonePattern = @"<span class=""milestone-total"">[0-9]*<\/span>";

			string lastCountJobsString = string.Empty;
			int lastCountJobs = 0;
			int currentCountJobs = 0;
			int currentMilestone = 0;

			using (StreamReader sr = new StreamReader("JobsBg.txt"))
			{
				while ((lastCountJobsString = sr.ReadLine()) != null)
				{
					lastCountJobs = int.Parse(lastCountJobsString);
				}
			}

			using (WebClient client = new WebClient { Encoding = Encoding.UTF8 })
			{
				byte[] content = client.DownloadData(@$"https://www.jobs.bg/front_job_search.php?subm=1&categories%5B%5D=56&techs%5B%5D=C%23&it_level%5B%5D=1");
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				Encoding encoding1251 = Encoding.GetEncoding("windows-1251");
				var convertedBytes = Encoding.Convert(encoding1251, Encoding.UTF8, content);

				string htmlCode = Encoding.UTF8.GetString(convertedBytes);

				Regex milstoneRegex = new Regex(milstonePattern);

				if (milstoneRegex.Match(htmlCode).Success)
				{
					Match match = milstoneRegex.Match(htmlCode);
					currentCountJobs = int.Parse(match.Value.Substring(30, 2));
				}
				else
				{
					Regex regex = new Regex(pattern);
					MatchCollection matches = regex.Matches(htmlCode);

					currentCountJobs = matches.Count;
				}
			}

			if (lastCountJobs != currentCountJobs)
			{
				using (StreamWriter sw = new StreamWriter("JobsBg.txt"))
				{
					sw.WriteLine(currentCountJobs);
				}

				if (lastCountJobs < currentCountJobs)
				{
					return "New job in Jobs.bg: https://www.jobs.bg/front_job_search.php?subm=1&categories%5B%5D=56&techs%5B%5D=C%23&it_level%5B%5D=1";
				}
				else if (lastCountJobs > currentCountJobs)
				{
					return $"Less job in Jobs.bg {DateTime.Now}";
				}
			}

			return $"Checked Jobs.bg {DateTime.Now}";
		}
	}
}
