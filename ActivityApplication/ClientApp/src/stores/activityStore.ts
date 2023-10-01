import {makeAutoObservable, runInAction} from "mobx";
import {Activity} from "../types/activity";
import agent from "../api/agent";

export default class ActivityStore
{
	  activities: Activity[] = []
	  selectedActivity: Activity | undefined = undefined
	  editMode = false
	  loading = false
	  loadingInitial = false
	  constructor()
	  {
			makeAutoObservable(this)
	  }

	  loadActivities = async () => {
			this.loadingInitial = true;
			try {
				  const activities = await agent.Activities.list();
				  runInAction(() => {  // Wrap state changes in runInAction
						activities.forEach(activity => {
							  activity.date = activity.date.split('T')[0];
							  this.activities.push(activity); // You probably meant this.activities here
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
			this.selectedActivity = this.activities.find(act => act.id === id)
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
	  
}