import axios, {AxiosResponse} from "axios";
import {Activity} from "../types/activity";

// Set some delay
const sleep = (delay: number) => {
	  return new Promise((resolve) => {
			setTimeout(resolve, delay)
	  })
}

// Configure default URL for APIs
axios.defaults.baseURL = "https://localhost:7290/api/Activity";

// Add delay for getting response from our API
axios.interceptors.response.use(async response => {
	  try {
			await sleep(1000);
			return response
	  } catch (error) {
			console.log(error);
			return Promise.reject(error)
	  }
})

// After calling API, we will get a response
const responseBody = <T> (response: AxiosResponse<T>) => response.data;

// Declaring types of requests
const requests = {
	  get: <T> (url: string) => axios.get<T>(url).then(responseBody),
	  post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
	  put: <T> (url: string, body: {}) => axios.put<T>(url,  body).then(responseBody),
	  del: <T> (url: string) => axios.delete<T>(url).then(responseBody)
}

// Implementing the requests for each service/ section
const Activities ={
	  list: () => requests.get<Activity[]>("/Get"),
	  details: (id: string) => requests.get<Activity>(`/Get/${id}`),
	  create: (activity: Activity) => requests.post<void>("/Create", activity),
	  update: (activity: Activity) => requests.put<void>(`/Update/${activity.id}`, activity),
	  delete: (id: string) => requests.del<void>(`/Delete/${id}`)
}

// Configuring which requests can be accessed in the application
const agent = {
	  Activities
}

// Export the agents in which we wish to use in the application
export default agent;