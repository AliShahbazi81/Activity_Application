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
	  loadingInitial = true
	  constructor()
	  {
			makeAutoObservable(this)
	  }
	  
	  // Sort the activities list by Date
	  get activitiesByDate()
	  {
			return Array.from(this.activityRegistry.values())
				  .sort((a, b) => Date.parse(a.date) - Date.parse(b.date))
	  }

	  loadActivities = async () => {
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
	  
	  selectActivity = (id: string) => {
			// this.selectedActivity = this.activities.find(act => act.id === id)
			//! The code down below is better in terms of coding rather than the above
			this.selectedActivity = this.activityRegistry.get(id)
	  }
	  
	  cancelSelectedActivity = () => {
			this.selectedActivity = undefined
	  }
	  
	  openForm = (id?: string) => {
			id ? this.selectActivity(id) : this.cancelSelectedActivity();
			this.editMode = true
	  }
	  
	  closeForm = () => {
			this.editMode = false
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
						if (this.selectedActivity?.id === id) this.cancelSelectedActivity()
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