import { observer } from "mobx-react-lite";
import React, {SyntheticEvent, useState} from "react";
import {Card, Header, Tab, Image, Grid, Button} from "semantic-ui-react";
import {Photo, Profile} from "../../types/profile";
import {useStore} from "../../stores/store";
import PhotoUploadWidget from "../../components/imageUpload/PhotoUploadWidget";

interface Props{
	  profile: Profile
}

export default observer(function ProfilePhotos({profile}: Props)
{
	  const {profileStore: {isCurrentUser, uploadPhoto, uploading, loading, setMainPhoto, deletePhoto}} = useStore();
	  const [addPhotoMode, setAddPhotoMode] = useState(false);
	  const [target, setTarget] = useState('');
	  
	  function handleSetMainPhoto (photo: Photo, e: SyntheticEvent<HTMLButtonElement>)
	  {
			setTarget(e.currentTarget.name);
			setMainPhoto(photo);
	  }
	  
	  function handleDeletePhoto (photo: Photo, e: SyntheticEvent<HTMLButtonElement>)
	  {
			setTarget(e.currentTarget.name)
			deletePhoto(photo);
	  }
	  
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
												  {isCurrentUser && (
														<Button.Group fluid widths={2}>
															  {/* 
															   */}
															  <Button 
																	basic 
																	color={"green"} 
																	content={"Main"}
																	// For loading indicator, name and target inside the loading indicator is important. If we do not
																	// 	want to see all the buttons showing loading indicator after an action, we have to differentiate the name
																	// 	and target inside each of our buttons 
																	name={"Main" + photo.publicId}
																	disabled={photo.isMain}
																	loading={target === "Main" + photo.publicId && loading}
																	onClick={e => handleSetMainPhoto(photo, e)}
															  />
															  <Button 
																	basic
																	color={"red"}
																	icon={"trash"}
																	name={photo.publicId}
																	loading={target === photo.publicId && loading}
																	onClick={e => handleDeletePhoto(photo, e)}
																	// If the photo is user's main, they cannot delete the photo
																	disabled={photo.isMain}
															  />
														</Button.Group>
												  )}
											</Card>
									  ))}

								</Card.Group>
						  )}
					</Grid.Column>
			  </Grid>
		</Tab.Pane>			
	  )
})