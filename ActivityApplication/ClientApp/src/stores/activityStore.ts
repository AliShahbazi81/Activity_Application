import {makeAutoObservable, runInAction} from "mobx";
import {Activity, ActivityFormValues} from "../types/activity";
import agent from "../api/agent";
import {format} from "date-fns";
import {store} from "./store";
import {Profile} from "../types/profile";
import {v4 as uuid} from 'uuid';

export default class ActivityStore {
	  activityRegistry = new Map<string, Activity>();

	  selectedActivity: Activity | undefined = undefined

	  loading = false

	  loadingInitial = false

	  constructor() {
			makeAutoObservable(this)
	  }

	  // Sort the activities list by Date
	  get activitiesByDate() {
			return Array.from(this.activityRegistry.values())
				  .sort((a, b) => a.date!.getTime() - b.date!.getTime())
	  }

	  // Group the activities based on their date
	  get groupedActivities() {
			return Object.entries(
				  this.activitiesByDate.reduce((activities, activity) => {
						const date = format(activity.date!, "dd MMM yyyy")
						// If the activity is in a same date, then add to the others, otherwise, create a new array
						activities[date] = activities[date] ? [...activities[date], activity] : [activity];
						return activities
				  }, {} as { [key: string]: Activity[] })
			)
	  }

	  setLoadingInitial = (state: boolean) => {
			this.loadingInitial = state;
	  }

	  loadActivities = async () => {
			this.setLoadingInitial(true)
			try {
				  const activities = await agent.Activities.list();
				  activities.forEach(activity => {
						this.setActivity(activity);
				  });
				  this.loadingInitial = false;
			} catch (error) {
				  runInAction(() => {  // Wrap state changes in runInAction
						this.loadingInitial = false;
				  });
				  console.log(error);
			}
	  };

	  loadActivity = async (id: string) => {
			/*! First, we will check if we do have the activity in the list in which we got from the beginning. 
			* If the activity is found, then we will simply return the activity
			* Otherwise, we will send a request to the server in order to get it's using its ID 
			*! NOTE: This will reduce the number of requests to the server.*/
			let activity = this.getActivity(id)
			if (activity) {
				  this.selectedActivity = activity;
				  return activity;
			} else {
				  this.setLoadingInitial(true)
				  try {
						activity = await agent.Activities.details(id);
						runInAction(() => {
							  this.selectedActivity = activity;
						})
						this.setActivity(activity)
						this.setLoadingInitial(false);
						return activity;
				  } catch (error) {
						console.log(error)
						this.setLoadingInitial(false)
				  }
			}


	  }

	  createActivity = async (activity: ActivityFormValues) => {
			// When creating an activity, the creator of the activity is the first Attendee and the host of the activity
			const user = store.userStore.user;
			const profile = new Profile(user!);
			try {
				  activity.id = uuid();
				  activity.hostUsername = user!.username;
				  activity.attendees = [profile];

				  await agent.Activities.create(activity)
				  const newActivity = new Activity(activity);
				  // Set the host's username to the creator of the activity using their username
				  /*newActivity.hostUsername = user!.username;*/
				  // Set the first attendee of the activity - the creator of the activity
				  /*newActivity.attendees = [profile]*/
				  this.setActivity(newActivity);
				  // If successfully created, then we have to update our activities
				  runInAction(() => {
						this.selectedActivity = newActivity;
				  })
			} catch (error) {
				  console.log(error)
			}
	  }

	  updateActivity = async (activity: ActivityFormValues) => {
			try {
				  const originalActivity = this.getActivity(activity.id!);
				  activity.hostUsername = originalActivity?.hostUsername;
				  activity.attendees = originalActivity?.attendees;
				  await agent.Activities.update(activity)
				  runInAction(() => {
						if (activity.id) {
							  // Everything inside the getActivity will be overwritten with the activity. Just the ones which have changed
							  const updatedActivity = {...this.getActivity(activity.id), ...activity}
							  this.activityRegistry.set(activity.id, updatedActivity as Activity)
							  this.selectedActivity = updatedActivity as Activity
						}
				  })
			} catch (error) {
				  console.log(error)
			}
	  }

	  deleteActivity = async (id: string) => {
			try {
				  await agent.Activities.delete(id)
				  runInAction(() => {
						// this.activities = [...this.activities.filter(x => x.id !== id)]
						this.activityRegistry.delete(id)
						this.loading = false
				  })
			} catch (error) {
				  console.log(error)
				  runInAction(() => {
						this.loading = false
				  })
			}
	  }

	  updateAttendance = async () => {
			const user = store.userStore.user;
			this.loading = true;
			try {
				  await agent.Activities.attend(this.selectedActivity!.id);
				  // Setting properties if the user goes or not going to the Activity
				  runInAction(() => {
						// If user is going, and they do not want to go anymore -> Is Going = false
						if (this.selectedActivity?.isGoing) {
							  // Delete the current user from the list of attendees for the specific and selected Activity
							  this.selectedActivity.attendees = this.selectedActivity.attendees?.filter(a => a.username !== user?.username);
							  this.selectedActivity.isGoing = false;
						}
						// Else, Is going -> true
						else {
							  const attendee = new Profile(user!)
							  // Join the current user to the list of attendees for the specific and selected Activity
							  this.selectedActivity?.attendees?.push(attendee);
							  this.selectedActivity!.isGoing = true;
						}
						this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
				  })
			} catch (error) {
				  console.log(error)
			} finally {
				  runInAction(() => this.loading = false)
			}
	  }

	  cancelActivityToggle = async () => {
			this.loading = true;
			try {
				  await agent.Activities.attend(this.selectedActivity!.id);
				  runInAction(() => {
						this.selectedActivity!.isCancelled = !this.selectedActivity!.isCancelled;
						this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
				  })
			} catch (error) {
				  console.log(error)
			} finally {
				  runInAction(() => {
						this.loading = false;
				  })
			}
	  }

	  private getActivity = (id: string) => {
			// The method down below, returns either An Activity OR Undefined. We have to check the returned value
			return this.activityRegistry.get(id)
	  }

	  private setActivity = (activity: Activity) => {

			const user = store.userStore.user;

			// If user is Authenticated
			if (user) {
				  // If the username of the user is found in the attendees, then return true
				  activity.isGoing = activity.attendees!.some(
						a => a.username === user.username
				  )
				  // If the username of the user is equal with the username of activity's creator. return true
				  activity.isHost = activity.hostUsername === user.username

				  // Get the profile of activity's creator
				  activity.host = activity.attendees?.find(u => u.username === activity.hostUsername)
			}

			activity.date = new Date(activity.date!)
			this.activityRegistry.set(activity.id, activity)
	  }
}