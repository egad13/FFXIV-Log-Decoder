namespace FFXIVLogDecoderLib {
	public static class LogDecoder {
		public static IEnumerable<LogEntry> Decode(string path) {
			using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
			using BinaryReader br = new(fs);
			var header = new LogHeader(br);

			List<LogEntry> entries = new();
			foreach (var eOffset in header.entryOffsets) {
				entries.Add(new(header.contentOffset + eOffset, br));
			}
			return entries;
		}
	}
}
