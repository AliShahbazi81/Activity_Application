import React, {useCallback} from 'react'
import {useDropzone} from 'react-dropzone'
import {Header, Icon} from "semantic-ui-react";

interface Props{
	  setFiles: (files: any) => void;
}

export default function PhotoWidgetDropzone({setFiles}: Props) {
	  // For education, we are styling the preview component here. In real world this has to be done in CSS
	  const dzStyle = {
			border: "dashed 3px #eee",
			borderColor: "#eee",
			borderRadius: "5px",
			paddingTop: "30px",
			textAlign: "center" as "center",
			height: 300
	  }
	  
	  const dzActive = {
			borderColor: "green"
	  }
	  
	  /* CallBack: When the file changes, it re-renders the codes inside - It uses Memorized version */
	  const onDrop = useCallback((acceptedFiles: object[]) => {
			setFiles(acceptedFiles.map((file: any) => Object.assign(file, {
				  /* Preview prop allows us to preview the file, which is the image here. */
				  /*! This preview file will be stored in user's memory, 
				  ! hence we will dispose it in our upper component which is PhotoUploadWidget, using useEffect */
				  preview: URL.createObjectURL(file)
			})))
	  }, [setFiles])
	  const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

	  return (
			<div {...getRootProps()} style={isDragActive ? {...dzStyle, ...dzActive} : dzStyle} >
				  <input {...getInputProps()} /> 
				  <Icon name={"upload"} size={"huge"}/>
				  <Header content={"Drop image here"} />
			</div>
	  )
}