import {ChatComments} from "../types/comments";
import {HubConnection, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {makeAutoObservable, runInAction} from "mobx";
import {store} from "./store";

export default class CommentStore {
	  comments: ChatComments[] = [];
	  hubConnection: HubConnection | null = null;
	  
	  constructor() {
			makeAutoObservable(this)
	  }
	  
	  // Make hub connection
	  createHubConnection = (activityId: string) => {
			// Make sure if we do have activity before making connection
			if (store.activityStore.selectedActivity)
			{
				  this.hubConnection = new HubConnectionBuilder()
						.withUrl("https://localhost:7290/chat?activityId=" + activityId, {
							  accessTokenFactory: () => store.userStore.user?.token!
						})
						.withAutomaticReconnect()
						.configureLogging(LogLevel.Information)
						.build();
				  
				  this.hubConnection.start().catch(error => console.log("Error establishing the connection: ", error));
				  
				  // When we connect to the hub, we want to get all the comments available for the activity
				  this.hubConnection.on("LoadComments", (comments: ChatComments[]) => {
						// Update the observable in the store
						runInAction(() => {
							  this.comments = comments;
						})
				  });
				  
				  // When comments are received, it automatically pushes to the comment arrays
				  this.hubConnection.on("ReceiveComment", (comment: ChatComments) => {
						runInAction(() => {
							  this.comments.push(comment)
						})
				  })
			}
	  }
	  
	  stopHubConnection = () => {
			this.hubConnection?.stop().catch(error => console.log("Error stopping connection: ", error))
	  }
	  
	  // When we disconnect from the hub - when opening another activity - the comments will be deleted, and it will be disconnected from the current hub
	  clearComments = () => {
			this.comments = [];
			this.stopHubConnection();
	  }
	  
	  addComment = async (values: {body: string, activityId?: string}) => {
			values.activityId = store.activityStore.selectedActivity?.id as string;
			try {
				  /*! The method name has to be matched with the method's name in our backend - CommentHub */
				  await this.hubConnection?.invoke("SendComment", values.body, values.activityId)
			}
			catch (error)
			{
				  console.log(error)
			}
	  }
}