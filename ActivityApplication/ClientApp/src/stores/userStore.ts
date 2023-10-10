import {User, UserFormValues} from "../types/user";
import {makeAutoObservable} from "mobx";
import agent from "../api/agent";

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
			console.log(user)
	  }
}