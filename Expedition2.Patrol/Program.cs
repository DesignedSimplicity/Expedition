using Expedition2.Core;
using CommandLine;  // https://github.com/commandlineparser/commandline/wiki/Getting-Started
using Pastel;       // https://github.com/silkfire/Pastel

namespace Expedition2.Patrol
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var parser = new Parser(with =>
			{
				with.CaseSensitive = false;
				with.CaseInsensitiveEnumValues = true;
			});

			var engine = new Engine();

			string[] test = { "create", @"H:\", "--report" };
			var result = parser.ParseArguments<CreateOptions, VerifyOptions>(test)
				.WithParsed<CreateOptions>(x => engine.DoCreate(x))
				.WithParsed<VerifyOptions>(x => engine.DoVerify(x));

			if (result.Tag == ParserResultType.NotParsed)
			{
				Console.WriteLine($"Error parsing options".Pastel("FF0000"));
			}
		}
	}
}