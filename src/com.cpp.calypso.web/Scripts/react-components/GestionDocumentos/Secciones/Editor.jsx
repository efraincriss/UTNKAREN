import * as React from "react";
import Jodit from "jodit";


const html = `
<p>Hey this <strong>editor</strong> rocks 😀</p>
`;

const Editor = () => {
  React.useEffect(() => {
    const editor = Jodit.make("#editor");
    editor.value = html;
  });

  return (
    <>
      <textarea id="editor" name="editor" />
    </>
  );
};

export default Editor;
