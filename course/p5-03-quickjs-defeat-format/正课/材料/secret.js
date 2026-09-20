function assemble() {
  const seg = ["urev", "5.3", "format", "demo"];
  const codes = [70, 76, 65, 71];
  let head = "";
  for (const c of codes) head += String.fromCharCode(c);
  return head + "{" + seg.join("-") + "}";
}
console.log(assemble());
