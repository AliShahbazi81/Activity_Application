import {ServerError} from "../types/serverError";
import {makeAutoObservable, reaction} from "mobx";

// CommonStore means that it can be used anywhere in the application.
// Hence, the fields are commonly used inside the app
export default class CommonStore {
	  error: ServerError | null = null;
	  //! When user refreshes the page, app will look for the jwt key in the localStorage.
	  // If found, retrieve the data, else return null -> user not logged in
	  token: string | null | undefined = localStorage.getItem("jwt");
	  appLoaded = false;
	  
	  constructor() {
			makeAutoObservable(this)
			
			// Whatever happens to the token, reaction method will be called.
			// Hence, we should not be worried if the user has token or not in the local storage
			reaction(
				  () => this.token,
				  token => {
						if (token)
							  localStorage.setItem("jwt", token)
						else 
							  localStorage.removeItem("jwt")
				  }
			)
	  }
	  
	  setServerError(error: ServerError)
	  {
			this.error = error;
	  }
	  
	  // Save the returned token into the local storage.
	  setToken(token: string | null | undefined)
	  {
			this.token = token;
	  }
	  
	  setAppLoaded = () => {
			this.appLoaded = true;
	  }
}