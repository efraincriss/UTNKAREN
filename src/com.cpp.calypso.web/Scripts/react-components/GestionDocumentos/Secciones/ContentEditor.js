import React, { Component } from "react"
import {
  EditorState,
  ContentState,
  convertFromHTML,
  convertToRaw,
} from "draft-js"
import { Editor } from "react-draft-wysiwyg"
import draftToHtml from "draftjs-to-html"

export class ContentEditor extends Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Contenido: props.Contenido ? props.Contenido : "",
      },
      editorState: EditorState.createEmpty(),
      content: "",
    }
  }

  createState = () => {
    this.setState({
      editorState: EditorState.createWithContent(
        ContentState.createFromBlockArray(
          convertFromHTML(this.state.data.Contenido)
        )
      ),
    })
  }

  componentWillMount() {
    console.log("actualizando")
    this.createState()
  }

  onEditorStateChange = (editorState) => {
    this.setState({
      editorState,
      content: draftToHtml(convertToRaw(editorState.getCurrentContent())),
    })
    this.props.updatedContent(
      draftToHtml(convertToRaw(editorState.getCurrentContent()))
    )
  }

  render() {
    const { editorState } = this.state
    

    return (
      <Editor
        editorState={editorState}
        wrapperClassName="demo-wrapper"
        editorClassName="demo-editor"
        onEditorStateChange={this.onEditorStateChange}
        toolbarHidden = {this.props.toolbarHidden}
      
      />
    )
  }
}
