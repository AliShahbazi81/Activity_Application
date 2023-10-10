import {ServerError} from "../types/serverError";
import {makeAutoObservable} from "mobx";

// CommonStore means that it can be used anywhere in the application.
// Hence, the fields are commonly used inside the app
export default class CommonStore {
	  error: ServerError | null = null;
	  token: string | null | undefined = null;
	  appLoaded = false;
	  
	  constructor() {
			makeAutoObservable(this)
	  }
	  
	  setServerError(error: ServerError)
	  {
			this.error = error;
	  }
	  
	  // Save the returned token into the local storage.
	  setToken(token: string | null | undefined)
	  {
			if (token)
				  localStorage.setItem("jwt", token)
			this.token = token;
	  }
	  
	  setAppLoaded = () => {
			this.appLoaded = true;
	  }
}