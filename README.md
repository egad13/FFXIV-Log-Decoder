# FFXIV Log Decoder

A tool for decoding FFXIV chat log files into a human-readable format.

## Usage

### Command Line

To output decoded log entries to the console:
```
FFXIVLogDecoder.exe -i <input file path>
```

Options:
- `-i`, `--input` - **Required.** The log file, or directory of log files, to decode.

- `-o`, `--output` - The file or directory to output decoded logs to. If this argument is omitted, output will be directed to the console.
- `--help` - Display help.
- `--version` - Display version information.

## Roadmap

- [x] Decode the basic log structure
- [x] CLI
- [ ] GUI
- [ ] Reduce garbage characters around names, items, abilities, etc
