protected void parse_instruction_size(ByteBuffer buffer, BHeader header, LHeaderParseState s) {
    if(s.format == 1) {
      // Custom xLua format=1 (e.g. com.kingsgroup.ww2): the header omits the
      // instruction-size byte entirely; instructions remain standard 4-byte.
      return;
    }
    // 1 byte instruction size
    int instructionSize = 0xFF & buffer.get();
    if(header.debug) {
      System.out.println("-- instruction size: " + instructionSize);
    }
    if(instructionSize != 4) {
      throw new IllegalStateException("The input chunk reports an unsupported instruction size: " + instructionSize + " bytes");
    }
  }
