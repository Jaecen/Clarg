using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarg
{
	public class ConsoleString
	{
		public readonly IEnumerable<ConsoleStringSegment> Segments;

		public ConsoleString(params ConsoleStringSegment[] segments)
		{
			Segments = segments ?? Enumerable.Empty<ConsoleStringSegment>();
		}

		public ConsoleString(IEnumerable<ConsoleStringSegment> segments)
		{
			Segments = segments ?? Enumerable.Empty<ConsoleStringSegment>();
		}

		public ConsoleString(IEnumerable<ConsoleString> values)
		{
			Segments = (values ?? Enumerable.Empty<ConsoleString>())
				.SelectMany(value => value.Segments)
				.ToArray();
		}

		public int GetLength()
			=> Segments.Sum(segment => segment.Text.Length);

		public ConsoleString PadRight(int totalWidth, char paddingCharacter = ' ')
			=> GetLength() >= totalWidth
				? this
				: this + new string(paddingCharacter, totalWidth - GetLength());

		public static ConsoleString Join(ConsoleStringSegment separator, IEnumerable<ConsoleString> values)
		{
			var valuesCount = values.Count();

			if(valuesCount == 0)
				return new ConsoleString();

			if(valuesCount == 1)
				return values.First();

			var joinedSegments = Enumerable.Empty<ConsoleStringSegment>();

			// Copy all but the last element with a separator
			for(var index = 0; index < valuesCount - 1; index++)
				joinedSegments = joinedSegments
					.Concat(values.ElementAt(index).Segments)
					.Concat(new[] { separator });

			// Copy the last element without a separator
			joinedSegments = joinedSegments.Concat(values.ElementAt(valuesCount - 1).Segments);

			return new ConsoleString(joinedSegments);
		}

		public ConsoleString Colored(ConsoleColor? foreground = null, ConsoleColor? background = null)
			=> new ConsoleString(Segments
				.Select(segment => new ConsoleStringSegment(
					segment.Text,
					foreground ?? segment.Foreground,
					background ?? segment.Background)));

		public override string ToString()
			=> string.Join(
				string.Empty,
				Segments.SelectMany(segment => segment.Text));

		public static ConsoleString operator +(ConsoleString left, ConsoleStringSegment right)
			=> new ConsoleString(left
				.Segments
				.Concat(new[] { right })
				.ToArray());

		public static ConsoleString operator +(ConsoleString left, string right)
			=> new ConsoleString(left
				.Segments
				.Concat(new[] { new ConsoleStringSegment(right) })
				.ToArray());

		public static ConsoleString operator +(ConsoleString left, ConsoleString right)
			=> new ConsoleString(left
				.Segments
				.Concat(right.Segments)
				.ToArray());

		public static implicit operator ConsoleString(string text)
			=> new ConsoleString(new ConsoleStringSegment(text));

		public static implicit operator ConsoleString(ConsoleStringSegment segment)
			=> new ConsoleString(segment);
	}

	public class ConsoleStringSegment
	{
		public readonly string Text;
		public readonly ConsoleColor? Foreground;
		public readonly ConsoleColor? Background;

		public ConsoleStringSegment(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
		{
			Text = text ?? string.Empty;
			Foreground = foreground;
			Background = background;
		}

		public static implicit operator ConsoleStringSegment(string text)
			=> new ConsoleStringSegment(text);

		public override string ToString()
			=> Text;
	}
}
