using CommandLine;	// https://github.com/commandlineparser/commandline/wiki/Getting-Started
using Pastel;		// https://github.com/silkfire/Pastel
using Expedition2.Scout;
using OfficeOpenXml.Style;

var parser = new Parser(with =>
{
	with.CaseSensitive = false;
	with.CaseInsensitiveEnumValues = true;
});

var result = parser.ParseArguments<CreateOptions, VerifyOptions>(args)
	.WithParsed<CreateOptions>(x => Engine.DoCreate(x))
	.WithParsed<VerifyOptions>(x => Engine.DoVerify(x));

if (result.Tag == ParserResultType.NotParsed)
{
	Console.WriteLine($"Error parsing options".Pastel("FF0000"));
}


/* scout
 * --report
 * 
 * 
 * 
 * 

/* 
 * Creates a new md5 hash file with the default name recursively in the current directory
 * scout create
 * 
 * Creates a new sha1 hash file with the default name recursively in the current directory
 * scout create --sha1
 * 
 * Creates a new sha512 hash file with the default name recursively in the current directory
 * scout create --sha512
 * 
 * Creates a new sha512 hash file with the default name recursively in the current directory with a detailed excel report
 * scout create --sha512 --report
 * 
 * Creates a new sha512 hash file with the default name recursively in the current directory with a simple text log report
 * scout create --sha512 --log
 * 
 * Creates a new sha512 hash file with the default name for current directory without recursion
 * scout create --sha512 --flat
 * 
 * Lists all discoverable hash files and asks which one to verify
 * scout verify
 * 
 * Verifies the first hash file found in the current directory
 * scout verify --auto
 * 
 * Verifies the named hash file and creates a detailed excel report
 * scout verify abc.md5 --report
 * 
 * Verifies the named hash file
 * scout verify filename.md5
 * 
  
 * scout update 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */

