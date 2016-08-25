using System;

namespace Clarg
{
	public class ConsoleStringRenderer
	{
		public void WriteLine(ConsoleString value)
		{
			Write(value);
			Console.WriteLine();
		}

		public void Write(ConsoleString value)
		{
			var originalForeground = Console.ForegroundColor;
			var originalBackground = Console.BackgroundColor;

			foreach(var segment in value.Segments)
			{
				if(segment.Foreground.HasValue)
					Console.ForegroundColor = segment.Foreground.Value;

				if(segment.Background.HasValue)
					Console.BackgroundColor = segment.Background.Value;

				Console.Write(segment.Text);

				Console.ForegroundColor = originalForeground;
				Console.BackgroundColor = originalBackground;
			}
		}
	}
}
