import { observer } from "mobx-react-lite";
import React, {useState} from "react";
import {Card, Header, Tab, Image, Grid, Button} from "semantic-ui-react";
import {Profile} from "../../types/profile";
import {useStore} from "../../stores/store";
import PhotoUploadWidget from "../../components/imageUpload/PhotoUploadWidget";

interface Props{
	  profile: Profile
}

export default observer(function ProfilePhotos({profile}: Props)
{
	  const {profileStore: {isCurrentUser, uploadPhoto, uploading}} = useStore();
	  const [addPhotoMode, setAddPhotoMode] = useState(false);
	  
	  function handlePhotoUpload(file: Blob) {
			uploadPhoto(file).then(() => setAddPhotoMode(false))
	  }
	  
	  return(
		<Tab.Pane>
			  {/* Attention: Float and moving component does not work very well outside the Grid*/}
			  <Grid>
					<Grid.Column width={16}>
						  <Header 
								floated={"left"} 
								icon={"image"} 
								content={"Photos"}/>
						  {isCurrentUser && (
								<Button
									  basic
									  floated={"right"}
									  content={addPhotoMode ? "Cancel" : "Add Photo"} 
									  onClick={() => setAddPhotoMode(!addPhotoMode)}/> 
						  )}
					</Grid.Column>
					<Grid.Column width={16}>
						  {addPhotoMode ? (
								<PhotoUploadWidget uploadPhoto={handlePhotoUpload} loading={uploading}/>
						  ) : (
								<Card.Group itemsPerRow={5}>
									  {profile.photos?.map(photo => (
											<Card key={photo.publicId}>
												  <Image src={ photo.url || "assets/user.png"}/>
											</Card>
									  ))}

								</Card.Group>
						  )}
					</Grid.Column>
			  </Grid>
		</Tab.Pane>			
	  )
})