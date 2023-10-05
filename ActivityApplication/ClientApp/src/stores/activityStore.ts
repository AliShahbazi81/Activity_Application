import {makeAutoObservable, runInAction} from "mobx";
import {Activity} from "../types/activity";
import agent from "../api/agent";
import { v4 as uuid } from 'uuid';

export default class ActivityStore
{
	  activityRegistry = new Map<string, Activity>();
	  selectedActivity: Activity | undefined = undefined
	  editMode = false
	  loading = false
	  loadingInitial = false
	  constructor()
	  {
			makeAutoObservable(this)
	  }
	  
	  setLoadingInitial = (state: boolean) =>
	  {
			this.loadingInitial = state;
	  }
	  
	  // Sort the activities list by Date
	  get activitiesByDate()
	  {
			return Array.from(this.activityRegistry.values())
				  .sort((a, b) => Date.parse(a.date) - Date.parse(b.date))
	  }
	  
	  // Group the activities based on their date
	  get groupedActivities()
	  {
			return Object.entries(
				  this.activitiesByDate.reduce((activities, activity) => {
						const date = activity.date;
						// If the activity is in a same date, then add to the others, otherwise, create a new array
						activities[date] = activities[date] ? [...activities[date], activity] : [activity];
						return activities
				  }, {} as {[key: string]: Activity[]})
			)
	  }

	  loadActivities = async () => {
			this.setLoadingInitial(true)
			try {
				  const activities = await agent.Activities.list();
				  runInAction(() => {  // Wrap state changes in runInAction
						activities.forEach(activity => {
							  activity.date = activity.date.split('T')[0];
							  this.activityRegistry.set(activity.id, activity)
						});
						this.loadingInitial = false;
				  });
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
			}
			else
			{
				  this.setLoadingInitial(true)
				  try 
				  {
						activity = await agent.Activities.details(id);
						runInAction(() => {
							  this.selectedActivity = activity;
						})
						this.setActivityDate(activity)
						this.setLoadingInitial(false);
						return activity;
				  }
				  catch (error)
				  {
						console.log(error)
						this.setLoadingInitial(false)
				  }
			}
			
			
	  }
	  
	  private getActivity = (id: string) => {
			// The method down below, returns either An Activity OR Undefined. We have to check the returned value
			return this.activityRegistry.get(id)
	  }
	  
	  private setActivityDate = (activity: Activity) => {
			 activity.date = activity.date.split("T")[0]
			this.activityRegistry.set(activity.id, activity)
	  }
	  
	  createActivity = async (activity: Activity) => {
			activity.id	= uuid();
			try {
				  await agent.Activities.create(activity)
				  // If successfully created, then we have to update our activities
				  runInAction(() => {
						// this.activities.push(activity)
						this.activityRegistry.set(activity.id, activity)
						this.selectedActivity = activity
						this.editMode = false
						this.loading = false
				  })
			}
			catch (error)
			{
				  console.log(error)
				  runInAction(() => {
						this.loading = false
				  })
			}
	  }
	  
	  updateActivity = async (activity: Activity) => {
			try {
				  await agent.Activities.update(activity)
				  runInAction(() => {
						// this.activities = [...this.activities.filter(x => x.id !== activity.id), activity]
						this.activityRegistry.set(activity.id, activity)
						this.selectedActivity = activity
						this.editMode = false
						this.loading = false
				  })
			}
			catch (error)
			{
				  console.log(error)
				  runInAction(() => {
						this.loading = false
				  })
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
			}
			catch (error)
			{
				  console.log(error)
				  runInAction(() => {
						this.loading = false
				  })
			}
	  }
	  
}