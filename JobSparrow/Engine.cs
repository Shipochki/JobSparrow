namespace JobSparrow
{
	using JobSparrow.JobsSites;
	using System.Media;
	using System.Text;

	public static class Engine
	{
		public static void Run()
		{
			Console.OutputEncoding = Encoding.UTF8;

			JobsBg jobsBg = new JobsBg();
			DevBg devBg = new DevBg();

			while (true)
			{
				string jobsBgResult = string.Empty;
				string devBgResult = string.Empty;

				try
				{
					jobsBgResult = jobsBg.SiteSearch();
				}
				catch (Exception)
				{
					Console.WriteLine("Problem in SiteSearch JobsBg");
					SoundPlayer player = new SoundPlayer(@"D:\JobSparrow\JobSparrow\Sounds\ProblemJobsBgSound.wav");
					player.Play();
				}

				try
				{
					devBgResult = devBg.SiteSearch();
				}
				catch (Exception)
				{
					Console.WriteLine("Problem in SiteSearch DevBg");
					SoundPlayer player = new SoundPlayer(@"D:\JobSparrow\JobSparrow\Sounds\ProblemDevBgSound.wav");
					player.Play();
				}

				Console.WriteLine(jobsBgResult);
				Console.WriteLine(devBgResult);
				Console.WriteLine();

				if (jobsBgResult.Length > 65)
				{
					SoundPlayer player = new SoundPlayer(@"D:\JobSparrow\JobSparrow\Sounds\JobsBgSound.wav");
					player.Play();
				}
				else if( devBgResult.Length > 65)
				{
					SoundPlayer player = new SoundPlayer(@"D:\JobSparrow\JobSparrow\Sounds\DevBgSound.wav");
					player.Play();
				}


				Thread.Sleep(15 * 64000);
			}
		}
	}
}
