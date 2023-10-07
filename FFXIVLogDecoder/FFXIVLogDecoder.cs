using CommandLine;
using CommandLine.Text;
using FFXIVLogDecoderLib;
using System.Reflection;
using System.Text;

namespace FFXIVLogDecoder {

	internal class FFXIVLogDecoder {

		private static ParserResult<Options>? ParserResult { get; set; } = null;

		static void Main(string[] args) {
			Parser parser = new(with => with.HelpWriter = null);
			ParserResult = parser.ParseArguments<Options>(args);

			if (ParserResult.Tag == ParserResultType.Parsed) {
				ParserResult.WithParsed(Main);
			}
			else {
				ParserResult.WithNotParsed(errs => DisplayHelp(ParserResult, errs));
			}
		}

		static void Main(Options opts) {
			// Validation
			string[] logs = Array.Empty<string>();
			if (Directory.Exists(opts.InputPath)) {
				logs = Directory.GetFiles(opts.InputPath, "*.log", SearchOption.TopDirectoryOnly);
			}
			else if (File.Exists(opts.InputPath) && opts.InputPath.EndsWith(".log")) {
				logs = new[] { opts.InputPath };
			}
			else {
				Console.Error.WriteLine("\nERROR: Invalid input path. Input path must lead to an existing log file, or a directory containing log files.\n");
				DisplayHelp(ParserResult, Array.Empty<Error>());
			}

			// Work
			if (Directory.Exists(opts.OutputPath)) { // write indiv files to dir
				Console.WriteLine("Writing log contents to directory...");
				foreach (string log in logs) {
					string outname = Path.GetFileNameWithoutExtension(log) + ".txt",
						outpath = Path.Combine(opts.OutputPath, outname);

					using StreamWriter sw = new(outpath, false, Encoding.UTF8);
					foreach(var entry in LogDecoder.Decode(log)) {
						sw.WriteLine(entry);
					}
				}
				Console.WriteLine($"Wrote to {logs.Length} files in {Path.GetFullPath(opts.OutputPath)}");
			}
			else if (!string.IsNullOrEmpty(opts.OutputPath)) { // write all to one file
				Console.WriteLine("Writing all log contents to a single file...");
				using StreamWriter sw = new(opts.OutputPath, false, Encoding.UTF8);
				foreach (string log in logs) {
					foreach (var entry in LogDecoder.Decode(log)) {
						sw.WriteLine(entry);
					}
				}
				Console.WriteLine($"Log contents saved to file {Path.GetFullPath(opts.OutputPath)}");
			}
			else { // write to console
				foreach (string log in logs) {
					var entries = LogDecoder.Decode(log);
					foreach (var entry in entries) {
					//foreach (var entry in entries.Take(20)) {
						Console.WriteLine(entry);
					}
				}
			}
		}

		static Task DisplayHelp<T>(ParserResult<T>? result, IEnumerable<Error> errs) {
			var asm = Assembly.GetExecutingAssembly();
			string? desc = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

			var helpText = HelpText.AutoBuild(result, h =>
			{
				h.Heading = Console.Out.NewLine + h.Heading;
				if (!string.IsNullOrEmpty(desc)) {
					h.AddPreOptionsLine(desc);
				}
				return HelpText.DefaultParsingErrorsHandler(result, h);
			}, e => e);

			Console.WriteLine(helpText);

			return Task.CompletedTask;
		}
	}

	internal class Options {
		[Option('i', "input", Required = true, HelpText = "The log file, or directory of log files, to decode.")]
		public string? InputPath { get; set; }

		[Option('o', "output", Required = false, HelpText = "The file or directory to output decoded logs to. If this argument is omitted, output will be directed to the console.")]
		public string? OutputPath { get; set; }
	}
}
