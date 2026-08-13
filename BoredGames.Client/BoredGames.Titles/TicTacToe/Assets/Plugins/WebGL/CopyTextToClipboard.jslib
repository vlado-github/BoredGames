mergeInto(LibraryManager.library, {
  CopyTextToClipboard: function (textPtr) {
    const text = UTF8ToString(textPtr);
    if (navigator.clipboard && window.isSecureContext) {
      navigator.clipboard.writeText(text).then(() => {
        console.log("Text copied to clipboard");
      }).catch(err => {
        console.error("Failed to copy: ", err);
      });
    } else {
      // Fallback using a hidden textarea
      const textArea = document.createElement("textarea");
      textArea.value = text;
      textArea.style.position = "fixed";
      document.body.appendChild(textArea);
      textArea.focus();
      textArea.select();
      try {
        document.execCommand("copy");
        console.log("Text copied using execCommand");
      } catch (err) {
        console.error("Fallback copy failed: ", err);
      }
      document.body.removeChild(textArea);
    }
  }
});