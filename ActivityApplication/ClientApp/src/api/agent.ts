import axios, {AxiosResponse} from "axios";

// Configure default URL for APIs
axios.defaults.baseURL = "https://localhost:7290/api";

// After calling API, we will get a response
const responseBody = (response: AxiosResponse) => response.data;

// Declaring types of requests
const requests = {
	  get: (url: string) => axios.get(url).then(responseBody),
	  post:(url: string, body: {}) => axios.post(url, body).then(responseBody),
	  put:(url: string, body: {}) => axios.put(url,  body).then(responseBody),
	  del:(url: string) => axios.delete(url).then(responseBody)
}

// Implementing the requests for each service/ section
const Activities ={
	  list: () => requests.get("/Activity/Get")
}

// Configuring which requests can be accessed in the application
const agent = {
	  Activities
}

// Export the agents in which we wish to use in the application
export default agent;