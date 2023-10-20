import React from "react";
import {Cropper} from "react-cropper";
import "cropperjs/dist/cropper.css";

interface Props{
	  imagePreview: string
	  setCropper: (cropper: Cropper) => void;
}

export default function PhotoWidgetCropper({imagePreview, setCropper}: Props)
{
	  return(
			/* Since we want to save only square image in our application, 
			we will use 2 props initialAspectRatio and aspectRation for the Cropper*/
			<Cropper 
				src={imagePreview}
				style={{height: 200, width: "100%"}}
				initialAspectRatio={1}
				aspectRatio={1}
				preview={".img-preview"}
				guides={false}
				viewMode={1}
				autoCropArea={1}
				background={false}
				onInitialized={cropper => setCropper(cropper)}
			/>
	  )
}