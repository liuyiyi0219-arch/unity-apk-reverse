protected void parse_instruction_size(ByteBuffer buffer, BHeader header, LHeaderParseState s) {
    // 1 byte instruction size
    int instructionSize = 0xFF & buffer.get();
    if(header.debug) {
      System.out.println("-- instruction size: " + instructionSize);
    }
    if(instructionSize != 4) {
      throw new IllegalStateException("The input chunk reports an unsupported instruction size: " + instructionSize + " bytes");
    }
  }
