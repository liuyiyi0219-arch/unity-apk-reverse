protected void parse_format(ByteBuffer buffer, BHeader header, LHeaderParseState s) {
    // 1 byte Lua "format"
    int format = 0xFF & buffer.get();
    if(format != 0) {
      throw new IllegalStateException("The input chunk reports a non-standard lua format: " + format);
    }
    s.format = format;
    if(header.debug) {
      System.out.println("-- format: " + format);
    }
  }
