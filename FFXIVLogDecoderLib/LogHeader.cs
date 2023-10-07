
namespace FFXIVLogDecoderLib {
	internal struct LogHeader {
		public uint bodyLength;
		public uint fileLength;
		public uint[] entryOffsets;
		public long contentOffset;

		internal LogHeader(BinaryReader log) {
			bodyLength = log.ReadUInt32();
			fileLength = log.ReadUInt32();
			int indexLength = (int)(fileLength - bodyLength);
			entryOffsets = new uint[indexLength];
			for (int i = 0; i < indexLength; i++) {
				entryOffsets[i] = log.ReadUInt32();
			}
			contentOffset = log.BaseStream.Position;
		}
	}
}
