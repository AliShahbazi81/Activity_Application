import React, {useEffect} from "react";
import { Grid } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import { observer } from "mobx-react-lite";
import {useParams} from "react-router-dom";
import {useStore} from "../../stores/store";
import LoadingComponent from "../../components/LoadingComponent";

export default observer(function ProfilePage()
{
	  // Get user's username from root params
	  const {username} = useParams<{username: string}>();
	  const {profileStore} = useStore();
	  const {loadProfile, loadingProfile, profile} = profileStore;
	  
	  // In order to call the loadProfile method when this component renders, we have to use UseEffect
	  useEffect(() => {
			if (username)
				  loadProfile(username)
	  }, [loadProfile, username]);
	  
	  if (loadingProfile) return <LoadingComponent content={"Loading Profile..."} />
	  
	  return (
			<Grid>
				<Grid.Column width={16}>
					  {profile && 
						  <>
                              <ProfileHeader profile={profile}/>
                              <ProfileContent profile={profile}/>
						  </>
					  }
				</Grid.Column>  
			</Grid>
	  )
})