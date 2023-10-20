import {Profile} from "../types/profile";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";
import {store} from "./store";

export default class ProfileStore {
	  profile: Profile | null = null;
	  loadingProfile = false;
	  uploading = false;
	  
	  constructor() {
			makeAutoObservable(this)
	  }
	  
	  // Simply checks if the user is logged in, and if the usernames are equal -> User is Authenticated
	  get isCurrentUser()
	  {
			if (store.userStore.user && this.profile)
				  return store.userStore.user.username === this.profile.username
			return false;
	  }
	  
	  loadProfile = async (username: string) => {
			this.loadingProfile = true;
			try {
				  const profile = await agent.Profiles.get(username);
				  runInAction(() => {
						this.profile = profile;
						this.loadingProfile = false;
				  })
			}
			catch (error)
			{
				  console.log(error);
				  runInAction(() => {
						this.loadingProfile = false;
				  })
			}
	  }
	  
	  uploadPhoto = async (file: Blob) => {
			this.uploading = true;
			try 
			{
				  const response = await agent.Profiles.uploadPhoto(file);
				  const photo = response.data;
				  runInAction(() => {
						/* Since profile can be Profile or null, we check if it is null for further error */
						if (this.profile)
						{
							  this.profile.photos?.push(photo);
							  /* Check if the photo is user's main photo, based on the IsMain in our server-side */
							  if (photo.isMain && store.userStore.user)
							  {
									store.userStore.setImage(photo.url)
									this.profile.image = photo.url;
							  }
						}
						this.uploading = false;
				  })
			}
			catch (error)
			{
				  console.log(error)
				  runInAction(() => {
						this.uploading = false;
				  })
			}
	  }
}