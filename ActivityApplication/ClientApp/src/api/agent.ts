import axios, {AxiosError, AxiosResponse} from "axios";
import {Activity} from "../types/activity";
import {toast} from "react-toastify";
import {router} from "../router/Routes";
import {store} from "../stores/store";
import {User, UserFormValues} from "../types/user";

// Set some delay
const sleep = (delay: number) => {
	  return new Promise((resolve) => {
			setTimeout(resolve, delay)
	  })
}

// Configure default URL for APIs
axios.defaults.baseURL = "https://localhost:7290/api/";

// Add delay for getting response from our API
// Axios interceptor is smart enough to understand what is a good or bad request
//! Axios interceptor => All the errors except 200, are bad requests
axios.interceptors.response.use(async response => {

	  await sleep(1000);
	  return response
	  // In order to catch the errors, we have to specify our errors status and data when it comes back from the server
}, (error: AxiosError) => {
	  const {data, status, config} = error.response as AxiosResponse;
	  switch (status) {
			// In case of Bad request, it can be validation error, hence, we will first check if the error is validation error or just
				  // simple 400
			case 400:
				  // If user tried to access an id which does not exist in the context
				  if(config.method === "get" && data.errors.hasOwnProperty("id"))
						router.navigate("/not-found")
				  
				  // If the error is validation error
				  if (data.errors)
				  {
						// Get the validation errors one by one
						const modalStateErrors = [];
						for (const key in data.errors)
						{
							  if (data.errors[key])
									modalStateErrors.push(data.errors[key])
						}
						throw modalStateErrors.flat();
				  }
				  // If it is just a simple 400
				  else
				  {
						toast.error(data)
				  }
				  break;

			case 401:
				  toast.error("Unauthorized")
				  break;

			case 403:
				  toast.error("Forbidden")
				  break;

			case 404:
				  // When NotFound -> Navigate to NotFound page
				  router.navigate("/not-found")
				  break;

			case 500:
				  store.commonStore.setServerError(data);
				  router.navigate("/server-error");
				  break;
	  }
	  return Promise.reject(error);
})

// After calling API, we will get a response
const responseBody = <T>(response: AxiosResponse<T>) => response.data;

// Declaring types of requests
const requests = {
	  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
	  post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
	  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
	  del: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}

// Implementing the requests for each service/ section
const Activities = {
	  list: () => requests.get<Activity[]>("Activity/Get"),
	  details: (id: string) => requests.get<Activity>(`Activity/Get/${id}`),
	  create: (activity: Activity) => requests.post<void>("Activity/Create", activity),
	  update: (activity: Activity) => requests.put<void>(`Activity/Update/${activity.id}`, activity),
	  delete: (id: string) => requests.del<void>(`Activity/Delete/${id}`)
}


const Account = {
	  // Get current user using the API in the back, returning a promise of type User
	  // After Get,Post,Put, etc., the type we mention, is the type that we expect to be returned from our server
	  current: () => requests.get<User>("account"),
	  login: (user: UserFormValues) => requests.post<User>("account/login", user),
	  register: (user: UserFormValues) => requests.post<User>("account/register", user)
}

// Configuring which requests can be accessed in the application
const agent = {
	  Activities,
	  Account
}

// Export the agents in which we wish to use in the application
export default agent;