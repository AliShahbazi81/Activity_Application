import {User, UserFormValues} from "../types/user";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";
import {store} from "./store";
import {router} from "../router/Routes";

export default class userStore {
	  // If user is logged in, then the user would be filled with User, otherwise it would be null.
	  user: User | null = null;

	  constructor() {
			makeAutoObservable(this);
	  }

	  getIsLoggedIn() {
			return !!this.user;
	  }

	  login = async (creds: UserFormValues) => {
			const user = await agent.Account.login(creds);
			// After receiving user's cred, send the returned token to the store in order to be saved in the local storage
			store.commonStore.setToken(user.token)
			
			runInAction(() => this.user = user)
			// Navigate user to activities after successful login
			await router.navigate("/activities")
	  }
	  
	  logout = async () => {
		store.commonStore.setToken(null);
		localStorage.removeItem("jwt");
		this.user = null;
		await router.navigate("/")
	  }
}