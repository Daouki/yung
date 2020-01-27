using CommandLine;

namespace Yung.Options
{
    [Verb("r")]
    internal class RunOptions
    {
        [Value(0)]
        public string File { get; set; }
        
        [Option('i')]
        public bool RunInteractivePrompt { get; set; }
    }
}
