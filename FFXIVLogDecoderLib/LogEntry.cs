using System.Text;

namespace FFXIVLogDecoderLib {
	public struct LogEntry {
		public uint timestamp;
		public ushort eventType;
		public ushort unknown;
		public string message;

		internal LogEntry(long offset, BinaryReader log) {
			timestamp = log.ReadUInt32();
			eventType = log.ReadUInt16();
			unknown = log.ReadUInt16();

			message = Encoding.UTF8.GetString(
				log.ReadBytes((int)(offset - log.BaseStream.Position))
			);
		}

		public readonly DateTime Timestamp() {
			return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
		}

		public override readonly string ToString() {
			return $"{Timestamp()} UTC | {eventType} | {message}";
		}
	}
}
