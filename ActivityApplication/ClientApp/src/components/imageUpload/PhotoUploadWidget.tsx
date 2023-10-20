import React, {useEffect, useState} from "react";
import {Button, Grid, Header} from "semantic-ui-react";
import PhotoWidgetDropzone from "./PhotoWidgetDropzone";
import PhotoWidgetCropper from "./PhotoWidgetCropper";

interface Props{
	  loading: boolean
	  uploadPhoto: (file: Blob) => void;
}

export default function PhotoUploadWidget({loading, uploadPhoto}: Props)
{
	  // Since we want to show a preview of uploaded image, we will use useState hook
	  const[files, setFiles] = useState<any>([]);
	  // Get the result of cropping image using PhotoWidgetCropper to send to our server side
	  const [cropper, setCropper] = useState<Cropper>();
	  
	  function onCrop()
	  {
			cropper?.getCroppedCanvas().toBlob(blob => uploadPhoto(blob!))
	  }

	  //! The useEffect down below ensures that the preview files disposes from user's memory after we are done with the file
	  useEffect(() => {
			return () => {
				  files.forEach((file:any) => URL.revokeObjectURL(file.preview))
			}
	  }, [files]);
	  
	  return(
			<Grid>
				  <Grid.Column width={4}>
						<Header sub color={"teal"} content={"Step 1 - Add Photo"}/>
						<PhotoWidgetDropzone setFiles = {setFiles}/>
				  </Grid.Column>
				  		<Grid.Column width={1}/>
				  <Grid.Column width={4}>
						<Header sub color={"teal"} content={"Step 2 - Resize Photo"}/>
						{/* files[0] means that we are allowing user to upload 1 photo at a time */}
						{files && files.length > 0 && (
							  <PhotoWidgetCropper setCropper={setCropper} imagePreview={files[0].preview}/>
						)}
				  </Grid.Column>
				  		<Grid.Column width={1}/>
				  <Grid.Column width={4}>
						<Header sub color={"teal"} content={"Step 3 - Preview & Upload"}/>
						{files && files.length > 0 &&
                            <>
                                <div className={"img-preview"} style={{minHeight: 200, overflow: "hidden"}}/>
                                <Button.Group widths={2}>
                                    <Button
                                        positive
										onClick={onCrop} 
										icon={"check"} 
										loading={loading}/>
                                    <Button 
										disabled={loading} 
										onClick={() => setFiles([])} 
										icon={"close"}/>
                                </Button.Group>
                            </>
						}
				  </Grid.Column>
			</Grid>
	  )
}