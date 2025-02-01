namespace HeaderInjectorViaProxy.Core;

public enum ConsoleArguments { Positional, Option, Flag, }

public class ConsoleArgument
{
    private readonly string[] _args;
    private readonly HashSet<string> _positionalArgs;
    private readonly Dictionary<string, string> _options;
    private readonly HashSet<string> _flags;
    private bool _loaded;

    public ConsoleArgument(string[] args)
    {
        _args = args;
        _positionalArgs = new HashSet<string>();
        _options = new Dictionary<string, string>();
        _flags = new HashSet<string>();
        _loaded = false;
    }

    private void Parse()
    {
        if (_loaded == true)
            return;

        for (int i = 0; i < _args.Length; i++)
        {
            if (_args[i].StartsWith("--"))
            {
                // Named Argument / Option Argument
                if (i + 1 < _args.Length && !_args[i + 1].StartsWith("--") && !_args[i + 1].StartsWith("-"))
                {
                    _options.Add(_args[i], _args[i + 1]);
                    i++;
                }
                else
                {
                    // Flag Argument
                    _flags.Add(_args[i]);
                }
            }
            else if (_args[i].StartsWith("-"))
            {
                // Short Option Argument
                string key = _args[i].Substring(1);
                if (i + 1 < _args.Length && !_args[i + 1].StartsWith("--") && !_args[i + 1].StartsWith("-"))
                {
                    _options.Add(_args[i], _args[i + 1]);
                    i++;
                }
                else
                {
                    // Flag Argument
                    _flags.Add(_args[i]);
                }
            }
            else
            {
                // Positional Argument
                _positionalArgs.Add(_args[i]);
            }
        }

        _loaded = true;
    }

    public bool NoArguments { get => _args.Length == 0; }

    public bool Check(params (string, ConsoleArguments)[] arguments)
    {
        if (arguments is null)
            throw new ArgumentNullException(nameof(arguments));

        Parse();

        if (arguments.Length == 0)
            return _args.Length == arguments.Length;

        if (arguments.Length != _positionalArgs.Count + _flags.Count + _options.Count)
            return false;

        if (_positionalArgs.Count != arguments.Count(s => s.Item2 == ConsoleArguments.Positional))
            return false;

        if (_options.Count != arguments.Count(s => s.Item2 == ConsoleArguments.Option))
            return false;

        if (_flags.Count != arguments.Count(s => s.Item2 == ConsoleArguments.Flag))
            return false;

        bool valid = true;

        foreach (var arg in arguments)
        {
            if (arg.Item2 == ConsoleArguments.Positional)
                valid = valid && _positionalArgs.Contains(arg.Item1);
            else if (arg.Item2 == ConsoleArguments.Option)
                valid = valid && _options.ContainsKey(arg.Item1);
            else if (arg.Item2 == ConsoleArguments.Flag)
                valid = valid && _flags.Contains(arg.Item1);
        }

        return valid;
    }

    public string? GetOptionArg(string argument)
    {
        if (string.IsNullOrEmpty(argument))
            throw new ArgumentNullException(nameof(argument));

        return _options.ContainsKey(argument) ? _options[argument] : null;
    }

    public static (string, ConsoleArguments) Positional(string arg) => (arg, ConsoleArguments.Positional);
    public static (string, ConsoleArguments) Option(string arg) => (arg, ConsoleArguments.Option);
    public static (string, ConsoleArguments) Flag(string arg) => (arg, ConsoleArguments.Flag);
}
