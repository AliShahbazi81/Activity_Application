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

	  get isLoggedIn() {
			return !!this.user;
	  }

	  //! Since we have added "reaction" method to our userStore for setToken, whatever happens to the token that reaction will be called.
	  // Hence, setItem and removeItem for the localStorage would be useless, that is why they have been commented out
	  login = async (creds: UserFormValues) => {
			try {
				  const user = await agent.Account.login(creds);
				  // After receiving user's cred, send the returned token to the store in order to be saved in the local storage
				  store.commonStore.setToken(user.token)
				  runInAction(() => this.user = user)
				  // Navigate user to activities after successful login
				  await router.navigate("/activities")
				  store.modalStore.closeModal() 
			}
			catch (error)
			{
				  throw error;
			}
	  }

	  logout = async () => {
			store.commonStore.setToken(null);
			/*? localStorage.removeItem("jwt"); */
			this.user = null;
			await router.navigate("/")
	  }

	  getUser = async () => {
			try {
				  const user = await agent.Account.current();
				  runInAction(() => this.user = user)
			} catch (error) {
				  console.log(error)
			}
	  }
}